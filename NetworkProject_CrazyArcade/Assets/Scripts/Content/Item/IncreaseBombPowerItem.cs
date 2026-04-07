using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IncreaseBombPowerItem : BaseItem
{
    [SerializeField]
    private int increaseRate = 1; // 속도 증가량

    

    public override void OperateItemLogic(PlayerController player)
    {

        if (player.photonView.IsMine)
        {
            player.playerstat.bombLength += increaseRate; // 플레이어의 속도 증가
        }
    }

    
}
