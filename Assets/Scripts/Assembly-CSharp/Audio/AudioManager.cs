using UnityEngine;

namespace Audio
{
	public class AudioManager : MonoBehaviour
	{
		public Sound[] sounds;

		public Sound[] explosions;

		public AudioLowPassFilter filter;

		private float desiredFreq = 500f;

		private float velFreq;

		private float freqSpeed = 0.2f;

		public bool muted;

		public static AudioManager Instance { get; set; }

		private void Awake()
		{
			Instance = this;
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				sound.source = base.gameObject.AddComponent<AudioSource>();
				sound.source.clip = sound.clip;
				sound.source.loop = sound.loop;
				sound.source.volume = sound.volume;
				sound.source.pitch = sound.pitch;
				sound.source.bypassListenerEffects = sound.bypass;
			}
			array = explosions;
			foreach (Sound sound2 in array)
			{
				sound2.source = base.gameObject.AddComponent<AudioSource>();
				sound2.source.clip = sound2.clip;
				sound2.source.loop = sound2.loop;
				sound2.source.volume = sound2.volume;
				sound2.source.pitch = sound2.pitch;
			}
			Play("Song");
			SetVolume(0.7f);
		}

		private void Update()
		{
			if (filter != null)
			{
				filter.cutoffFrequency = Mathf.SmoothDamp(filter.cutoffFrequency, desiredFreq, ref velFreq, freqSpeed);
			}
		}

		public void MuteSounds(bool b)
		{
			muted = b;
		}

		public void PlayButton()
		{
			if (muted)
			{
				return;
			}
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				if (sound.name == "Button")
				{
					sound.source.pitch = 0.8f + Random.Range(-0.03f, 0.03f);
					break;
				}
			}
			Play("Button");
		}

		public void MuteMusic()
		{
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				if (sound.name == "Song")
				{
					sound.source.volume = 0f;
					break;
				}
			}
		}

		public void SetVolume(float v)
		{
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				if (sound.name == "Song")
				{
					sound.source.volume = v;
					break;
				}
			}
		}

		public void UnmuteMusic()
		{
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				if (sound.name == "Song")
				{
					sound.source.volume = 1.15f;
					break;
				}
			}
		}

		public void Play(string n)
		{
			if (muted && n != "Song")
			{
				return;
			}
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				if (sound.name == n)
				{
					sound.source.Play();
					break;
				}
			}
		}

		public void PlayHitEnemy()
		{
			if (!muted)
			{
				Play("Hit");
				int num = Random.Range(0, 4);
				explosions[num].source.Play();
			}
		}

		public void Stop(string n)
		{
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				if (sound.name == n)
				{
					sound.source.Stop();
					break;
				}
			}
		}

		public void SetFreq(float freq)
		{
			desiredFreq = freq;
		}
	}
}
