using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
	public TextMeshProUGUI kills;

	public TextMeshProUGUI dist;

	public TextMeshProUGUI highscore;

	public TextMeshProUGUI level;

	public TextMeshProUGUI death;

	private void OnEnable()
	{
		kills.text = string.Concat(SaveManager.Instance.state.kills);
		dist.text = string.Concat(SaveManager.Instance.state.distanceTravelled);
		highscore.text = string.Concat(SaveManager.Instance.state.highScore);
		level.text = string.Concat(SaveManager.Instance.state.GetLevel());
		death.text = string.Concat(SaveManager.Instance.state.deaths);
	}

	public void DeleteStats()
	{
		AudioManager.Instance.Stop("Song");
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
