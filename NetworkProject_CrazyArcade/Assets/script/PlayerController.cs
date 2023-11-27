using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Transform playerBomb;
    public GameObject Bomb;

    public int bombNum = 1;

    private Animator animator;
    private SpriteRenderer rend;

    private Transform tr;
    private PhotonView pv;
    private Vector2 currentPos; //�ǽ������� Vector3������ 2D���� �������̹Ƿ� Vector2�� ����
    private Quaternion currentRot;  //ȸ���� �ʿ��Ѱ�?

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        //�� ���ӿ����� �÷��̾ ī�޶� ������ �ʿ䰡 ���ٰ� �����ؼ� �ϴ� �ּ�ó��
        //if (pv.isMine) Camera.main.GetComponent<FollowCam>().targetTr = tr;

        //����ȭ ���� ����
       // pv.ObservedComponents[0] = this;
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        PutBomb();
    }

    //��ź ����
    void PutBomb()
    {
        if(bombNum > 0)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                float bombX = 0.0f;
                float bombY = 0.0f;

                if(transform.position.x >= 0)
                {
                    bombX = Mathf.FloorToInt(playerBomb.transform.position.x) + 0.5f;
                }
                else
                {
                    bombX = Mathf.CeilToInt(playerBomb.transform.position.x) - 0.5f;
                }

                if (transform.position.y >= 0)
                {
                    bombY = Mathf.FloorToInt(playerBomb.transform.position.y) + 0.5f;
                }
                else
                {
                    bombY = Mathf.CeilToInt(playerBomb.transform.position.y) - 0.5f;
                }

                Instantiate(Bomb, new Vector3(bombX, bombY), Quaternion.identity);
            }
        }
    }


    // Ű�� ó�� �ð�
    Dictionary<KeyCode, float> keyTimes = new Dictionary<KeyCode, float>()
    {
        { KeyCode.UpArrow, float.MinValue },
        { KeyCode.DownArrow, float.MinValue },
        { KeyCode.LeftArrow, float.MinValue },
        { KeyCode.RightArrow, float.MinValue },
    };

    void playerMove()
    {       
        float moveY = 0f;
        float moveX = 0f;

        // Ű�� �������� �ð� ���, ���� ���� �ð� ����
        foreach (KeyCode key in keyTimes.Keys.ToList())
        {
            if (Input.GetKeyDown(key))
            {
                keyTimes[key] = Time.time;
            }
            else if (Input.GetKeyUp(key))
            {
                keyTimes[key] = float.MinValue;
            }
        }

        // ���� �ֱٿ� ���� Ű ã��
        KeyCode latestKey = keyTimes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        // �� Ű�� ���� ĳ���� �����̱�
        if (keyTimes[latestKey] != float.MinValue) 
        {
            switch (latestKey)
            {
                case KeyCode.UpArrow:
                    moveY = moveSpeed * Time.deltaTime;
                    animator.SetBool("goUp", true);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", false);

                    rend.flipX = false;
                    break;
                case KeyCode.DownArrow:
                    moveY = -moveSpeed * Time.deltaTime;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", true);
                    animator.SetBool("goSide", false);

                    rend.flipX = false;
                    break;
                case KeyCode.LeftArrow:
                    moveX = -moveSpeed * Time.deltaTime;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", true);
                    rend.flipX = false;

                    break;
                case KeyCode.RightArrow:
                    moveX = moveSpeed * Time.deltaTime;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", true);
                    rend.flipX = true;
                    break;
            }
        }
        else
        {
            animator.SetBool("goUp", false);
            animator.SetBool("goDown", false);
            animator.SetBool("goSide", false);

        }
        

        Vector3 moveDirection = new Vector3(moveX, moveY, 0);
        transform.position += moveDirection;
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�� �¾Ҿ��");
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)    //�� ��ġ ������ ���� ����
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else //�ٸ� ������� ��ġ ����ȭ
        {
            currentPos = (Vector2)stream.ReceiveNext();
            currentRot = (Quaternion)stream.ReceiveNext();
        }
    }

}

