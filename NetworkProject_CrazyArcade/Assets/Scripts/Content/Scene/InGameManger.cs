using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEditor;

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

    public GameObject winnerCanvas;

	[Header("플레이어 생성 위치 할당")]
	[Tooltip("플레이어 생성 위치 리스트")]
	public  GameObject[] playerSpawnLocation = new GameObject[4];

	//[Tooltip("해당 위치에 플레이어 생성 가능한지 여부 판단")]
	//public bool[] canSpawnPlayer;
	[Tooltip("플레이어 프리팹")]
	public GameObject[] PlayerPrefabs = new GameObject[4];

	private int randomIndex;

	// 게임 시작 시의 로직과 관련
	private List<Player> playerList;
	private Player me;
	private Player winner;
	public int count;

	// Start is called before the first frame update
	void Start()
    {
		pv = GetComponent<PhotonView>();
	}

    public void IsGameClear()
    {
		winnerCanvas.SetActive(true);
		winnerName.text = winner.NickName;
		StartCoroutine(BackToWaitingRoom(WaitTime));
    }

	IEnumerator BackToWaitingRoom(float time)
    {
		float waitTime = time;
		
		while(waitTime >= 0f)
        {
			SetWatingSecondText((int)waitTime);
			waitTime -= 1f;
			yield return new WaitForSeconds(1f);
		}

		SceneManager.LoadScene("WaitingLevel");
    }

    public void SetWatingSecondText(int time)
    {
		BackToWatingSecond.text = time + " 초 뒤에 대기방으로 이동합니다...";
	}

	public IEnumerator CreatePlayer(Player myInfo)
	{
		Vector3 spawnPosition = playerSpawnLocation[(int)myInfo.CustomProperties["waitingIndex"]].transform.position;
		PhotonNetwork.Instantiate(PlayerPrefabs[(int)myInfo.CustomProperties["characterIndex"]].name, 
			spawnPosition, Quaternion.identity, 0);

		yield return null;
	}

    public void GameStart(Player myInfo)
	{
		me = myInfo;
		StartCoroutine(CheckWinner());


        if (PhotonNetwork.IsMasterClient)
		{
            CreateRandomItem[] creatRand;
            creatRand = GameObject.FindObjectsOfType<CreateRandomItem>();
            for (int i = 0; i < creatRand.Length; i++)
            {
				creatRand[i].InitRandItem();
                creatRand[i].RandIteam();
            }
        }
            
    }

	IEnumerator CheckWinner()
    {
		while (true)
		{
			count = CheckPlayerAlive();
			if (count <= 1) break;
			yield return null;
		}

		if(count == 0)
        {
			CreatePlayer(me);
			StartCoroutine(CheckWinner());
        }
		else
        {
			foreach(Player player in PhotonNetwork.PlayerList)
            {
				if((bool)player.CustomProperties["isDie"] == false)
                {
					winner = player;
				}
            }
			IsGameClear();
		}
	}

	public int CheckPlayerAlive()
	{
		int aliveCount = PhotonNetwork.PlayerList.Length;
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			if ((bool)player.CustomProperties["isDie"])
			{
				aliveCount -= 1;
			}
		}
		return aliveCount;
	}

	[PunRPC]
	public void MakePlayer(int index, Vector3 pos, Quaternion rotation)
    {
		Debug.Log("생성 완료");
		Instantiate(PlayerPrefabs[index], pos, rotation);
	}
} 
