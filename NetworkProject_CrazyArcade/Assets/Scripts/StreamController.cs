using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("���ƾ�1");
        if (other.tag == "BOMB")
        {
            Debug.Log("���ƾ�2");
            other.gameObject.GetComponent<BombController>().BombBombBomb();
        }
    }


}

