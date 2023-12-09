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

	[Header("���� ���� ������Ƽ")]
	public Canvas ToolTipCanvas;
	public TextEffect toolTipText;

	[Header("���� �κ� ���� ������Ƽ")]
	public MainInit mainRoom;

	[Header("�κ� ���� ������Ƽ")]
	public LobbyInit lobbyRoom;

	[Header("���� ���� ������Ƽ")]
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
		toolTipText.StartTextEffect("������ ���� ��", Effect.WAIT);

		PhotonNetwork.ConnectUsingSettings("NetworkProject Server 1.0");

		while (PhotonNetwork.connectionState == ConnectionState.Connecting)
		{
			yield return new WaitForSeconds(0.5f);
		}

		toolTipText.StartTextEffect("���� ���� �Ϸ�!", Effect.FADE);

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
			toolTipText.StartTextEffect("�̸��� �����ּ���!", Effect.FADE);
			return;
		}

		playerName = name;
		ConnectToLobby();
    }
}
