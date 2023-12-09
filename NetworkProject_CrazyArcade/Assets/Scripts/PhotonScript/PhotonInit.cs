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
        //서버 버전별 분리
        PhotonNetwork.ConnectUsingSettings("NetworkProject Server 1.0");

		DontDestroyOnLoad(gameObject);
	}

    //로비에 입장하였을 때 호출되는 콜백함수
	public override void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby");

		// 랜덤 룸 입장 불가능하게
        //PhotonNetwork.JoinRandomRoom();
	}

	//랜덤 룸 입장에 실패했을 때의 콜백 함수
	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("No Room");
		PhotonNetwork.CreateRoom("NewCreate Room");
	}

	//룸을 생성했을 떄의 콜백 함수
	public override void OnCreatedRoom()
	{
		Debug.Log("FInish make a Room");
	}

	//룸에 입장되었을 경우 호출되는 콜백함수
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
        Debug.Log(playerInput.text + "를 입력하셨습니다");
        //PhotonNewtwork.LoadLevel("LobbyLevel");
        SceneManager.LoadScene("LobbyLevel");
        PlayerName.instance.playerName = playerInput.text;
        isGameStart = true;
    }
}
