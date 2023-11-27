using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomItem : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // ������ ������Ʈ �迭
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
        int randomIndex = Random.Range(0, objectsToSpawn.Length); // ���� �ε��� ����
        Instantiate(objectsToSpawn[randomIndex], transform.position, Quaternion.identity); // ���� ������Ʈ ����
    }
}
