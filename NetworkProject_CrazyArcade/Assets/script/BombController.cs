using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float bombTime = 3.0f;
    public int streamLength = 1;
    public GameObject BombStream;
    // Start is called before the first frame upd ate
    void Start()
    {
        Invoke("BombAction", bombTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BombAction()
    {
        Instantiate(BombStream, transform.position, Quaternion.identity);
        for (int i = 1; i <= streamLength; i++)
        {
            Instantiate(BombStream, transform.position + new Vector3(0, -i, 0), Quaternion.identity);
            Instantiate(BombStream, transform.position + new Vector3(0, i, 0), Quaternion.identity);
            Instantiate(BombStream, transform.position + new Vector3(-i, 0, 0), Quaternion.identity);
            Instantiate(BombStream, transform.position + new Vector3(i, 0, 0), Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
