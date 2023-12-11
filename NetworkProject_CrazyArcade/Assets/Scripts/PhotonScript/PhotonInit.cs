using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum State
{
	MAIN = 0,
	LOBBY = 1,
	ROOM = 2,
	GAME = 3
}

public class PhotonInit : MonoBehaviourPunCallbacks
{
	public static PhotonInit Instance = null;

	private bool isReady = false;

	private bool isMain = false;
	private bool isLobby = false;
	private bool isWaitingRoom = false;

	[Header("툴팁 관련 프로퍼티")]
	public Canvas ToolTipCanvas;
	public TextEffect toolTipText;

	[Header("메인 로비 관련 프로퍼티")]
	public MainInit mainRoom;

	[Header("로비 관련 프로퍼티")]
	public LobbyInit lobbyRoom;
	public List<RoomInfo> rooms = new List<RoomInfo>();

	[Header("대기방 관련 프로퍼티")]
	public WaitingRoomInit waitingRoom;
	
	void Awake()
    {
		if (Instance == null)
		{
			Instance = this;

			mainRoom = GameObject.Find("MainManager").GetComponent<MainInit>();

			ConnectToServer();

			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(ToolTipCanvas);
		}
		else if (Instance != null)
			Destroy(gameObject);
	}

    private void Update()
    {
		if(!isMain && SceneManager.GetActiveScene().name == "MainLevel")
        {
			isMain = true;
			isLobby = false;
			isWaitingRoom = false;
			if (mainRoom == null && GameObject.Find("MainManager") != null)
			{
				mainRoom = GameObject.Find("MainManager").GetComponent<MainInit>();
				mainRoom.SetUIInteractable(true);
			}
        }
		else if (!isLobby && SceneManager.GetActiveScene().name == "LobbyLevel")
        {
			isMain = false;
			isLobby = true;
			isWaitingRoom = false;
			if (lobbyRoom == null && GameObject.Find("LobbyManager") != null)
			{
				lobbyRoom = GameObject.Find("LobbyManager").GetComponent<LobbyInit>();
				lobbyRoom.InitMainLobby(PhotonNetwork.LocalPlayer.NickName);
			}
		}
		else if (!isWaitingRoom && SceneManager.GetActiveScene().name == "WaitingLevel")
        {
			isMain = false;
			isLobby = false;
			isWaitingRoom = true;
			if (waitingRoom == null && GameObject.Find("RoomManager") != null)
			{
				waitingRoom = GameObject.Find("RoomManager").GetComponent<WaitingRoomInit>();
			}
		}
	}

    public void ConnectToServer()
    {
		StartCoroutine(TryJoin(State.MAIN));
	}


	IEnumerator TryJoin(State state, int roomNum = -1)
	{
		switch(state)
		{
			case State.MAIN:
				PhotonNetwork.GameVersion = "NetworkProject Server 1.0";
				PhotonNetwork.ConnectUsingSettings();
				break;
			case State.LOBBY:
				PhotonNetwork.JoinLobby();
				break;
			case State.ROOM:
				if (roomNum != -1)
				{
					if(rooms[roomNum].PlayerCount >= 4)
                    {
						toolTipText.StartTextEffect("방이 꽉 찼습니다!", Effect.FADE);
						yield break;
                    }
					PhotonNetwork.JoinRoom(rooms[roomNum].Name);
				}
				break;
		}

		toolTipText.StartTextEffect(string.Format("{0}에 접속 중", 
			(state != State.LOBBY) && (state != State.ROOM) 
				? "서버" : (state != State.ROOM) 
					? "로비" : "방" ), 
						Effect.WAIT);

		switch (state)
		{
			case State.MAIN:
				while (PhotonNetwork.IsConnectedAndReady == false)
					yield return null;
				break;
			case State.LOBBY:
				while (PhotonNetwork.InLobby == false)
					yield return null;
				break;
			case State.ROOM:
				while (PhotonNetwork.InRoom == false)
					yield return null;
				break;
		}

		if(state != State.ROOM)
			toolTipText.StartTextEffect("접속 완료!", Effect.FADE);

		if (state == State.MAIN)
		{
			isReady = true;
			mainRoom.SetUIInteractable(true);
		}
	}

    public void ConnectToLobby()
    {
        if(PhotonNetwork.IsConnected == true)
        {
			PhotonNetwork.LoadLevel("LobbyLevel");
        }
		else
        {
			toolTipText.StartTextEffect("로비 진입에 실패하였습니다!\n서버 연결을 확인해주세요", Effect.FADE);
        }
    }

	public void LobbyConnected()
    {
		StartCoroutine(TryJoin(State.LOBBY));
	}

    //로비에 입장하였을 때 호출되는 콜백함수
    public override void OnJoinedLobby()
	{
		
	}

	public override void OnLeftLobby()
	{
		PhotonNetwork.LoadLevel("MainLevel");
	}

	//룸을 생성했을 떄의 콜백 함수
	public override void OnCreatedRoom()
	{
		
	}

    public override void OnJoinedRoom()
    {
		PhotonNetwork.LoadLevel("WaitingLevel");
	}

    public void SetPlayerName(string name)
    {
		if (!isReady)
			return;

		if (name == "")
		{
			toolTipText.StartTextEffect("이름을 정해주세요!", Effect.FADE);
			return;
		}

		PhotonNetwork.LocalPlayer.NickName = name;
		ConnectToLobby();
    }

	public bool CreateRoom(string roomName, string pw, bool isPassword)
    {
		if(rooms.Count == 6)
        {
			toolTipText.StartTextEffect("서버에 방이 꽉 찼습니다!", Effect.FADE);
			return false;
        }

		RoomOptions myRoomOptions = new RoomOptions();
		myRoomOptions.MaxPlayers = 4;

		string myRoomName = string.Empty;
		if (isPassword)
		{
			myRoomName = roomName == "" ? "[P]GameRoom" + rooms.Count + 1 : "[P]" + roomName;
			myRoomOptions.CustomRoomProperties = new Hashtable()
			{
				{ "password", pw }
			};
			myRoomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };
		}
		else
		{
			myRoomName = roomName == "" ? "GameRoom" + rooms.Count + 1 : roomName;
		}

		StartCoroutine(TryJoin(State.ROOM));
		PhotonNetwork.CreateRoom(myRoomName, myRoomOptions);
		return true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
		int roomCount = roomList.Count;
		for(int i = 0; i < roomCount; i++)
        {
			if (!roomList[i].RemovedFromList)
			{
				if (!rooms.Contains(roomList[i])) rooms.Add(roomList[i]);
				else rooms[rooms.IndexOf(roomList[i])] = roomList[i];
			}
			else if (rooms.IndexOf(roomList[i]) != -1) rooms.RemoveAt(rooms.IndexOf(roomList[i]));
        }

		lobbyRoom.RoomListRenewal(rooms);
    }

	public void EnterRoom(int roomNum, bool b_password, string password = "")
    {
		if(!b_password)
        {
			if (rooms[roomNum].CustomProperties["password"] == null)
			{
				StartCoroutine(TryJoin(State.ROOM, roomNum));
				lobbyRoom.RoomListRenewal(rooms);
			}
			else
				lobbyRoom.ShowPasswordPanel(true);
        }
		else
        {
			if ((string)rooms[roomNum].CustomProperties["password"] == password)
			{
				StartCoroutine(TryJoin(State.ROOM, roomNum));
				lobbyRoom.RoomListRenewal(rooms);
				lobbyRoom.ShowPasswordPanel(false);
			}
			else
				toolTipText.StartTextEffect("비밀번호가 틀렸습니다!", Effect.FADE);
        }
    }
}
