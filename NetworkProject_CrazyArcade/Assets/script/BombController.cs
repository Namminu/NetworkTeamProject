using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float bombTime = 3.0f;
    public int streamLength = 1;
    public GameObject BombStream;
    private Animator animator;
    private bool isBomb = false;

    private string bombName = "";
    public string overlappingPlayer = "";               // Player가 폭탄 배치시 오버랩된 플레이어를 판별하기 위한 string

    public string getBombName() { return bombName; }
    public void setBombName(string name) { bombName = name; }

    private int raycastDistance;
    private float distanceUp;
    private float distanceDown;
    private float distanceLeft;
    private float distanceRight;
    enum Way
    {
        up = 0,
        down,
        left,
        right,
        none
    }
    //private Way way;

    // Start is called before the first frame upd ate
    void Start()
    {
        
        animator = GetComponent<Animator>();
        Invoke("BombAction", bombTime);
        raycastDistance = streamLength;


        distanceUp = streamLength;
        distanceDown = streamLength;
        distanceLeft = streamLength;
        distanceRight = streamLength;
        // 상
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("Wall"));
        if (hitUp.collider != null)
        {
            distanceUp = hitUp.distance;

            Debug.Log("상 방향과의 거리: " + distanceUp);
        }

        // 디버그 Ray 그리기
        Debug.DrawRay(transform.position, Vector2.up * raycastDistance, Color.red);

        // 하
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("Wall"));
        if (hitDown.collider != null)
        {
            distanceDown = hitDown.distance;
            Debug.Log("하 방향과의 거리: " + distanceDown);
        }

        // 디버그 Ray 그리기
        Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.green);

        // 좌
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, LayerMask.GetMask("Wall"));
        if (hitLeft.collider != null)
        {
            distanceLeft = hitLeft.distance;
            Debug.Log("좌 방향과의 거리: " + distanceLeft);
        }

        // 디버그 Ray 그리기
        Debug.DrawRay(transform.position, Vector2.left * raycastDistance, Color.blue);

        // 우
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("Wall"));
        if (hitRight.collider != null)
        {
            distanceRight = hitRight.distance;
            Debug.Log("우 방향과의 거리: " + distanceRight);
        }

        // 디버그 Ray 그리기
        Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.yellow);

    }

    // Update is called once per frame
    void Update()
    { 


            
    }


    void BombAction()
    {
        if (isBomb)
            return;

        Instantiate(BombStream, transform.position, Quaternion.identity);
        for (int i = 1; i <= streamLength; i++)
        {

            //위
            if (distanceUp >= i)
            {

                
                Instantiate(BombStream, transform.position + new Vector3(0, i, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
            }
            
            // 아래
            if(distanceDown >= i)
            {

                
                Instantiate(BombStream, transform.position + new Vector3(0, -i, 0), Quaternion.Euler(new Vector3(0, 0 ,-90)));
            }

            // 왼쪽
            if (distanceLeft >= i)
            {

                
                Instantiate(BombStream, transform.position + new Vector3(-i, 0, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
            }

            // 오른쪽
            if (distanceRight >= i)
            {

                
                Instantiate(BombStream, transform.position + new Vector3(i, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            }

        }
        isBomb = true;
        Destroy(gameObject, 0.8f);
    }

    // 물줄기에 폭탄이 닿았을때 터지기
    public void BombBombBomb()
    {
        animator.SetTrigger("isBomb");
        BombAction();
    }
}

