using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonInit : Photon.PunBehaviour
{
    void Awake()
    {
        //서버 버전별 분리
        PhotonNetwork.ConnectUsingSettings("NetworkProject Server 1.0");
    }

    //로비에 입장하였을 때 호출되는 콜백함수
	public override void OnJoinedLobby()
	{
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinRandomRoom();
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

		//플레이어 생성
		StartCoroutine(this.CreatePlayer());
	}

	//플레이어 생성 코루틴
	/* 추후 랜덤 플레이어 캐릭터를 지정된 위치에서 생성하도록 수정 예정 */
	IEnumerator CreatePlayer()
	{
		PhotonNetwork.Instantiate("Player", new Vector2(0, 0), Quaternion.identity, 0);
		yield return null;
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}
 