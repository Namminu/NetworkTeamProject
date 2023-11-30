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
        if (other.tag == "BOMB")
        {
            other.gameObject.GetComponent<BombController>().BombBombBomb();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("HitBox"))
        {
            print("1");
            other.gameObject.GetComponentInParent<PlayerController>().IsDie();
        }
    }


}

