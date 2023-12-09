using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public static PlayerName instance = null;
    public string playerName = "";

    void Awake()
    {
        // �̱��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �ı����� �ʵ��� ����
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
