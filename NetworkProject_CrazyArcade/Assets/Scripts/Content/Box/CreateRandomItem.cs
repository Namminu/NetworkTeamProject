using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomItem : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // 생성할 오브젝트 배열
    private bool createItem = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BombStream" && !createItem)
        {
            createItem = true;
            Destroy(gameObject);
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        int randomIndex = Random.Range(0, objectsToSpawn.Length); // 랜덤 인덱스 생성
        Instantiate(objectsToSpawn[randomIndex], transform.position, Quaternion.identity); // 랜덤 오브젝트 생성
    }
}
