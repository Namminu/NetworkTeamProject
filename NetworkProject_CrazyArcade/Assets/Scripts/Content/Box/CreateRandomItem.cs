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

    public GameObject[] objectsToSpawn; // 생성할 오브젝트 배열
    private bool createItem = false;

    private GameObject spawnObject = null;
    private SpawnController itemSpawnInfo;

    [Header("개발자 전용")]
    [Tooltip ("체크 시 아이템 박스에서 무조건 아이템이 나오도록 보장해줍니다")]
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

        int randomItemIndex = Random.Range(0, objectsToSpawn.Length); // 랜덤 인덱스 생성
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
            
            PhotonNetwork.Instantiate(spawnObject.name, transform.position, Quaternion.identity); // 랜덤 오브젝트 생성
            createItem = true;
        }
        Destroy(gameObject);
    }
}
