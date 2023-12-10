using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class InGameManger : MonoBehaviourPun
{
	//���� ��ȯ�� ���� �ð� ����
	[Header("�¸��� �г� ǥ�� �ð�")]
    [Tooltip("�������� ȭ�� �Ѿ�� ��� �ð�")]
	public float WaitTime;
    [Tooltip("n �� �ڿ� �������� �̵��մϴ� �ؽ�Ʈ")]
    public Text BackToWatingSecond; //�ð� ����
	private float inner_WatingTime;   //���� �ڵ忡�� �ʱ�ȭ�� ���� ����, �� ������ �ð� ������ ����

    public GameObject winnerCanvas;

	[Header("�÷��̾� ���� ��ġ �Ҵ�")]
	public GameObject player1SpawnLocation;
	public GameObject player2SpawnLocation;
	public GameObject player3SpawnLocation;
	public GameObject player4SpawnLocation;

    //�ӽÿ� ���� ���� ����
    private PhotonInit pi;

	// Start is called before the first frame update
	void Start()
    {
        pi = GetComponent<PhotonInit>();

		if (pi != null)
        {
            Debug.Log("���� ���� �Ϸ�");
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
			PhotonNetwork.LoadLevel("WaitingLevel");
		}
    }

    public void SetWatingSecondText()
    {
		BackToWatingSecond.text = (int)inner_WatingTime + " �� �ڿ� �������� �̵��մϴ�...";
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
