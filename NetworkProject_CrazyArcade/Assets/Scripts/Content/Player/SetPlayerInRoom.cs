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
        //�̸� ����
        //�÷��̾� ���̰� ����
        //�÷��̾� ���ö����� �� �����ʿ��� �����ǵ���
        //�÷��̾� ���������� �ϳ��� �и�����

        EnablePlayerImage();
    }

    void EnablePlayerImage()
    {
        
    }

    void DisablePlayerImage()
    {

    }
}
