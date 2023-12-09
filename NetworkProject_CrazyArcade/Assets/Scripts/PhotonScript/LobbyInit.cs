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

    #region 措扁规 积己 包访 皋家靛
    void CreateNewWaitingRoom()
    {

    }

    #endregion
}
