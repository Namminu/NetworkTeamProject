using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInit : MonoBehaviour
{
    PhotonView pv;

    public void InitLobby()
    {
        pv = GetComponent<PhotonView>();
    }

    #region ���� ���� ���� �޼ҵ�
    void CreateNewWaitingRoom()
    {

    }

    #endregion
}
