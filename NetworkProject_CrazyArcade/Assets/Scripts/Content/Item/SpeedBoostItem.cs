using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : BaseItem
{
    [SerializeField]
    private float speedBoost = 0.5f; // 속도 증가량

    public override void OperateItemLogic(PlayerController player)
    {
        player.playerstat.playerSpeed += speedBoost; // 플레이어의 속도 증가
    }
}
