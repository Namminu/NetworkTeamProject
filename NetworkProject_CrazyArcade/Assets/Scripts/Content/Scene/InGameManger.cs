using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEditor;

public class InGameManger : MonoBehaviourPun
{
	//대기방 전환을 위한 시간 변수
	[Header("승리자 패널 표시 시간")]
    [Tooltip("대기방으로 화면 넘어가는 대기 시간")]
	public float WaitTime;
    [Tooltip("n 초 뒤에 대기방으로 이동합니다 텍스트")]
    public Text BackToWatingSecond; //시간 문구
	private float inner_WatingTime;   //내부 코드에서 초기화를 위한 변수, 이 변수로 시간 설정할 예정

    public GameObject winnerCanvas;

	[Header("플레이어 생성 위치 할당")]
	[Tooltip("플레이어 생성 위치 리스트")]
	public  GameObject[] playerSpawnLocation = new GameObject[4];
	//[Tooltip("해당 위치에 플레이어 생성 가능한지 여부 판단")]
	//public bool[] canSpawnPlayer;
	[Tooltip("플레이어 프리팹")]
	public GameObject[] PlayerPrefabs = new GameObject[4];

	private int randomIndex;

	

	// Start is called before the first frame update
	void Start()
    {
		 
	}

    // Update is called once per frame 
    void Update()
    {
        /*if(true)
        {
            GameClear();
        }
		inner_WatingTime -= Time.deltaTime; */
	}

    //게임 클리어 시 호출 함수
    public void GameClear() 
    {
		winnerCanvas.SetActive(true);
		inner_WatingTime = WaitTime;

        BackToWaitingRoom();
        SetWatingSecondText();
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

	public IEnumerator temp_CreatePlayer()
	{
		// 캐릭터 랜덤으로 설정함 -> 방에 이미 들어와있는 플레이어들의 캐릭터랑 중복되지 않게함
		if (PlayerPrefabs.Length != 0)
			randomIndex = SetCharacterIndex();
		else
			randomIndex = Random.Range(0, PlayerPrefabs.Length);

		Vector3 spawnPosition = playerSpawnLocation[Random.Range(0, playerSpawnLocation.Length)].transform.position;
		PhotonNetwork.Instantiate(PlayerPrefabs[randomIndex].name, spawnPosition, Quaternion.identity, 0); 
		yield return null;

		/* // 랜덤 인덱스 할당
		//Vector3 spawnPosition;
		////다른 플레이어가 이전에 생성된 위치인지 확인
		//while (true)
		//{
		//	int randomIndex = Random.Range(0, playerSpawnLocation.Length);

		//	if (canSpawnPlayer[randomIndex])	//생성 가능한 인덱스 값이라면
		//	{
		//		canSpawnPlayer[randomIndex] = false;
		//		// 해당 인덱스의 위치값 할당
		//		spawnPosition = playerSpawnLocation[randomIndex].transform.position;
		//		break;
		//	}
		//	else //생성할 수 없는 인덱스 값이라면
		//	{
		//	}
		//}
		//// 할당된 위치값으로 플레이어 생성
		//PhotonNetwork.Instantiate("playerBlue", spawnPosition, Quaternion.identity, 0);
		//yield return null; */

	}

	// 캐릭터 중복을 막기위한 함수
	int SetCharacterIndex()
	{
		int randNum;
		List<int> exclusionNum = new List<int>();

		for (int i = 0; i < PlayerPrefabs.Length; i++)
			exclusionNum.Add(PlayerPrefabs[i].GetComponent<CharacterInfo>().CharacterNumber);

		//do
		//{
		//	randNum = Random.Range(0, PlayerPrefabs.Length);
		//} while (exclusionNum.Contains(randNum));
		randNum = Random.Range(0, 3);
		return randNum;
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
} 
