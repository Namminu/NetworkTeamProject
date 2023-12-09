using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class WaitingRoomInit : Photon.PunBehaviour
{
	PhotonInit photonManger;
	PhotonView pv;

	[Header("Waiting Room")]
	public GameObject[] playerImages = new GameObject[4];
	private GameObject[] playerImagePos = new GameObject[4];
	private List<GameObject> playerObjects = new List<GameObject>();
	private int myWaitingIndex;
	private int myCharacterIndex;

    private void Start()
    {
		photonManger = GameObject.Find("PhotonManager").GetComponent<PhotonInit>();

		InitWaitingRoom();
    }

    #region 대기방 관련 메소드
    public void InitWaitingRoom()
	{
		pv = GetComponent<PhotonView>();

		for (int i = 0; i < playerImagePos.Length; i++)
			playerImagePos[i] = GameObject.Find("PlayerImgPos" + (i + 1));

		myWaitingIndex = playerObjects.Count + 1;

		EnterWaitingRoom();
	}

	// 캐릭터 중복을 막기위한 함수
	int SetCharacterIndex()
	{
		int randNum;
		List<int> exclusionNum = new List<int>();

		for (int i = 0; i < playerObjects.Count; i++)
			exclusionNum.Add(playerObjects[i].GetComponent<CharacterInfo>().CharacterNumber);

		do
		{
			randNum = Random.Range(0, playerImages.Length);
		} while (exclusionNum.Contains(randNum));

		return randNum;
	}

	// 플레이어가 대기방에 입장할때 실행되는 함수
	void EnterWaitingRoom()
	{
		// 만약 대기방에 플레이어가 꽉차있다면 
		if (playerObjects.Count >= 4)
		{
			Debug.Log("대기방이 꽉찼습니다!");
			PhotonNetwork.LoadLevel("LobbyLevel");
			return;
		}

		// 캐릭터 랜덤으로 설정함 -> 방에 이미 들어와있는 플레이어들의 캐릭터랑 중복되지 않게함
		if (playerObjects.Count != 0)
			myCharacterIndex = SetCharacterIndex();
		else
			myCharacterIndex = Random.Range(0, playerImages.Length);

		// 모든 플레이어들의 Player패널에다가 입장한 플레이어 이미지 생성 
		pv.RPC("PlayerEnter", PhotonTargets.All, playerObjects.Count, myCharacterIndex);
	}

	// 플레이어가 방에 나갈때 실행되는 함수
	void LeaveWaitingRoom()
	{
		// 모든 플레이어들의 Player패널에 나간 플레이어 이미지 없앰
		pv.RPC("PlayerLeave", PhotonTargets.All, myWaitingIndex);
	}

	[PunRPC]
	void PlayerEnter(int index, int playerImgIndex)
	{
		InstantiatePlayerImage(index, playerImgIndex);
	}

	[PunRPC]
	void PlayerLeave(int index)
	{
		DestroyPlayerImage(index);
	}

	void InstantiatePlayerImage(int index, int playerImgIndex)
	{
		Debug.Log("Create Image at: " + index);
		GameObject playerWaitingObj = Instantiate(playerImages[playerImgIndex], playerImagePos[index].transform);
		playerObjects.Add(playerWaitingObj);
	}

	void DestroyPlayerImage(int index)
	{
		// Destroy할 오브젝트 인덱스 찾기
		GameObject targetOjbectToDestroy = playerObjects[index - 1];
		playerObjects.Remove(targetOjbectToDestroy);
		Destroy(targetOjbectToDestroy);

		// 만약 자신의 인덱스가 아니라면 상대방이 나간것이므로 Player패널 업데이트
		UpdatePlayerPanel(index);

		if (myWaitingIndex > index)
			myWaitingIndex -= 1;
	}

	// 플레이어 패널 업데이트
	void UpdatePlayerPanel(int leavedPlayerIndex)
	{
		for (int i = leavedPlayerIndex; i < playerImagePos.Length; i++)
		{
			if (playerImagePos[i].transform.childCount != 0)
			{
				GameObject targetObject = playerImagePos[i].transform.GetChild(0).gameObject;
				targetObject.transform.SetParent(playerImagePos[i - 1].transform);
				targetObject.transform.localPosition = Vector3.zero;
				targetObject.transform.localScale = Vector3.one;
			}
		}
	}

	// 디버깅 하기 위해 임시로 사용하는 업데이트문
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			LeaveWaitingRoom();
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			EnterWaitingRoom();
		}
	}
    #endregion
}
