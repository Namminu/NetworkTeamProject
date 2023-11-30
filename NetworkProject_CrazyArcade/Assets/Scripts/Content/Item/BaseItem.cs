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
                Debug.Log("먹음");
                OperateItemLogic(player);
                Destroy(gameObject);
            }
        }

        if(other.gameObject.tag == "BombStream")
        {
            Destroy(gameObject);          // 폭발 물줄기에 닿으면 ItemBox가 사라지는 것을 구현한 후에 주석 풀어야 됨
        }
    }
}
