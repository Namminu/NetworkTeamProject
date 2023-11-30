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

    public GameObject spawnObject;
    private MaxItemInfo itemSpawnInfo;

    private void Start()
    {
        itemSpawnInfo = GameObject.Find("ItemSpawnController").GetComponent<MaxItemInfo>();
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
        int randomIndex = Random.Range(0, objectsToSpawn.Length); // 랜덤 인덱스 생성
        if(itemCount[(int)ItemNumber.FEATHER] > itemSpawnInfo.maxFeathers)
        {
            SetRandomSpawnObject();
            return;
        }

        if (itemCount[(int)ItemNumber.BACKPACK] > itemSpawnInfo.maxBackPacks)
        {
            SetRandomSpawnObject();
            return;
        }

        if (itemCount[(int)ItemNumber.SCROLL] > itemSpawnInfo.maxScrolls)
        {
            SetRandomSpawnObject();
            return;
        }

        spawnObject = objectsToSpawn[randomIndex];
        itemCount[randomIndex]++;
    }

    void SpawnRandomObject()
    {
        Instantiate(spawnObject, transform.position, Quaternion.identity); // 랜덤 오브젝트 생성
    }
}
