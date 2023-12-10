using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class InGameManger : MonoBehaviourPun
{
    //���� ��ȯ�� ���� �ð� ����
    public float WaitTime;
	private float inner_WatingTime;   //���� �ڵ忡�� �ʱ�ȭ�� ���� ����, �� ������ �ð� ������ ����
    public Text BackToWatingSecond; //�ð� ����

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

    //���� Ŭ���� �� ȣ�� �Լ�
    public void GameClear() 
    {
		winnerCanvas.SetActive(true);
		inner_WatingTime = WaitTime;

		BackToWatingSecond.text = (int)inner_WatingTime + " �� �ڿ� �������� �̵��մϴ�...";
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
		BackToWatingSecond.text = (int)inner_WatingTime + " �� �ڿ� �������� �̵��մϴ�...";
	}

    //public void temp_CreatePlayer()
    //{
    //    PhotonNetwork.Instantiate();
    //}
} 
