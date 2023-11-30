using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : Photon.PunBehaviour
{
	public InputField playerInput;
	bool isGameStart = false;
	string playerName = "";

	void Awake()
    {
        //���� ������ �и�
        PhotonNetwork.ConnectUsingSettings("NetworkProject Server 1.0");
    }

    //�κ� �����Ͽ��� �� ȣ��Ǵ� �ݹ��Լ�
	public override void OnJoinedLobby()
	{
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinRandomRoom();
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

		//�÷��̾� ����
		StartCoroutine(this.CreatePlayer());
	}

	//�÷��̾� ���� �ڷ�ƾ
	/* ���� ���� �÷��̾� ĳ���͸� ������ ��ġ���� �����ϵ��� ���� ���� */
	IEnumerator CreatePlayer()
	{
		GameObject tempPlayer = PhotonNetwork.Instantiate("player", new Vector2(0, 0), Quaternion.identity, 0);
        tempPlayer.GetComponent<PlayerController>().playerstat.playerName.text = playerName;
        yield return null;
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	/// <summary>
	///  ĳ���� ���̵� ���� �κ������ �̵�
	/// </summary>
    public void MoveToLobby()
    {
        SceneManager.LoadScene("LobbyLevel");
        //PhotonNewtwork.LoadLevel("LobbyLevel");
		playerName = playerInput.text;
		isGameStart = true;
    }
}
 