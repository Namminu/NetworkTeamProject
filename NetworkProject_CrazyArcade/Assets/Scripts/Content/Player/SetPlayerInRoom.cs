using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class SetPlayerInRoom : Photon.MonoBehaviour
{
    [Header("Waiting Room")]
    public RawImage[] playerImages = new RawImage[4];
    private List<GameObject> playerObjects;

    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        //이름 설정
        //플레이어 보이게 설정
        //플레이어 들어올때마다 맨 오른쪽에서 생성되도록
        //플레이어 나갈때마다 하나씩 밀리도록

        EnablePlayerImage();
    }

    void EnablePlayerImage()
    {
        
    }

    void DisablePlayerImage()
    {

    }
}
