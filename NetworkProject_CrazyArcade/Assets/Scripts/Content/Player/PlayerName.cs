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
        Debug.Log("�� ����� ��ư �Է� : â ����");
        gameObject.SetActive(true);
    }
	//CreateNewRoom UI �����
	public void OnCilickCreateUIOFFButton()
	{
		Debug.Log("�� ����� ��ư �Է� : â ������");
		gameObject.SetActive(false);
	}

	//Password UI ���� 
	public void OnCilickPasswordUIONButton()
    {
		Debug.Log("�� ��й�ȣ �Է� ��ư �Է� : â ����");
		gameObject.SetActive(true);
	}
	//Password UI �����
	public void OnCilickPasswordUIOFFButton()
	{
		Debug.Log("�� ��й�ȣ �Է� ��ư �Է� : â ������");
		gameObject.SetActive(false);
	}
	/*----------- Wating Scene ------------- */
}
