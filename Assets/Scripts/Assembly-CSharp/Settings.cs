using Audio;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	private bool performance;

	private bool pull;

	private int sensitivity;

	private int difficulty;

	public Image pBtn;

	public Image gBtn;

	public Image pullBtn;

	public Image pushBtn;

	public Transform senSlider;

	public Transform diffSlider;

	private void OnEnable()
	{
		performance = SaveManager.Instance.state.performance;
		pull = SaveManager.Instance.state.pull;
		sensitivity = SaveManager.Instance.state.sensitivity;
		difficulty = SaveManager.Instance.state.difficulty;
		UpdateButtons();
		UpdateSensitivity(senSlider, sensitivity);
		UpdateSensitivity(diffSlider, difficulty);
	}

	private void UpdateButtons()
	{
		if (performance)
		{
			pBtn.color = Color.white;
			gBtn.color = Color.gray;
		}
		else
		{
			pBtn.color = Color.gray;
			gBtn.color = Color.white;
		}
		if (pull)
		{
			pullBtn.color = Color.white;
			pushBtn.color = Color.gray;
		}
		if (!pull)
		{
			pullBtn.color = Color.gray;
			pushBtn.color = Color.white;
		}
		SaveManager.Instance.state.pull = pull;
		SaveManager.Instance.state.performance = performance;
		SaveManager.Instance.Save();
		GameController.Instance.SetPerformanceMode(performance);
	}

	public void Performance()
	{
		performance = true;
		UpdateButtons();
	}

	public void Graphics()
	{
		performance = false;
		UpdateButtons();
	}

	public void Pull(bool p)
	{
		pull = p;
		UpdateButtons();
	}

	public void SetMusic(bool b)
	{
		if (!b)
		{
			AudioManager.Instance.Stop("Song");
		}
		if (b)
		{
			AudioManager.Instance.Play("Song");
		}
	}

	public void ChangeSensitivity(int s)
	{
		if (s > 0)
		{
			if (sensitivity < 4)
			{
				sensitivity++;
			}
		}
		else if (s < 0 && sensitivity > 0)
		{
			sensitivity--;
		}
		UpdateSensitivity(senSlider, sensitivity);
		SaveManager.Instance.state.sensitivity = sensitivity;
		SaveManager.Instance.Save();
	}

	public void ChangeDifficulty(int s)
	{
		if (s > 0)
		{
			if (difficulty < 2)
			{
				difficulty++;
			}
		}
		else if (s < 0 && difficulty > 0)
		{
			difficulty--;
		}
		UpdateSensitivity(diffSlider, difficulty);
		SaveManager.Instance.state.difficulty = difficulty;
		SaveManager.Instance.Save();
	}

	private void UpdateSensitivity(Transform slider, int setting)
	{
		ResetSlider(slider);
		for (int i = 0; i <= setting; i++)
		{
			slider.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
		}
	}

	private void ResetSlider(Transform slider)
	{
		for (int i = 0; i < slider.childCount; i++)
		{
			slider.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
		}
	}

	public void SetSounds(bool b)
	{
		AudioManager.Instance.MuteSounds(!b);
	}
}
