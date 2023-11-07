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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        PutBomb();
    }

    //폭탄 놓기
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
                    bombX = Mathf.FloorToInt(transform.position.x) + 0.5f;
                }
                else
                {
                    bombX = Mathf.CeilToInt(transform.position.x) - 0.5f;
                }

                if (transform.position.y >= 0)
                {
                    bombY = Mathf.FloorToInt(transform.position.y) + 0.5f;
                }
                else
                {
                    bombY = Mathf.CeilToInt(transform.position.y) - 0.5f;
                }

                Instantiate(Bomb, new Vector3(bombX, bombY), Quaternion.identity);
            }
        }
    }


    // 키의 처음 시간
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

        // 키가 눌렸을때 시간 기록, 땠을 때의 시간 리셋
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

        // 가장 최근에 눌린 키 찾기
        KeyCode latestKey = keyTimes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        // 그 키에 따라 캐릭터 움직이기
        if (keyTimes[latestKey] != float.MinValue)
        {
            switch (latestKey)
            {
                case KeyCode.UpArrow:
                    moveY = moveSpeed * Time.deltaTime;
                    break;
                case KeyCode.DownArrow:
                    moveY = -moveSpeed * Time.deltaTime;
                    break;
                case KeyCode.LeftArrow:
                    moveX = -moveSpeed * Time.deltaTime;
                    break;
                case KeyCode.RightArrow:
                    moveX = moveSpeed * Time.deltaTime;
                    break;
            }
        }

        Vector3 moveDirection = new Vector3(moveX, moveY, 0);
        transform.position += moveDirection;
    }

}
