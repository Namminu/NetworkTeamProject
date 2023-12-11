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
		// ĳ���� �������� ������ -> �濡 �̹� �����ִ� �÷��̾���� ĳ���Ͷ� �ߺ����� �ʰ���
		if (PlayerPrefabs.Length != 0)
			randomIndex = SetCharacterIndex();
		else
			randomIndex = Random.Range(0, PlayerPrefabs.Length);

		Vector3 spawnPosition = playerSpawnLocation[Random.Range(0, playerSpawnLocation.Length)].transform.position;
		PhotonNetwork.Instantiate(PlayerPrefabs[randomIndex].name, spawnPosition, Quaternion.identity, 0); 
		yield return null;

		/* // ���� �ε��� �Ҵ�
		//Vector3 spawnPosition;
		////�ٸ� �÷��̾ ������ ������ ��ġ���� Ȯ��
		//while (true)
		//{
		//	int randomIndex = Random.Range(0, playerSpawnLocation.Length);

		//	if (canSpawnPlayer[randomIndex])	//���� ������ �ε��� ���̶��
		//	{
		//		canSpawnPlayer[randomIndex] = false;
		//		// �ش� �ε����� ��ġ�� �Ҵ�
		//		spawnPosition = playerSpawnLocation[randomIndex].transform.position;
		//		break;
		//	}
		//	else //������ �� ���� �ε��� ���̶��
		//	{
		//	}
		//}
		//// �Ҵ�� ��ġ������ �÷��̾� ����
		//PhotonNetwork.Instantiate("playerBlue", spawnPosition, Quaternion.identity, 0);
		//yield return null; */

	}

	// ĳ���� �ߺ��� �������� �Լ�
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
