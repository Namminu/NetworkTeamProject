using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

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
	public GameObject player1SpawnLocation;
	public GameObject player2SpawnLocation;
	public GameObject player3SpawnLocation;
	public GameObject player4SpawnLocation;

    //임시용 서버 접속 위함
    private PhotonInit pi;

	// Start is called before the first frame update
	void Start()
    {
        pi = GetComponent<PhotonInit>();

		if (pi != null)
        {
            Debug.Log("서버 접속 완료");
			temp_JoinServer();
		}

		StartCoroutine(temp_CreatePlayer());
	}

    // Update is called once per frame 
    void Update()
    {
		inner_WatingTime -= Time.deltaTime;

        //BackToWaitingRoom();
        //SetWatingSecondText();

	}

    //게임 클리어 시 호출 함수
    public void GameClear() 
    {
		winnerCanvas.SetActive(true);
		inner_WatingTime = WaitTime;

		BackToWatingSecond.text = (int)inner_WatingTime + " 초 뒤에 대기방으로 이동합니다...";
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

    IEnumerator temp_CreatePlayer() 
    {
        PhotonNetwork.Instantiate("playerBlue", player1SpawnLocation.transform.position, Quaternion.identity, 0);
        yield return null;    
    }

    public void temp_JoinServer()
    {
        pi.ConnectToServer();
	}
} 
