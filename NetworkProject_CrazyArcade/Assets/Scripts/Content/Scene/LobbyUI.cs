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
    private InputField roomName; //방 이름 입력
    [SerializeField]
    private InputField passwordNumber; // 비밀번호 입력
    [SerializeField]
    private GameObject makeRoomPopupUI; //방 만들기 팝업
    [SerializeField]
    private GameObject passwordInputUI; // 방 만들기 비밀번호 UI
    [SerializeField]
    private Toggle myToggle; // 비밀번호 설정 토글
    [SerializeField]
    private Button[] roomNumberButton;

    private int makingRoomState= 1; // 비밀번호 유무에 따른 상태
    private int roomNumberIndex = 0; // 방 번호용 인덱스
    private bool eraseRoom =false;

    void Start()
    {
        // Toggle 객체의 onValueChanged 이벤트에 콜백 함수를 등록합니다.
        myToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(myToggle); });
    }

    private void Update()
    {
        //2번 누르면 방 삭제함
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cancelRoom();
            Debug.Log("방이 사라짐");
        }
        UpdateTexts();
    }

    /// <summary>
    /// 방 이름 띄워주는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <param name="roomName"></param>
    void PutRoom(int index, string roomName)
    {
            Text buttonText = roomNumberButton[index].GetComponentInChildren<Text>();
                buttonText.text = (roomName);
    }

    /// <summary>
    /// 방 없어지면 나머지 방 앞으로 땡겨주는 함수
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
                    buttonText.text = null; // 마지막 버튼의 텍스트를 null로 설정
                    roomNumberIndex--;
                    eraseRoom = false;
                }

            }
        }
    }

    /// <summary>
    /// 방 삭제를 위한 임시 코드
    /// </summary>
    void cancelRoom()
    {
        Text buttonText = roomNumberButton[1].GetComponentInChildren<Text>();
        buttonText.text = "";
        eraseRoom = true;
    }

    /// <summary>
    /// 메인화면으로 UI
    /// </summary>
    public void MoveToMainUI()
    {
        SceneManager.LoadScene("MainLevel");
    }

    /// <summary>
    /// 방 만들기 UI 팝업
    /// </summary>
    public void MakeRoom()
    {
        makeRoomPopupUI.SetActive(true);
    }

    /// <summary>
    /// 비번 유무에 따라 방 생성 버튼
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
    /// 인풋값 초기화
    /// </summary>
    void ClearInputText()
    {
        roomName.text = "";
        passwordNumber.text = "";
    }

    /// <summary>
    /// 방 만들기 UI 확인 버튼
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
    /// 비번 없는 방 생성 버튼
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
    /// 방 만들기 UI X버튼
    /// </summary>
    public void stopMakingRoom()
    {
        makeRoomPopupUI.SetActive(false);
        ClearInputText();
    }

    // Toggle의 상태가 변했을 때 호출되는 함수입니다.
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
