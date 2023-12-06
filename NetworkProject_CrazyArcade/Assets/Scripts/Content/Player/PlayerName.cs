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

    /*----------- Lobby Scene ----------------*/
    //CreateNewRoom UI 띄우기
    public void OnCilickCreateUIONButton()
    {
        gameObject.SetActive(true);
    }
	//CreateNewRoom UI 지우기
	public void OnCilickCreateUIOFFButton()
	{
		gameObject.SetActive(false);
	}

	//Password UI 띄우기 
	public void OnCilickPasswordUIONButton()
    {
		gameObject.SetActive(true);
	}
	//Password UI 지우기
	public void OnCilickPasswordUIOFFButton()
	{
		gameObject.SetActive(false);
	}
	/*----------- Wating Scene ------------- */
}
