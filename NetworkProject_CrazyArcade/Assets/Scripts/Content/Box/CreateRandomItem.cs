using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

enum ItemNumber
{
    FEATHER = 0,
    BACKPACK,
    SCROLL
}

public class CreateRandomItem : MonoBehaviourPun
{
    public static int[] itemCount = new int[3];

    public GameObject[] objectsToSpawn; // ������ ������Ʈ �迭
    private bool createItem = false;

    private GameObject spawnObject = null;
    private SpawnController itemSpawnInfo;

    [Header("������ ����")]
    [Tooltip ("üũ �� ������ �ڽ����� ������ �������� �������� �������ݴϴ�")]
    [SerializeField] bool isAlwaysSpawnObject = false;

    private void Start()
    {
        RandIteam();
        
    }

    void RandIteam()
    {
        Debug.Log(PhotonNetwork.IsMasterClient.ToString());
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(spawnObject);
            itemSpawnInfo = GameObject.Find("ItemSpawnController").GetComponent<SpawnController>();
            Debug.Log((itemSpawnInfo.spawnRate * 10f));
            if (Random.Range(1f, 10f) <= (itemSpawnInfo.spawnRate * 10f) || isAlwaysSpawnObject)
                SetRandomSpawnObject();
                
            photonView.RPC("OthersSpawnObject", RpcTarget.Others, spawnObject);
        }
        
    }

    [PunRPC]
    void OthersSpawnObject(GameObject newSpawnObject)
    {
        spawnObject = newSpawnObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BombStream" && !createItem)
        {
            createItem = true;
            Destroy(gameObject);
        }
    }

    void SetRandomSpawnObject()
    {
        if(itemCount[(int)ItemNumber.FEATHER] == itemSpawnInfo.maxFeathers && 
            itemCount[(int)ItemNumber.BACKPACK] == itemSpawnInfo.maxBackPacks && 
            itemCount[(int)ItemNumber.SCROLL] == itemSpawnInfo.maxScrolls)
        {
            return;
        }

        int randomItemIndex = Random.Range(0, objectsToSpawn.Length); // ���� �ε��� ����
        itemCount[randomItemIndex]++;
        if (itemCount[(int)ItemNumber.FEATHER] > itemSpawnInfo.maxFeathers)
        {
            itemCount[(int)ItemNumber.FEATHER]--;
            SetRandomSpawnObject();
            return;
        }

        if (itemCount[(int)ItemNumber.BACKPACK] > itemSpawnInfo.maxBackPacks)
        {
            itemCount[(int)ItemNumber.BACKPACK]--;
            SetRandomSpawnObject();
            return;
        }

        if (itemCount[(int)ItemNumber.SCROLL] > itemSpawnInfo.maxScrolls)
        {
            itemCount[(int)ItemNumber.SCROLL]--;
            SetRandomSpawnObject();
            return;
        }

        spawnObject = objectsToSpawn[randomItemIndex];
        
    }

    public void SpawnRandomObject()
    {
        if (spawnObject)
        {
            
            PhotonNetwork.Instantiate(spawnObject.name, transform.position, Quaternion.identity); // ���� ������Ʈ ����
            createItem = true;
        }
        Destroy(gameObject);
    }
}
