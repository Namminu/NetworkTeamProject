using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCtrl : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.CompareTag("BOMBSTREAM"))
		{
			Debug.Log("��ź �¾Ҿ��");
			Destroy(gameObject, 1f);
		}
	}
}
