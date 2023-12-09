using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public static PlayerName instance = null;
    public string playerName = "";

    void Awake()
    {
        // 싱글턴 패턴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }
     
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(playerName);
        }
    }
}
