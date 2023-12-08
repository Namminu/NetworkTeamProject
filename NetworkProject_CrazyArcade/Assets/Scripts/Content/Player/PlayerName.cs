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
        Debug.Log("방 만들기 버튼 입력 : 창 띄우기");
        gameObject.SetActive(true);
    }
	//CreateNewRoom UI 지우기
	public void OnCilickCreateUIOFFButton()
	{
		Debug.Log("방 만들기 버튼 입력 : 창 내리기");
		gameObject.SetActive(false);
	}

	//Password UI 띄우기 
	public void OnCilickPasswordUIONButton()
    {
		Debug.Log("방 비밀번호 입력 버튼 입력 : 창 띄우기");
		gameObject.SetActive(true);
	}
	//Password UI 지우기
	public void OnCilickPasswordUIOFFButton()
	{
		Debug.Log("방 비밀번호 입력 버튼 입력 : 창 내리기");
		gameObject.SetActive(false);
	}
	/*----------- Wating Scene ------------- */
}
