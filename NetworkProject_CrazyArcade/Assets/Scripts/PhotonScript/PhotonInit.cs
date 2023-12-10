using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using JetBrains.Annotations;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
	public static PhotonInit Instance = null;

	private bool isReady = false;
	private string playerName = "";

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
				lobbyRoom.InitMainLobby(playerName);
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

		NextScene();
	}

    public void ConnectToServer()
    {
		StartCoroutine(TryConnect());
	}

    IEnumerator TryConnect()
    {
		PhotonNetwork.GameVersion = "NetworkProject Server 1.0";
		PhotonNetwork.ConnectUsingSettings();

		toolTipText.StartTextEffect("서버에 연결 중", Effect.WAIT);

		while(PhotonNetwork.IsConnectedAndReady == false)
		{
			yield return new WaitForSeconds(0.5f);
		}

		toolTipText.StartTextEffect("서버 연결 완료!", Effect.FADE);

		isReady = true;
		mainRoom.SetUIInteractable(true);
	}

    public void ConnectToLobby()
    {
        if(PhotonNetwork.IsConnected == true)
        {
			PhotonNetwork.JoinLobby();
        }
		else
        {
			toolTipText.StartTextEffect("로비 진입에 실패하였습니다!\n서버 연결을 확인해주세요", Effect.FADE);
        }
    }

    //로비에 입장하였을 때 호출되는 콜백함수
    public override void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby");

		PhotonNetwork.LoadLevel("LobbyLevel");
	}

	public override void OnLeftLobby()
	{
		base.OnLeftLobby();
		Debug.Log("Left Lobby");

		PhotonNetwork.LoadLevel("MainLevel");
	}

	//룸을 생성했을 떄의 콜백 함수
	public override void OnCreatedRoom()
	{
		Debug.Log("Finish make a Room");
	}

	//void OnGUI()
	//{
	//	GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	//}

    public void SetPlayerName(string name)
    {
		if (!isReady)
			return;

		if (name == "")
		{
			toolTipText.StartTextEffect("이름을 정해주세요!", Effect.FADE);
			return;
		}

		playerName = name;
		ConnectToLobby();
    }

	//디버깅 용 방 넘겨주는 함수
	public void NextScene()
	{
		if(Input.GetKey(KeyCode.Escape))
		{
			PhotonNetwork.JoinOrCreateRoom("room1", new RoomOptions { MaxPlayers = 4 }, null);
			PhotonNetwork.LoadLevel("WaitingLevel");
		}
        else if(Input.GetKey(KeyCode.F1))
        {
			PhotonNetwork.LoadLevel("Level1");
		}
		else if (Input.GetKey(KeyCode.F2))
		{
			PhotonNetwork.LoadLevel("Level2");
		}
		else if(Input.GetKey(KeyCode.F3))
		{
			PhotonNetwork.LoadLevel("Level3");
		}
	}
}
