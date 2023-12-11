using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WaitingRoomInit : MonoBehaviourPunCallbacks
{
	[Header("Waiting Room")]
	public GameObject[] playerImages = new GameObject[4];
	private GameObject[] playerImagePos = new GameObject[4];
	private Text[] playerText = new Text[4];
	public GameObject[] readyText = new GameObject[4];
	
	private int myWaitingIndex;
	private int myCharacterIndex;

	private Room currentRoom;
	public List<Player> Players = new List<Player>();

	private void Start()
	{
		// ���� �����ִ� �� ����
		currentRoom = PhotonNetwork.CurrentRoom;

		InitWaitingRoom();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdatePlayerList(newPlayer, true);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdatePlayerList(otherPlayer, false);
	}

	void UpdatePlayerList(Player player, bool b_Enter)
	{
		Debug.Log(player.NickName + string.Format("���� {0}�ϼ̽��ϴ�.", b_Enter ? "����" : "����"));
		if (b_Enter)
		{
			if (!Players.Contains(player))
			{
				Players.Add(player);
			}
		}
		else
		{
			if (Players.IndexOf(player) != -1)
			{
				Players.RemoveAt(Players.IndexOf(player));
				UpdatePlayerPanel(player, false);

				Hashtable properties = new Hashtable();
				properties.Add("isMaster", false);
				properties.Add("waitingIndex", -1);
				properties.Add("characterIndex", -1);
				properties.Add("ready", false);
				properties.Add("InitComplete", false);
				player.SetCustomProperties(properties);
			}
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (Players.Contains(targetPlayer))
		{
			if ((bool)changedProps["InitComplete"] == false)
			{
				changedProps["InitComplete"] = true;
				if (targetPlayer != PhotonNetwork.LocalPlayer)
				{
					UpdatePlayerPanel(targetPlayer, true);
				}
			}
			Players[Players.IndexOf(targetPlayer)] = targetPlayer;

			// ���� �ؽ�Ʈ ������Ʈ
			for(int i = 1; i < readyText.Length; i++)
            {
				if(i <= Players.Count - 1)
                {
					if ((bool)Players[i].CustomProperties["ready"]) readyText[i].SetActive(true);
					else readyText[i].SetActive(false);
				}
				else
                {
					readyText[i].SetActive(false);
				}
            }
		}
	}

	// �÷��̾� �г� ������Ʈ
	void UpdatePlayerPanel(Player targetPlayer, bool is_Enter)
	{
		int targetPlayerWaitingIndex = (int)targetPlayer.CustomProperties["waitingIndex"];
		// ���� �÷��̾ ���ͼ� ����� ���̶��
		if (is_Enter)
        {
			Instantiate(playerImages[(int)targetPlayer.CustomProperties["characterIndex"]],
						playerImagePos[targetPlayerWaitingIndex].transform);
			playerText[targetPlayerWaitingIndex].text = targetPlayer.NickName;
		}
		else
		{
			// ���� �÷��̾ ������
			Destroy(playerImagePos[targetPlayerWaitingIndex].transform.GetChild(0).gameObject);
			playerText[targetPlayerWaitingIndex].text = "";

			foreach(Player player in Players)
            {
				int myPlayerIndex = (int)player.CustomProperties["waitingIndex"];
				// ������ ������ �÷��̾���� �������� �з���
				if (myPlayerIndex > targetPlayerWaitingIndex)
                {
					GameObject myPlayerImg = playerImagePos[myPlayerIndex].transform.GetChild(0).gameObject;
					string myPlayerText = playerText[myPlayerIndex].text;
					int newPlayerIndex = myPlayerIndex - 1;
					
					// �÷��̾� �̹��� ��ġ �ٽ� ����
					myPlayerImg.transform.SetParent(playerImagePos[newPlayerIndex].transform);
					myPlayerImg.transform.localPosition = Vector3.zero;
					myPlayerImg.transform.localScale = Vector3.one;

					// �÷��̾� �ؽ�Ʈ ��ġ ����
					playerText[myPlayerIndex].text = "";
					playerText[newPlayerIndex].text = myPlayerText;

					// �÷��̾� waitingIndex������Ƽ �� �ٽ� ����
					Hashtable tempProperties = player.CustomProperties;
					tempProperties["waitingIndex"] = newPlayerIndex;

					// ���� ���� �ڸ���� �������� ����
					if (newPlayerIndex == targetPlayerWaitingIndex)
					{
						tempProperties["isMaster"] = true;
						tempProperties["ready"] = true;
					}

					player.SetCustomProperties(tempProperties);
				}		
            }
		}
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
			for (int i = 0; i < Players.Count; i++)
			{
				Debug.Log("Player[" + i + "]");
				Debug.Log("IsMaster: " + Players[i].CustomProperties["isMaster"]);
				Debug.Log("waitingIndex: " + Players[i].CustomProperties["waitingIndex"]);
				Debug.Log("characterIndex: " + Players[i].CustomProperties["characterIndex"]);
				Debug.Log("ready: " + Players[i].CustomProperties["ready"]);
			}
		}
    }

    #region ���� ���� �޼ҵ�
    public void InitWaitingRoom()
	{
		for (int i = 0; i < playerImagePos.Length; i++)
		{
			playerText[i] = GameObject.Find("Player" + (i + 1) + "NameText").transform.GetComponent<Text>();
			playerImagePos[i] = GameObject.Find("PlayerImgPos" + (i + 1));
		}

		myWaitingIndex = currentRoom.PlayerCount - 1;

		StartCoroutine(InitPlayerProperty());
	}
	
	// �÷��̾ ���濡 �����Ҷ� ����Ǵ� �Լ�
	IEnumerator InitPlayerProperty()
	{
		
		while(PhotonNetwork.NetworkClientState == ClientState.Joining)
        {
			Debug.Log("���� ��");
			yield return null;
        }

		//�̹� �濡 ���� �ִ� �÷��̾���� ĳ���͸� �������
		foreach (Player otherPlayer in PhotonNetwork.PlayerListOthers)
		{
			Players.Add(otherPlayer);
			Instantiate(playerImages[(int)otherPlayer.CustomProperties["characterIndex"]],
				playerImagePos[(int)otherPlayer.CustomProperties["waitingIndex"]].transform);
			playerText[(int)otherPlayer.CustomProperties["waitingIndex"]].text = otherPlayer.NickName;
		}

		Hashtable properties = new Hashtable();
		properties.Add("isMaster", false);
		properties.Add("ready", false);

		// ĳ���� �������� ������ -> �濡 �̹� �����ִ� �÷��̾���� ĳ���Ͷ� �ߺ����� �ʰ���
		if (currentRoom.PlayerCount >= 1)
			myCharacterIndex = SetCharacterIndex();
		else
		{
			myCharacterIndex = Random.Range(0, playerImages.Length);
			properties["isMaster"] = true;
			properties["ready"] = true;
			PhotonNetwork.AutomaticallySyncScene = true;
		}

		GameObject player = Instantiate(playerImages[myCharacterIndex], playerImagePos[myWaitingIndex].transform);

		properties.Add("waitingIndex", myWaitingIndex);
		properties.Add("characterIndex", player.GetComponent<CharacterInfo>().CharacterNumber);
		properties.Add("InitComplete", false);

		PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

		Players.Add(PhotonNetwork.LocalPlayer);
		playerText[myWaitingIndex].text = PhotonNetwork.LocalPlayer.NickName;
		PhotonInit.Instance.toolTipText.StartTextEffect(string.Format("{0}���� �濡 �����Ͽ����ϴ�", PhotonNetwork.MasterClient.NickName), Effect.FADE);
	}

	// ĳ���� �ߺ��� �������� �Լ�
	int SetCharacterIndex()
	{
		int randNum;
		List<int> exclusionNum = new List<int>();

		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			if (PhotonNetwork.PlayerList[i] != PhotonNetwork.LocalPlayer)
			{
				if (Players[i].CustomProperties["characterIndex"] != null)
					exclusionNum.Add((int)Players[i].CustomProperties["characterIndex"]);
			}
		}

		do
		{
			randNum = Random.Range(0, playerImages.Length);
		} while (exclusionNum.Contains(randNum));

		return randNum;
	}
    #endregion

	public void Ready()
	{
		if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["isMaster"] == true)
		{
			PhotonInit.Instance.toolTipText.StartTextEffect("������ ���� Ǯ �� �����ϴ�!", Effect.FADE);
			return;
		}

		Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
		properties["ready"] = (bool)properties["ready"] ? false : true;
		PhotonNetwork.AutomaticallySyncScene = (bool)properties["ready"];

		PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

		if ((bool)properties["ready"])
			PhotonInit.Instance.SetPlayerForGame(PhotonNetwork.LocalPlayer);
		else
			PhotonInit.Instance.ResetPlayerInfo();
	}

	public void BackToLobby()
    {
		if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["isMaster"] == false)
		{
			if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["ready"] == true)
			{
				PhotonInit.Instance.toolTipText.StartTextEffect("���� �������� ���� ���� Ǯ���ּ���!", Effect.FADE);
				return;
			}
		}

		PhotonNetwork.AutomaticallySyncScene = false;
		PhotonNetwork.LeaveRoom();
		StartCoroutine(TryLeaveRoom());
    }

	IEnumerator TryLeaveRoom()
    {
		PhotonInit.Instance.toolTipText.StartTextEffect("���� ��������", Effect.WAIT);
		while (PhotonNetwork.InRoom == true)
        {
			yield return null;
        }

		PhotonInit.Instance.toolTipText.SetInvisible();
	}
	
    public void StartGame()
    {
		if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
		{
			foreach(Player player in Players)
            {
				if((bool)player.CustomProperties["ready"] == false)
                {
					PhotonInit.Instance.toolTipText.StartTextEffect("��� �غ� �ؾ� ������ �� �ֽ��ϴ�!", Effect.FADE);
					return;
				}
            }
			PhotonInit.Instance.SetPlayerForGame(PhotonNetwork.LocalPlayer);
			PhotonNetwork.LoadLevel("Level1");
		}
		else
			PhotonInit.Instance.toolTipText.StartTextEffect("���常 ������ ������ �� �ֽ��ϴ�!", Effect.FADE);
	}
}
