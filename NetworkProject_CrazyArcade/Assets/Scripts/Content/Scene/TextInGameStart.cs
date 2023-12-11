using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInGameStart : MonoBehaviour
{
	public Canvas cv_GameStart;
	private TextEffect TE;
	private void Start()
	{
		TE = GetComponent<TextEffect>();
		Invoke("callEffect", 1.5f);
	
	}
	private void callEffect() 
	{
		TE.StartTextEffect("Game Start!", Effect.FADE);
	}
}
