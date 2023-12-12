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

	//���� ��ȯ�� ���� �ð� ����
	[Header("�¸��� �г� ǥ�� �ð�")]
	[Tooltip("�¸��� �̸� �ؽ�Ʈ")]
	public Text winnerName;
    [Tooltip("�������� ȭ�� �Ѿ�� ��� �ð�")]
	public float WaitTime;

    [Tooltip("n �� �ڿ� �������� �̵��մϴ� �ؽ�Ʈ")]
    public Text BackToWatingSecond; //�ð� ����

    public GameObject winnerCanvas;

	[Header("�÷��̾� ���� ��ġ �Ҵ�")]
	[Tooltip("�÷��̾� ���� ��ġ ����Ʈ")]
	public  GameObject[] playerSpawnLocation = new GameObject[4];

	//[Tooltip("�ش� ��ġ�� �÷��̾� ���� �������� ���� �Ǵ�")]
	//public bool[] canSpawnPlayer;
	[Tooltip("�÷��̾� ������")]
	public GameObject[] PlayerPrefabs = new GameObject[4];

	private int randomIndex;

	// ���� ���� ���� ������ ����
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
		BackToWatingSecond.text = time + " �� �ڿ� �������� �̵��մϴ�...";
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
		Debug.Log("���� �Ϸ�");
		Instantiate(PlayerPrefabs[index], pos, rotation);
	}
} 
