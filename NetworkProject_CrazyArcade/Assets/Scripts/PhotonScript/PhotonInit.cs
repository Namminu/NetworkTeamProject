using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : Photon.PunBehaviour
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
	}

    public void ConnectToServer()
    {
		StartCoroutine(TryConnect());
	}

    IEnumerator TryConnect()
    {
		toolTipText.StartTextEffect("서버에 연결 중", Effect.WAIT);

		PhotonNetwork.ConnectUsingSettings("NetworkProject Server 1.0");

		while (PhotonNetwork.connectionState == ConnectionState.Connecting)
		{
			yield return new WaitForSeconds(0.5f);
		}

		toolTipText.StartTextEffect("서버 연결 완료!", Effect.FADE);

		isReady = true;
		mainRoom.SetUIInteractable(true);
	}

    public void ConnectToLobby()
    {
        if(PhotonNetwork.connected == true)
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
		Debug.Log("FInish make a Room");
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
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

		playerName = name;
		ConnectToLobby();
    }
}
