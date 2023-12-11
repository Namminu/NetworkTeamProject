using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IncreaseBombPowerItem : BaseItem
{
    [SerializeField]
    private int increaseRate = 1; // �ӵ� ������

    

    public override void OperateItemLogic(PlayerController player)
    {

        if (player.photonView.IsMine)
        {
            player.playerstat.bombLength += increaseRate; // �÷��̾��� �ӵ� ����
        }
    }

    
}
