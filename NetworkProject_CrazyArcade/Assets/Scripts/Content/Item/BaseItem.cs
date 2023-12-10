using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public abstract void OperateItemLogic(PlayerController player);

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("∏‘¿Ω");
                OperateItemLogic(player);
                Destroy(gameObject);
            }
        }

        if(other.gameObject.tag == "BombStream")
        {
            Destroy(gameObject);         
        }
    }
}
