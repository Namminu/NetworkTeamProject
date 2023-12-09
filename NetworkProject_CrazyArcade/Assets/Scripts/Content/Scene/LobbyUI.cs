using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private InputField roomName; //�� �̸� �Է�
    [SerializeField]
    private InputField passwordNumber; // ��й�ȣ �Է�
    [SerializeField]
    private GameObject makeRoomPopupUI; //�� ����� �˾�
    [SerializeField]
    private GameObject passwordInputUI; // �� ����� ��й�ȣ UI
    [SerializeField]
    private Toggle myToggle; // ��й�ȣ ���� ���
    [SerializeField]
    private Button[] roomNumberButton;

    private int makingRoomState= 1; // ��й�ȣ ������ ���� ����
    private int roomNumberIndex = 0; // �� ��ȣ�� �ε���
    private bool eraseRoom =false;

    void Start()
    {
        // Toggle ��ü�� onValueChanged �̺�Ʈ�� �ݹ� �Լ��� ����մϴ�.
        myToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(myToggle); });
    }

    private void Update()
    {
        //2�� ������ �� ������
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cancelRoom();
            Debug.Log("���� �����");
        }
        UpdateTexts();
    }

    /// <summary>
    /// �� �̸� ����ִ� �Լ�
    /// </summary>
    /// <param name="index"></param>
    /// <param name="roomName"></param>
    void PutRoom(int index, string roomName)
    {
            Text buttonText = roomNumberButton[index].GetComponentInChildren<Text>();
                buttonText.text = (roomName);
    }

    /// <summary>
    /// �� �������� ������ �� ������ �����ִ� �Լ�
    /// </summary>
    void UpdateTexts() 
    {
        for (int i = 0; i < roomNumberButton.Length; i++)
        {
            Text buttonText = roomNumberButton[i].GetComponentInChildren<Text>();
            if(eraseRoom==true)
            {
                if (buttonText.text == null || buttonText.text == "")
                {
                    for (int j = i; j < roomNumberButton.Length - 1; j++)
                    {
                        Text nextButtonText = roomNumberButton[j + 1].GetComponentInChildren<Text>();
                        buttonText.text = nextButtonText.text;
                        buttonText = nextButtonText;
                    }
                    buttonText.text = null; // ������ ��ư�� �ؽ�Ʈ�� null�� ����
                    roomNumberIndex--;
                    eraseRoom = false;
                }

            }
        }
    }

    /// <summary>
    /// �� ������ ���� �ӽ� �ڵ�
    /// </summary>
    void cancelRoom()
    {
        Text buttonText = roomNumberButton[1].GetComponentInChildren<Text>();
        buttonText.text = "";
        eraseRoom = true;
    }

    /// <summary>
    /// ����ȭ������ UI
    /// </summary>
    public void MoveToMainUI()
    {
        SceneManager.LoadScene("MainLevel");
    }

    /// <summary>
    /// �� ����� UI �˾�
    /// </summary>
    public void MakeRoom()
    {
        makeRoomPopupUI.SetActive(true);
    }

    /// <summary>
    /// ��� ������ ���� �� ���� ��ư
    /// </summary>
    public void pressedOkMakingRoom()
    {
        switch(makingRoomState)
        {
            case 0:
                SucceededMakingPWRoom(roomName, passwordNumber);
                break;
            case 1:
                SucceededMakingNonePWRoom(roomName);
                break;
        }
    }

    /// <summary>
    /// ��ǲ�� �ʱ�ȭ
    /// </summary>
    void ClearInputText()
    {
        roomName.text = "";
        passwordNumber.text = "";
    }

    /// <summary>
    /// �� ����� UI Ȯ�� ��ư
    /// </summary>
    /// <param name="nameOfTheRoom"></param>
    /// <param name="passwordOfTheRoom"></param>
    private void SucceededMakingPWRoom(InputField nameOfTheRoom, InputField passwordOfTheRoom)
    {
        if(nameOfTheRoom == null || passwordOfTheRoom ==null) { }
        else
        {
            PutRoom(roomNumberIndex,nameOfTheRoom.text);
            roomNumberIndex++;
        }
        makeRoomPopupUI.SetActive(false);
        ClearInputText();
    }

    /// <summary>
    /// ��� ���� �� ���� ��ư
    /// </summary>
    /// <param name="nameOfTheRoom"></param>
    private void SucceededMakingNonePWRoom(InputField nameOfTheRoom)
    {

        if (nameOfTheRoom == null) { }
        else
        {
            PutRoom(roomNumberIndex, nameOfTheRoom.text);
            roomNumberIndex++;
        }
        makeRoomPopupUI.SetActive(false);
        ClearInputText();
    }

    /// <summary>
    /// �� ����� UI X��ư
    /// </summary>
    public void stopMakingRoom()
    {
        makeRoomPopupUI.SetActive(false);
        ClearInputText();
    }

    // Toggle�� ���°� ������ �� ȣ��Ǵ� �Լ��Դϴ�.
    public void ToggleValueChanged(Toggle change)
    {
        if (myToggle.isOn)
        {
            passwordInputUI.SetActive(true);
            makingRoomState = 0;
        }
        else
        {
            passwordInputUI.SetActive(false);
            makingRoomState = 1;
        }
    }
}
