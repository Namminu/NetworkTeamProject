using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInit : MonoBehaviour
{
    [SerializeField] private InputField nameInput;
    [SerializeField] private Button startBtn;

	// Start is called before the first frame update
	void Start()
    {
        nameInput.interactable = false;
        startBtn.interactable = false;
	}

    public void SetUIInteractable(bool b)
    {
        nameInput.interactable = true;
        startBtn.interactable = true;
    }

    public void SetPlayerName()
    {
        PhotonInit.Instance.SetPlayerName(nameInput.text);
	}
}
