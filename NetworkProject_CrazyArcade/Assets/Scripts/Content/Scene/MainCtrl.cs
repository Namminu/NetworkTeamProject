using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCtrl : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.LoadScene("LobbyLevel");
        //PhotonNewtwork.LoadLevel("LobbyLevel");
    }
} 
