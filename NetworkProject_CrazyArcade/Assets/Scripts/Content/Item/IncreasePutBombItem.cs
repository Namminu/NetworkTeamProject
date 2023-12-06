using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePutBombItem : BaseItem
{
    [SerializeField]
    private int increaseRate = 1; // �ӵ� ������

    public override void OperateItemLogic(PlayerController player)
    {
        player.playerstat.numberOfBombs += increaseRate; // �÷��̾��� �ӵ� ����
    }
}
