using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WaitingRoomInit : MonoBehaviourPunCallbacks
{
	[Header("Waiting Room")]
	public GameObject[] playerImages = new GameObject[4];
	private GameObject[] playerImagePos = new GameObject[4];
	private Text[] playerText = new Text[4];
	
	private int myWaitingIndex;
	private int myCharacterIndex;

	private Room currentRoom;
	public List<Player> Players = new List<Player>();

	private void Start()
	{
		// 현재 들어와있는 방 저장
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
		Debug.Log(player.NickName + string.Format("님이 {0}하셨습니다.", b_Enter ? "입장" : "퇴장"));
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
		}
	}

	// 플레이어 패널 업데이트
	void UpdatePlayerPanel(Player targetPlayer, bool is_Enter)
	{
		int targetPlayerWaitingIndex = (int)targetPlayer.CustomProperties["waitingIndex"];
		// 만약 플레이어가 들어와서 변경된 것이라면
		if (is_Enter)
        {
			Instantiate(playerImages[(int)targetPlayer.CustomProperties["characterIndex"]],
						playerImagePos[targetPlayerWaitingIndex].transform);
			playerText[targetPlayerWaitingIndex].text = targetPlayer.NickName;
		}
		else
		{
			// 나간 플레이어를 제거함
			Destroy(playerImagePos[targetPlayerWaitingIndex].transform.GetChild(0).gameObject);
			playerText[targetPlayerWaitingIndex].text = "";

			foreach(Player player in Players)
            {
				int myPlayerIndex = (int)player.CustomProperties["waitingIndex"];
				// 오른쪽 기준의 플레이어들이 왼쪽으로 밀려남
				if (myPlayerIndex > targetPlayerWaitingIndex)
                {
					GameObject myPlayerImg = playerImagePos[myPlayerIndex].transform.GetChild(0).gameObject;
					string myPlayerText = playerText[myPlayerIndex].text;
					int newPlayerIndex = myPlayerIndex - 1;
					
					// 플레이어 이미지 위치 다시 설정
					myPlayerImg.transform.SetParent(playerImagePos[newPlayerIndex].transform);
					myPlayerImg.transform.localPosition = Vector3.zero;
					myPlayerImg.transform.localScale = Vector3.one;

					// 플레이어 텍스트 위치 설정
					playerText[myPlayerIndex].text = "";
					playerText[newPlayerIndex].text = myPlayerText;

					// 플레이어 waitingIndex프로퍼티 값 다시 설정
					Hashtable tempProperties = player.CustomProperties;
					tempProperties["waitingIndex"] = newPlayerIndex;
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

    #region 대기방 관련 메소드
    public void InitWaitingRoom()
	{
		for (int i = 0; i < playerImagePos.Length; i++)
		{
			playerText[i] = GameObject.Find("Player" + (i + 1) + "NameText").transform.GetComponent<Text>();
			playerImagePos[i] = GameObject.Find("PlayerImgPos" + (i + 1));
		}

		myWaitingIndex = currentRoom.PlayerCount - 1;

		InitPlayerProperty();
	}
	
	// 플레이어가 대기방에 입장할때 실행되는 함수
	void InitPlayerProperty()
	{
		//이미 방에 들어와 있는 플레이어들의 캐릭터를 만들어줌
		foreach (Player otherPlayer in PhotonNetwork.PlayerListOthers)
		{
			Players.Add(otherPlayer);
			Instantiate(playerImages[(int)otherPlayer.CustomProperties["characterIndex"]],
				playerImagePos[(int)otherPlayer.CustomProperties["waitingIndex"]].transform);
			playerText[(int)otherPlayer.CustomProperties["waitingIndex"]].text = otherPlayer.NickName;
		}

		Hashtable properties = new Hashtable();
		properties.Add("isMaster", false);

		// 캐릭터 랜덤으로 설정함 -> 방에 이미 들어와있는 플레이어들의 캐릭터랑 중복되지 않게함
		if (currentRoom.PlayerCount != 1)
			myCharacterIndex = SetCharacterIndex();
		else
		{
			myCharacterIndex = Random.Range(0, playerImages.Length);
			properties["isMaster"] = true;
			PhotonNetwork.AutomaticallySyncScene = true;
		}

		GameObject player = Instantiate(playerImages[myCharacterIndex], playerImagePos[myWaitingIndex].transform);

		properties.Add("waitingIndex", myWaitingIndex);
		properties.Add("characterIndex", player.GetComponent<CharacterInfo>().CharacterNumber);
		properties.Add("ready", false);
		properties.Add("InitComplete", false);

		PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

		Players.Add(PhotonNetwork.LocalPlayer);
		playerText[myWaitingIndex].text = PhotonNetwork.LocalPlayer.NickName;
		PhotonInit.Instance.toolTipText.StartTextEffect(string.Format("{0}님의 방에 입장하였습니다", PhotonNetwork.MasterClient.NickName), Effect.FADE);
	}

	// 캐릭터 중복을 막기위한 함수
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
		if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
			PhotonInit.Instance.toolTipText.StartTextEffect("방장은 레디할 수 없습니다!", Effect.FADE);

		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public void StartGame()
    {
		PhotonInit.Instance.SetPlayerForGame(Players);
		PhotonInit.Instance.GameStart();
    }
}
