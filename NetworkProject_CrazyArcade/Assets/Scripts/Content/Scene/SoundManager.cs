using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour 
{
	private AudioSource m_AudioSource;
	private AudioSource m_EffectSource;

	public static SoundManager Instance;
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		if (Instance == null)
		{
			Instance = new GameObject("SoundManager").AddComponent<SoundManager>();

			DontDestroyOnLoad(Instance.gameObject);
		}
		else if(Instance != null)
		{
			Destroy(Instance.gameObject);
		}
	}

	private void Awake()
	{
		m_AudioSource = GetComponent<AudioSource>();
		if (m_AudioSource == null)
		{
			m_AudioSource = gameObject.AddComponent<AudioSource>();
			m_AudioSource.loop = true;
			m_EffectSource = gameObject.AddComponent<AudioSource>();
		}
	}

	public void PlayEffectClip(AudioClip clip) 
	{
		m_EffectSource.Stop();
		m_EffectSource.clip = clip;
		m_EffectSource.Play();
	}

	public void StopEffectClip()
	{
		m_EffectSource.Stop();
		m_EffectSource.clip = null;
	}

	public void PlayEffectOneShot(AudioClip clip)
	{
		m_EffectSource.PlayOneShot(clip);
	}


	public void PlayBGM(AudioClip clip)
	{
		m_AudioSource.clip = clip;
		m_AudioSource.Play();
	}

	public void StopBGM()
	{
		m_AudioSource.Stop();
		m_AudioSource.clip = null;
	}

}
