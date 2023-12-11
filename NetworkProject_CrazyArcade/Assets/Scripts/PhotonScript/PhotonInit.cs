using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using JetBrains.Annotations;
using Photon.Realtime;
using UnityEngine.UIElements;

public class PhotonInit : MonoBehaviourPunCallbacks
{
	public static PhotonInit Instance = null;

	private bool isReady = false;
	private string playerName = "";

	private bool isMain = false;
	private bool isLobby = false;
	private bool isWaitingRoom = false;
	private bool isInGame = false;

	[Header("���� ���� ������Ƽ")]
	public Canvas ToolTipCanvas;
	public TextEffect toolTipText;

	[Header("���� �κ� ���� ������Ƽ")]
	public MainInit mainRoom;

	[Header("�κ� ���� ������Ƽ")]
	public LobbyInit lobbyRoom;

	[Header("���� ���� ������Ƽ")]
	public WaitingRoomInit waitingRoom;

	[Header("�ΰ��� ���� ������Ƽ")]
	public InGameManger InGameRoom;

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
		if (!isMain && SceneManager.GetActiveScene().name == "MainLevel")
		{
			isMain = true;
			isLobby = false;
			isWaitingRoom = false;
			isInGame = false;
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
			isInGame = false;
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
			isInGame = false;
			if (waitingRoom == null && GameObject.Find("RoomManager") != null)
			{
				waitingRoom = GameObject.Find("RoomManager").GetComponent<WaitingRoomInit>();
			}
		}
		else if (!isInGame && (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3"))
		{
			isMain = false;
			isLobby = false;
			isWaitingRoom = false;
			isInGame = true;
			if (InGameRoom == null && GameObject.Find("InGameManager") != null)
			{
				InGameRoom = GameObject.Find("InGameManager").GetComponent<InGameManger>();
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

		toolTipText.StartTextEffect("������ ���� ��", Effect.WAIT);

		while(PhotonNetwork.IsConnectedAndReady == false)
		{
			yield return new WaitForSeconds(0.5f);
		}

		toolTipText.StartTextEffect("���� ���� �Ϸ�!", Effect.FADE);

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
			toolTipText.StartTextEffect("�κ� ���Կ� �����Ͽ����ϴ�!\n���� ������ Ȯ�����ּ���", Effect.FADE);
        }
    }

    //�κ� �����Ͽ��� �� ȣ��Ǵ� �ݹ��Լ�
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

	//���� �������� ���� �ݹ� �Լ�
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
			toolTipText.StartTextEffect("�̸��� �����ּ���!", Effect.FADE);
			return;
		}

		playerName = name;
		ConnectToLobby();
    }

	//����� �� �� �Ѱ��ִ� �Լ�
	public void NextScene()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			PhotonNetwork.LoadLevel("WaitingLevel");
        }
        else if(Input.GetKeyDown(KeyCode.F1))
        {
            PhotonNetwork.JoinOrCreateRoom("room1", new RoomOptions { MaxPlayers = 4 }, null);

			PhotonNetwork.LoadLevel("Level1");
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			PhotonNetwork.LoadLevel("Level2");
		}
		else if(Input.GetKeyDown(KeyCode.F3))
		{
			PhotonNetwork.LoadLevel("Level3");
		}
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined Room");
        Debug.Log(PhotonNetwork.IsMasterClient.ToString());
        InGameManger IGM = GameObject.Find("InGameManager").GetComponent<InGameManger>();
		StartCoroutine(IGM.temp_CreatePlayer());
		IGM.GameStart();
	}

}
