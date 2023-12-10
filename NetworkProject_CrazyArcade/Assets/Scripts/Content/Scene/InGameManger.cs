using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class InGameManger : MonoBehaviourPun
{
    //대기방 전환을 위한 시간 변수
    public float WaitTime;
	private float inner_WatingTime;   //내부 코드에서 초기화를 위한 변수, 이 변수로 시간 설정할 예정
    public Text BackToWatingSecond; //시간 문구

    public GameObject winnerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        //GameClear();
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
			SceneManager.LoadScene("WaitingLevel");
		}
    }

    public void SetWatingSecondText()
    {
		BackToWatingSecond.text = (int)inner_WatingTime + " 초 뒤에 대기방으로 이동합니다...";
	}

    //public void temp_CreatePlayer()
    //{
    //    PhotonNetwork.Instantiate();
    //}
} 
