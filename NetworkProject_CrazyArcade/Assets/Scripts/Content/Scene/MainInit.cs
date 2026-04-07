using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInit : MonoBehaviour
{
    [SerializeField] private InputField nameInput;
    [SerializeField] private Button startBtn;

    [SerializeField]
    private AudioClip main_lobby_BGM;
    [SerializeField]
    private AudioClip buttonSound;
	// Start is called before the first frame update
	void Start()
    {
        nameInput.interactable = false;
        startBtn.interactable = false;

        SoundManager.Instance.PlayBGM(main_lobby_BGM);
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

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			SoundManager.Instance.PlayEffectOneShot(buttonSound);
		}
	}
}
