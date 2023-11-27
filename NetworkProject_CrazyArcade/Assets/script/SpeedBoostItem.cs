using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    private float speedBoost = 5.0f; // 속도 증가량

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                //player.playerstat.playerSpeed  += speedBoost; // 플레이어의 속도 증가
                //Destroy(gameObject); // 아이템 사용 후 제거
                //Debug.Log("먹음");
                //Debug.Log(player.playerstat.playerSpeed);
            }
        }
    }

}
