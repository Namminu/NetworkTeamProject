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
	public List<string> playersName = new List<string>();

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
				playersName.Add(player.NickName);
			}
		}
		else
		{
			if (Players.IndexOf(player) != -1)
			{
				Players.RemoveAt(Players.IndexOf(player));
				playersName.RemoveAt(player.NickName.IndexOf(player.NickName));
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

			// 레디 텍스트 업데이트
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

					// 만약 방장 자리라면 방장으로 설정
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

    #region 대기방 관련 메소드
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
	
	// 플레이어가 대기방에 입장할때 실행되는 함수
	IEnumerator InitPlayerProperty()
	{
		
		while(PhotonNetwork.NetworkClientState == ClientState.Joining)
        {
			Debug.Log("조인 중");
			yield return null;
        }

		//이미 방에 들어와 있는 플레이어들의 캐릭터를 만들어줌
		foreach (Player otherPlayer in PhotonNetwork.PlayerListOthers)
		{
			Players.Add(otherPlayer);
			playersName.Add(otherPlayer.NickName);
			Instantiate(playerImages[(int)otherPlayer.CustomProperties["characterIndex"]],
				playerImagePos[(int)otherPlayer.CustomProperties["waitingIndex"]].transform);
			playerText[(int)otherPlayer.CustomProperties["waitingIndex"]].text = otherPlayer.NickName;
		}

		Hashtable properties = new Hashtable();
		properties.Add("isMaster", false);
		properties.Add("ready", false);

		// 캐릭터 랜덤으로 설정함 -> 방에 이미 들어와있는 플레이어들의 캐릭터랑 중복되지 않게함
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
		if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["isMaster"] == true)
		{
			PhotonInit.Instance.toolTipText.StartTextEffect("방장은 레디를 풀 수 없습니다!", Effect.FADE);
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
				PhotonInit.Instance.toolTipText.StartTextEffect("방을 나가려면 레디를 먼저 풀어주세요!", Effect.FADE);
				return;
			}
		}

		PhotonNetwork.AutomaticallySyncScene = false;
		PhotonNetwork.LeaveRoom();
		StartCoroutine(TryLeaveRoom());
    }

	IEnumerator TryLeaveRoom()
    {
		PhotonInit.Instance.toolTipText.StartTextEffect("방을 나가는중", Effect.WAIT);
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
					PhotonInit.Instance.toolTipText.StartTextEffect("모두 준비를 해야 시작할 수 있습니다!", Effect.FADE);
					return;
				}
            }
			PhotonInit.Instance.SetPlayerForGame(PhotonNetwork.LocalPlayer);
			PhotonNetwork.LoadLevel("Level1");	// 이후 Level2, Level3 까지 완성되면 아래 2줄로 수정
			//랜덤씬 불러오기
			//int sceneIndex = Random.Range(1,3);
			//PhotonNetwork.LoadLevel("Level" + sceneIndex);
		}
		else
			PhotonInit.Instance.toolTipText.StartTextEffect("방장만 게임을 시작할 수 있습니다!", Effect.FADE);
	}
}
