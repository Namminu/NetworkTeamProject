using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class BaseItem : MonoBehaviourPun
{
    public abstract void OperateItemLogic(PlayerController player);

    PhotonView pv;

	public AudioClip soundClip;
	private AudioSource audioSource;

	private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();

		audioSource = GetComponent<AudioSource>();
		audioSource.clip = soundClip;
	}
    void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("∏‘¿Ω");
                if(player.photonView.IsMine && pv != null)
                {
                    OperateItemLogic(player);
                    Destroy(gameObject);
                    pv.RPC("ItemEat", RpcTarget.Others);
				    audioSource.Play();
                }
			}
        }
        
        if(other.gameObject.tag == "BombStream")
        {
            Destroy(gameObject);         
        }
    }

    [PunRPC]
    public void ItemEat()
    {
        Destroy(gameObject); 
    }

}
