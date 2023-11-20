using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float bombTime = 3.0f;
    public int streamLength = 1;
    public GameObject BombStream;
    private Animator animator;
    private bool isBomb = false;
    // Start is called before the first frame upd ate
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("BombAction", bombTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BombAction()
    {
        if (isBomb)
            return;


        //Instantiate(BombStream, transform.position, Quaternion.identity);
        for (int i = 1; i <= streamLength; i++)
        {
            Instantiate(BombStream, transform.position + new Vector3(0, -i, 0), Quaternion.Euler(new Vector3(0, 0 ,-90)));
            Instantiate(BombStream, transform.position + new Vector3(0, i, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
            Instantiate(BombStream, transform.position + new Vector3(-i, 0, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
            Instantiate(BombStream, transform.position + new Vector3(i, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        isBomb = true;
        Destroy(gameObject, 0.8f);
    }

    public void BombBombBomb()
    {
        animator.SetTrigger("isBomb");
        BombAction();
    }
}

