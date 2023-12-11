using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShowWhereAmI : MonoBehaviourPun
{
    public Canvas Arrow;
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            Arrow.gameObject.SetActive(true);
			Invoke("deleteImage", 2.5f);
		}        
    }
    private void deleteImage()
    {
        Destroy( Arrow );
    }
}
