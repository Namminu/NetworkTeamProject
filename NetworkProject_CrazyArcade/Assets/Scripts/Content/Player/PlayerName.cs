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

    /*----------- Lobby Scene ----------------*/
    //CreateNewRoom UI ����
    public void OnCilickCreateUIONButton()
    {
        gameObject.SetActive(true);
    }
	//CreateNewRoom UI �����
	public void OnCilickCreateUIOFFButton()
	{
		gameObject.SetActive(false);
	}

	//Password UI ���� 
	public void OnCilickPasswordUIONButton()
    {
		gameObject.SetActive(true);
	}
	//Password UI �����
	public void OnCilickPasswordUIOFFButton()
	{
		gameObject.SetActive(false);
	}
	/*----------- Wating Scene ------------- */
}
