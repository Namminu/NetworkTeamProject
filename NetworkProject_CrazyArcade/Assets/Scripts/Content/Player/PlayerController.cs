using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public Transform playerBomb;
    public GameObject Bomb;

    public int bombNum = 1;

    private Animator animator;
    private SpriteRenderer rend;
    private Rigidbody2D rb;

    //private Text playerName;
    private Transform tr;
    private PhotonView pv;
    private Vector3 currentPos; //�ǽ������� Vector3������ 2D���� �������̹Ƿ� Vector2�� ����
    private Quaternion currentRot;  //ȸ���� �ʿ��Ѱ�?

    private Direction dir = Direction.Down;
    private Dictionary<string, Direction> hitObjectDirs = new Dictionary<string, Direction>();

    private int bombCount = 0;

    private bool isPlayerDie;
    private bool isBomb = false;


    public PlayerStat playerstat;

    [SerializeField]
    private GameObject[] bombNumbers;

    private Vector3 movespeed;

    void Start()
    {
        playerstat.playerName = "";

        bombNumbers = new GameObject[playerstat.numberOfBombs];
        System.Array.Clear(bombNumbers, 0, playerstat.numberOfBombs);
        //playerstat = GetComponent<PlayerStat>();          // Ȥ�� �𸣴� �ּ� ó��

        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        isPlayerDie = false;
        //�� ���ӿ����� �÷��̾ ī�޶� ������ �ʿ䰡 ���ٰ� �����ؼ� �ϴ� �ּ�ó��
        //if (pv.isMine) Camera.main.GetComponent<FollowCam>().targetTr = tr;

        //����ȭ ���� ����
        pv.ObservedComponents[0] = this;

        movespeed = Vector3.zero;
    }

    private void FixedUpdate() 
    {
        if(pv.IsMine)
        {
			if (!isPlayerDie)
				rb.velocity = movespeed;
		}
        else
        {
            rb.velocity = Vector3.Lerp(tr.position, currentPos, Time.deltaTime * 10.0f);
        }
    }
    // Update is called once per framex
    void Update()
    {
		if (!isPlayerDie)
		{
			movespeed = playerMove();

			//��ź ����
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PutBomb();
			}
			//��ź ���� üũ
			CheckNumberBombs();
		}

    }

	//��ź ���� ȣ��
	void PutBomb()  //���� �÷��̾ �۵��ϱ� ���� ��ź ���� �Լ�
	{
		StartCoroutine(CreateBomb());
		pv.RPC("PutBombRPC", RpcTarget.Others);
	}

	[PunRPC]
	void PutBombRPC()   //�ٸ� �÷��̾��� ����ȭ�� ���� ��ź ���� �Լ�
	{
		StartCoroutine(CreateBomb());
	}
	//��ź ���� �ڷ�ƾ
	IEnumerator CreateBomb()
    {
		if (playerstat.numberOfBombs > bombCount && !isBomb)
		{
			float bombX = 0.0f;
			float bombY = 0.0f;

			if (transform.position.x >= 0)
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

			GameObject bomb = Instantiate(Bomb, new Vector3(bombX, bombY), Quaternion.identity);

			for (int i = 0; i < playerstat.numberOfBombs; i++)
			{
				if (bombNumbers[i] == null)
				{
					bombCount++;
					bombNumbers[i] = bomb;
					break;
				}
			} 

			bomb.GetComponent<BombController>().BombstreamLength(playerstat.bombLength);
			bomb.GetComponent<BombController>().setBombName("Bomb" + (++bombCount));
			bomb.GetComponent<BombController>().overlappingPlayer = gameObject.name;
		}
		yield return null;
    }

	void CheckNumberBombs()
    {
        System.Array.Resize(ref bombNumbers, playerstat.numberOfBombs);
        int bombnotNull = 0;
        for (int i = 0; i < playerstat.numberOfBombs; i++)
        {
            if (bombNumbers[i] != null)
            {
                bombnotNull++;
            }
        }

        if(bombnotNull != bombCount)
            bombCount = bombnotNull;
    }



    // Ű�� ó�� �ð�
    Dictionary<KeyCode, float> keyTimes = new Dictionary<KeyCode, float>()
    {
        { KeyCode.UpArrow, float.MinValue },
        { KeyCode.DownArrow, float.MinValue },
        { KeyCode.LeftArrow, float.MinValue },
        { KeyCode.RightArrow, float.MinValue },
    };

    Vector3 playerMove()
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

        if (CheckDir(hitObjectDirs, latestKey))
            return Vector3.zero;

        // �� Ű�� ���� ĳ���� �����̱�
        if (keyTimes[latestKey] != float.MinValue)
        {
            switch (latestKey)
            {
                case KeyCode.UpArrow:
                    dir = Direction.Up;
                        moveY = playerstat.playerSpeed ;
                    animator.SetBool("goUp", true);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", false);

                    rend.flipX = false;
                    break;
                case KeyCode.DownArrow:
                    dir = Direction.Down;
                        moveY = -playerstat.playerSpeed ;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", true);
                    animator.SetBool("goSide", false);

                    rend.flipX = false;
                    break;
                case KeyCode.LeftArrow:
                    dir = Direction.Left;
                        moveX = -playerstat.playerSpeed ;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", true);
                    rend.flipX = false;

                    break;
                case KeyCode.RightArrow:
                    dir = Direction.Right;
                        moveX = playerstat.playerSpeed ;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", true);

                    rend.flipX = true;
                    break;
            }
        }
        else
        {
            dir = Direction.None;
            animator.SetBool("goUp", false);
            animator.SetBool("goDown", false);
            animator.SetBool("goSide", false);

        }


        Vector3 moveDirection = new Vector3(moveX, moveY, 0);
        //transform.position += moveDirection;
        return moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BOMB" /*|| collision.gameObject.tag == "VEHICLE"*/)
        {
            if (collision.gameObject.tag == "BOMB")
                if (collision.GetComponent<BombController>().overlappingPlayer == gameObject.name)
                    return;

            SetHitDir(collision);
        }       
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BOMB")
        {
            BombController bombColl = collision.GetComponent<BombController>();
            if (bombColl.overlappingPlayer == gameObject.name)
            {
                bombColl.overlappingPlayer = "";
                return;
            }

            if (hitObjectDirs.Count > 0 && hitObjectDirs.ContainsKey(bombColl.getBombName()))
            {

                hitObjectDirs.Remove(bombColl.getBombName());
            }
        }

        //if (collision.gameObject.tag == "VEHICLE")
        //{
        //    if (hitObjectDirs.Count > 0 && hitObjectDirs.ContainsKey(collision.gameObject.name))
        //    {
        //        hitObjectDirs.Remove(collision.gameObject.name);
        //    }
        //}
    }

    bool CheckDir(Dictionary<string, Direction> object_directions, KeyCode latestKey)
    {
        if (object_directions.Count != 0)
        {
            foreach (KeyValuePair<string, Direction> direction in object_directions)
            {
                if (direction.Value == Direction.Up && latestKey == KeyCode.UpArrow)
                    return true;
                else if (direction.Value == Direction.Down && latestKey == KeyCode.DownArrow)
                    return true;
                else if (direction.Value == Direction.Left && latestKey == KeyCode.LeftArrow)
                    return true;
                else if (direction.Value == Direction.Right && latestKey == KeyCode.RightArrow)
                    return true;
            }
        }
        return false;
    }

    void SetHitDir(Collider2D coll)
    {
        if (coll.gameObject.tag == "BOMB")
        {
            switch (dir)
            {
                case Direction.Up:
                    hitObjectDirs[coll.gameObject.GetComponent<BombController>().getBombName()] = Direction.Up;
                    break;
                case Direction.Down:
                    hitObjectDirs[coll.gameObject.GetComponent<BombController>().getBombName()] = Direction.Down;
                    break;
                case Direction.Left:
                    hitObjectDirs[coll.gameObject.GetComponent<BombController>().getBombName()] = Direction.Left;
                    break;
                case Direction.Right:
                    hitObjectDirs[coll.gameObject.GetComponent<BombController>().getBombName()] = Direction.Right;
                    break;
                case Direction.None:
                    break;
            }
        }
        //else if (coll.gameObject.tag == "VEHICLE")
        //{
        //    switch (dir)
        //    {
        //        case Direction.Up:
        //            hitObjectDirs[coll.gameObject.name] = Direction.Up;
        //            break;
        //        case Direction.Down:
        //            hitObjectDirs[coll.gameObject.name] = Direction.Down;
        //            break;
        //        case Direction.Left:
        //            hitObjectDirs[coll.gameObject.name] = Direction.Left;
        //            break;
        //        case Direction.Right:
        //            hitObjectDirs[coll.gameObject.name] = Direction.Right;
        //            break;
        //        case Direction.None:
        //            break;
        //    }
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)    //�� ��ġ ������ ���� ����
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else //�ٸ� ������� ��ġ ����ȭ
        {
            currentPos = (Vector3)stream.ReceiveNext();
            currentRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void IsDie()
    {
        rb.velocity = Vector3.zero;
        isPlayerDie = true;
        animator.SetTrigger("isDie");
        Destroy(gameObject, 2.0f);
    }

    public void IsBomb()
    {
        isBomb = true;
    }
    public void IsNotBomb()
    {
        isBomb = false; 
    }

}

