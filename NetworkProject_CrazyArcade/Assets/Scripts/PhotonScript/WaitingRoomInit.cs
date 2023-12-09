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

    #region ���� ���� �޼ҵ�
    public void InitWaitingRoom()
	{
		pv = GetComponent<PhotonView>();

		for (int i = 0; i < playerImagePos.Length; i++)
			playerImagePos[i] = GameObject.Find("PlayerImgPos" + (i + 1));

		myWaitingIndex = playerObjects.Count + 1;

		EnterWaitingRoom();
	}

	// ĳ���� �ߺ��� �������� �Լ�
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

	// �÷��̾ ���濡 �����Ҷ� ����Ǵ� �Լ�
	void EnterWaitingRoom()
	{
		// ���� ���濡 �÷��̾ �����ִٸ� 
		if (playerObjects.Count >= 4)
		{
			Debug.Log("������ ��á���ϴ�!");
			PhotonNetwork.LoadLevel("LobbyLevel");
			return;
		}

		// ĳ���� �������� ������ -> �濡 �̹� �����ִ� �÷��̾���� ĳ���Ͷ� �ߺ����� �ʰ���
		if (playerObjects.Count != 0)
			myCharacterIndex = SetCharacterIndex();
		else
			myCharacterIndex = Random.Range(0, playerImages.Length);

		// ��� �÷��̾���� Player�гο��ٰ� ������ �÷��̾� �̹��� ���� 
		pv.RPC("PlayerEnter", PhotonTargets.All, playerObjects.Count, myCharacterIndex);
	}

	// �÷��̾ �濡 ������ ����Ǵ� �Լ�
	void LeaveWaitingRoom()
	{
		// ��� �÷��̾���� Player�гο� ���� �÷��̾� �̹��� ����
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
		// Destroy�� ������Ʈ �ε��� ã��
		GameObject targetOjbectToDestroy = playerObjects[index - 1];
		playerObjects.Remove(targetOjbectToDestroy);
		Destroy(targetOjbectToDestroy);

		// ���� �ڽ��� �ε����� �ƴ϶�� ������ �������̹Ƿ� Player�г� ������Ʈ
		UpdatePlayerPanel(index);

		if (myWaitingIndex > index)
			myWaitingIndex -= 1;
	}

	// �÷��̾� �г� ������Ʈ
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

	// ����� �ϱ� ���� �ӽ÷� ����ϴ� ������Ʈ��
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
