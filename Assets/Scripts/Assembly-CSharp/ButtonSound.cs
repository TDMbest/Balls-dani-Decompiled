using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
	public AudioClip sound;

	private Button b
	{
		get
		{
			return GetComponent<Button>();
		}
	}

	private AudioSource source
	{
		get
		{
			return GetComponent<AudioSource>();
		}
	}

	private void Start()
	{
		base.gameObject.AddComponent<AudioSource>();
		source.clip = sound;
		source.playOnAwake = false;
		b.onClick.AddListener(PlaySound);
	}

	private void PlaySound()
	{
		source.PlayOneShot(sound);
	}
}
