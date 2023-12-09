using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : Photon.PunBehaviour
{
	public static PhotonInit instance = null;
	PhotonView pv;

	public InputField playerInput;
	private bool isGameStart = false;

	WaitingRoomInit waitingRoom;

	public static PhotonInit Instance
	{
		get
		{
			if (!instance)
			{
				instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

				if (instance == null)
					Debug.Log("no singleton obj");
			}

			return instance;
		}
	}

	void Awake()
    {
        //���� ������ �и�
        PhotonNetwork.ConnectUsingSettings("NetworkProject Server 1.0");

		DontDestroyOnLoad(gameObject);
	}

    //�κ� �����Ͽ��� �� ȣ��Ǵ� �ݹ��Լ�
	public override void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby");

		// ���� �� ���� �Ұ����ϰ�
        //PhotonNetwork.JoinRandomRoom();
	}

	//���� �� ���忡 �������� ���� �ݹ� �Լ�
	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("No Room");
		PhotonNetwork.CreateRoom("NewCreate Room");
	}

	//���� �������� ���� �ݹ� �Լ�
	public override void OnCreatedRoom()
	{
		Debug.Log("FInish make a Room");
	}

	//�뿡 ����Ǿ��� ��� ȣ��Ǵ� �ݹ��Լ�
	public override void OnJoinedRoom()
	{
		Debug.Log("Joined Room");

		PhotonNetwork.LoadLevel("WaitingLevel");
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

    public void SetPlayerName()
    {
        Debug.Log(playerInput.text + "�� �Է��ϼ̽��ϴ�");
        //PhotonNewtwork.LoadLevel("LobbyLevel");
        SceneManager.LoadScene("LobbyLevel");
        PlayerName.instance.playerName = playerInput.text;
        isGameStart = true;
    }
}
