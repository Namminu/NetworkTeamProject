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

	//���� ��ȯ�� ���� �ð� ����
	[Header("�¸��� �г� ǥ�� �ð�")]
    [Tooltip("�������� ȭ�� �Ѿ�� ��� �ð�")]
	public float WaitTime;
    [Tooltip("n �� �ڿ� �������� �̵��մϴ� �ؽ�Ʈ")]
    public Text BackToWatingSecond; //�ð� ����
	private float inner_WatingTime;   //���� �ڵ忡�� �ʱ�ȭ�� ���� ����, �� ������ �ð� ������ ����

    public GameObject winnerCanvas;

	[Header("�÷��̾� ���� ��ġ �Ҵ�")]
	[Tooltip("�÷��̾� ���� ��ġ ����Ʈ")]
	public  GameObject[] playerSpawnLocation = new GameObject[4];
	//[Tooltip("�ش� ��ġ�� �÷��̾� ���� �������� ���� �Ǵ�")]
	//public bool[] canSpawnPlayer;
	[Tooltip("�÷��̾� ������")]
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

    //���� Ŭ���� �� ȣ�� �Լ�
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
		BackToWatingSecond.text = (int)inner_WatingTime + " �� �ڿ� �������� �̵��մϴ�...";
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
		Debug.Log("���� �Ϸ�");
		Instantiate(PlayerPrefabs[index], pos, rotation);
	}
} 
