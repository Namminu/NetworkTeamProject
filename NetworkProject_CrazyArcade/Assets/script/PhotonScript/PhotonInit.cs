using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonInit : Photon.PunBehaviour
{
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(" ~ ");
    }
}
 