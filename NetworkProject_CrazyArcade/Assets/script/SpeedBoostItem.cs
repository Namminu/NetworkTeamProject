using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    private float speedBoost = 5.0f; // �ӵ� ������

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                //player.playerstat.playerSpeed  += speedBoost; // �÷��̾��� �ӵ� ����
                //Destroy(gameObject); // ������ ��� �� ����
                //Debug.Log("����");
                //Debug.Log(player.playerstat.playerSpeed);
            }
        }
    }

}
