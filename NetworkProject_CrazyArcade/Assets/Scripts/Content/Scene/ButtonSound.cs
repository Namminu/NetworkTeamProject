using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
	private SoundManager sound;
	private AudioSource clickSound;

	// Start is called before the first frame update
	void Start()
    {
		sound = FindObjectOfType<SoundManager>();
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            clickSound.Play();
        }
    }
}
