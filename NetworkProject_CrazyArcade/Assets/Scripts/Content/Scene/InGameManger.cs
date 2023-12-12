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

	//���� ��ȯ�� ���� �ð� ����
	[Header("�¸��� �г� ǥ�� �ð�")]
	[Tooltip("�¸��� �̸� �ؽ�Ʈ")]
	public Text winnerName;
    [Tooltip("�������� ȭ�� �Ѿ�� ��� �ð�")]
	public float WaitTime;

    [Tooltip("n �� �ڿ� �������� �̵��մϴ� �ؽ�Ʈ")]
    public Text BackToWatingSecond; //�ð� ����
	private float inner_WatingTime;   //���� �ڵ忡�� �ʱ�ȭ�� ���� ����, �� ������ �ð� ������ ����

    public GameObject winnerCanvas;

	[Header("�÷��̾� ���� ��ġ �Ҵ�")]
	[Tooltip("�÷��̾� ���� ��ġ ����Ʈ")]
	public  GameObject[] playerSpawnLocation = new GameObject[4];

	[Tooltip("�÷��̾� ������")]
	public GameObject[] PlayerPrefabs = new GameObject[4];

	[Tooltip("�¸� ���� �Ǻ��� ���� �÷��̾� �� ����")]
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
        // ��Ʈ��ũ ��� ���� ��������
        var stats = PhotonNetwork.NetworkingClient.LoadBalancingPeer.VitalStatsToString(false);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            // ��Ʈ��ũ ��� ���� ���
            Debug.Log(stats);
		playersName = new List<string>(playerCount);

		//�𸣰ڴ�........ 
		//playerName ����Ʈ�� �÷��̾���� �̸��� �߰��ϴ°� �Ϸ� / ���Ÿ� �ΰ��ӿ��� �÷��̾ ����� �� �Ϸ��� �ϴµ� ����� �𸣰���
		if (playerCount <= 1)
		{
			GameClear(playersName);
		}

		Debug.Log("�÷��̾� �� : " + playerCount);
		Debug.Log("�÷��̾�� �̸� ����Ʈ : " + playersName.Count);
	}

	//���� Ŭ���� �� ȣ�� �Լ�
	public void GameClear(List<string> winner)
	{
		winnerCanvas.SetActive(true);
		inner_WatingTime = WaitTime;

		SetWinnerName(winner[0]);	//����� �÷��̾�� ���� �����ϰ� ���� �÷��̾�� ������ �ϳ�
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
		BackToWatingSecond.text = (int)inner_WatingTime + " �� �ڿ� �������� �̵��մϴ�...";
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
		Debug.Log("���� �Ϸ�");
		Instantiate(PlayerPrefabs[index], pos, rotation);
	}
} 
