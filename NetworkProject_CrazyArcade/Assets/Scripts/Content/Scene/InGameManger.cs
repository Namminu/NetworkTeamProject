using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
		pv = GetComponent<PhotonView>();
	}

    // Update is called once per frame 
    void Update()
    {
        
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
		Vector3 spawnPosition = playerSpawnLocation[Random.Range(0, playerSpawnLocation.Length)].transform.position;
		PhotonNetwork.Instantiate(PlayerPrefabs[randomIndex].name, spawnPosition, Quaternion.identity, 0);

		yield return null;

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
