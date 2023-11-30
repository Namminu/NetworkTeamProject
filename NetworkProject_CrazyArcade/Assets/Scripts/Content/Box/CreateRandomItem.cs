using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ItemNumber
{
    FEATHER = 0,
    BACKPACK,
    SCROLL
}

public class CreateRandomItem : MonoBehaviour
{
    public static int[] itemCount = new int[3];

    public GameObject[] objectsToSpawn; // 생성할 오브젝트 배열
    private bool createItem = false;

    public GameObject spawnObject = null;
    private MaxItemInfo itemSpawnInfo;

    private void Start()
    {
        itemSpawnInfo = GameObject.Find("ItemSpawnController").GetComponent<MaxItemInfo>();
        if(Random.Range(1, 10) > 6)
            SetRandomSpawnObject();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BombStream" && !createItem)
        {
            createItem = true;
            SpawnRandomObject();
            Destroy(gameObject);
        }
    }

    void SetRandomSpawnObject()
    {
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

    void SpawnRandomObject()
    {
        if(spawnObject)
            Instantiate(spawnObject, transform.position, Quaternion.identity); // 랜덤 오브젝트 생성
    }
}
