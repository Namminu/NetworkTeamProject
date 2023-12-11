using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.4f);
        Invoke("ColliderOff", 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BOMB")
        {
            other.gameObject.GetComponentInParent<BombController>().BombBombBomb();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("HitBox"))
        {
            other.gameObject.GetComponentInParent<PlayerController>().IsDie();
        }
    }

    private void ColliderOff()
    {
        GetComponent<BoxCollider2D>().enabled = false; 
    } 


}

