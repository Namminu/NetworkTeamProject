using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class PlayerController : MonoBehaviour
{
    public Transform playerBomb;
    public GameObject Bomb;

    public int bombNum = 1;

    private Animator animator;
    private SpriteRenderer rend;

    //private Text playerName;
    private Transform tr;
    private PhotonView pv;
    private Vector2 currentPos; //�ǽ������� Vector3������ 2D���� �������̹Ƿ� Vector2�� ����
    private Quaternion currentRot;  //ȸ���� �ʿ��Ѱ�?

    private Direction dir = Direction.Down;
    private Dictionary<string, Direction> hitObjectDirs = new Dictionary<string, Direction>();
    private int bombCount = 0;

    private bool isPlayerDie;

    public PlayerStat playerstat;
    void Start()
    {
        playerstat.playerName = "";
        playerstat.bombLength = 1;
        playerstat.playerSpeed= 5.0f;
        playerstat.numberOfBombs= 1;

        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        isPlayerDie = false;
        //�� ���ӿ����� �÷��̾ ī�޶� ������ �ʿ䰡 ���ٰ� �����ؼ� �ϴ� �ּ�ó��
        //if (pv.isMine) Camera.main.GetComponent<FollowCam>().targetTr = tr;

        //����ȭ ���� ����
        // pv.ObservedComponents[0] = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlayerDie)
        {
            playerMove();
            PutBomb();
        }
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

                GameObject bomb = Instantiate(Bomb, new Vector3(bombX, bombY), Quaternion.identity);
                bomb.GetComponent<BombController>().setBombName("Bomb" + (++bombCount));
                bomb.GetComponent<BombController>().overlappingPlayer = gameObject.name;
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

        if (CheckDir(hitObjectDirs, latestKey))
            return;

        // �� Ű�� ���� ĳ���� �����̱�
        if (keyTimes[latestKey] != float.MinValue)
        {
            switch (latestKey)
            {
                case KeyCode.UpArrow:
                    dir = Direction.Up;
                    moveY = playerstat.playerSpeed * Time.deltaTime;
                    animator.SetBool("goUp", true);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", false);

                    rend.flipX = false;
                    break;
                case KeyCode.DownArrow:
                    dir = Direction.Down;
                    moveY = -playerstat.playerSpeed * Time.deltaTime;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", true);
                    animator.SetBool("goSide", false);

                    rend.flipX = false;
                    break;
                case KeyCode.LeftArrow:
                    dir = Direction.Left;
                    moveX = -playerstat.playerSpeed * Time.deltaTime;
                    animator.SetBool("goUp", false);
                    animator.SetBool("goDown", false);
                    animator.SetBool("goSide", true);
                    rend.flipX = false;

                    break;
                case KeyCode.RightArrow:
                    dir = Direction.Right;
                    moveX = playerstat.playerSpeed * Time.deltaTime;
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
        transform.position += moveDirection;
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BOMB" || collision.gameObject.tag == "VEHICLE")
        {
            if (collision.gameObject.tag == "BOMB")
                if (collision.GetComponent<BombController>().overlappingPlayer == gameObject.name)
                    return;

            SetHitDir(collision);
        }
        


        if (collision.gameObject.tag == "BombStream")
        {
            //Debug.Log("�� ���� �־��~");
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BOMB")
        {
            if (collision.GetComponent<BombController>().overlappingPlayer == gameObject.name)
            {
                collision.GetComponent<BombController>().overlappingPlayer = "";
                return;
            }

            if (hitObjectDirs.Count > 0)
                hitObjectDirs.Remove(collision.gameObject.GetComponent<BombController>().getBombName());
        }

        if (collision.gameObject.tag == "VEHICLE")
        {
            if (hitObjectDirs.Count > 0)
                hitObjectDirs.Remove(collision.gameObject.name);
        }
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
        else if (coll.gameObject.tag == "VEHICLE")
        {
            switch (dir)
            {
                case Direction.Up:
                    hitObjectDirs[coll.gameObject.name] = Direction.Up;
                    break;
                case Direction.Down:
                    hitObjectDirs[coll.gameObject.name] = Direction.Down;
                    break;
                case Direction.Left:
                    hitObjectDirs[coll.gameObject.name] = Direction.Left;
                    break;
                case Direction.Right:
                    hitObjectDirs[coll.gameObject.name] = Direction.Right;
                    break;
                case Direction.None:
                    break;
            }
        }
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


    public void IsDie()
    {
        isPlayerDie = true;
        animator.SetTrigger("isDie");
        Destroy(this, 1.0f);
    }
}

