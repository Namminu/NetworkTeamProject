using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using UnityEditor;
using System.Text;

public class InGameManger : MonoBehaviourPun
{
	PhotonView pv;

	//대기방 전환을 위한 시간 변수
	[Header("승리자 패널 표시 시간")]
	[Tooltip("승리자 이름 텍스트")]
	public Text winnerName;
    [Tooltip("대기방으로 화면 넘어가는 대기 시간")]
	public float WaitTime;

    [Tooltip("n 초 뒤에 대기방으로 이동합니다 텍스트")]
    public Text BackToWatingSecond; //시간 문구
	private float inner_WatingTime;   //내부 코드에서 초기화를 위한 변수, 이 변수로 시간 설정할 예정

    public GameObject winnerCanvas;

	[Header("플레이어 생성 위치 할당")]
	[Tooltip("플레이어 생성 위치 리스트")]
	public  GameObject[] playerSpawnLocation = new GameObject[4];

	[Tooltip("플레이어 프리팹")]
	public GameObject[] PlayerPrefabs = new GameObject[4];

	[Tooltip("승리 조건 판별을 위한 플레이어 수 변수")]
	public int playerCount = 0;
	public List<string> playersName;
	// Start is called before the first frame update
	void Start()
    {
		pv = GetComponent<PhotonView>();
        PhotonNetwork.NetworkStatisticsEnabled = true;
    }

	// Update is called once per frame 
	void Update()
    {
        // 네트워크 통계 정보 가져오기
        var stats = PhotonNetwork.NetworkingClient.LoadBalancingPeer.VitalStatsToString(false);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            // 네트워크 통계 정보 출력
            Debug.Log(stats);
		playersName = new List<string>(playerCount);

		//모르겠다........ 
		//playerName 리스트에 플레이어들의 이름을 추가하는건 완료 / 제거를 인게임에서 플레이어가 사망할 때 하려고 하는데 방법을 모르겠음
		if (playerCount <= 1)
		{
			GameClear(playersName);
		}

		Debug.Log("플레이어 수 : " + playerCount);
		Debug.Log("플레이어들 이름 리스트 : " + playersName.Count);
	}

	//게임 클리어 시 호출 함수
	public void GameClear(List<string> winner)
	{
		winnerCanvas.SetActive(true);
		inner_WatingTime = WaitTime;

		SetWinnerName(winner[0]);	//사망한 플레이어는 전부 제거하고 남은 플레이어는 무조건 하나
		BackToWaitingRoom();
		SetWatingSecondText();
	}

	public void SetWinnerName(string IamWinner)
	{
		winnerName.text = IamWinner.ToString();
	}

	public void BackToWaitingRoom()
    {
        if(inner_WatingTime <= 0.0f)
        {
			PhotonNetwork.LoadLevel("WaitingLevel");
		}
    }

    public void SetWatingSecondText()
    {
		BackToWatingSecond.text = (int)inner_WatingTime + " 초 뒤에 대기방으로 이동합니다...";
	}

	public IEnumerator temp_CreatePlayer(Player myInfo, int players)
	{
		Vector3 spawnPosition = playerSpawnLocation[(int)myInfo.CustomProperties["waitingIndex"]].transform.position;
		PhotonNetwork.Instantiate(PlayerPrefabs[(int)myInfo.CustomProperties["characterIndex"]].name, 
			spawnPosition, Quaternion.identity, 0);

		yield return null;
		playerCount = players;
	}

    public void GameStart()
	{
        CreateRandomItem[] creatRand;
        creatRand = GameObject.FindObjectsOfType<CreateRandomItem>();
		for(int i = 0; i < creatRand.Length;i++)
		{
			creatRand[i].RandIteam();
        }
    }

    [PunRPC]
	public void MakePlayer(int index, Vector3 pos, Quaternion rotation)
    {
		Debug.Log("생성 완료");
		Instantiate(PlayerPrefabs[index], pos, rotation);
	}
} 
