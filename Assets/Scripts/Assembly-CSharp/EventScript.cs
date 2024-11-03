using TMPro;
using UnityEngine;

public class EventScript : MonoBehaviour
{
	private Vector2 fullScale;

	public TextMeshProUGUI progress;

	public Transform progressBar;

	public GameObject unlockText;

	private int desiredKills;

	private int currentKills;

	public static EventScript Instance { get; private set; }

	private void Start()
	{
		Instance = this;
		fullScale = progressBar.transform.localScale;
		progressBar.transform.localScale = Vector2.zero;
		UpdateProgress();
	}

	public void UpdateProgress()
	{
		currentKills = SaveManager.Instance.state.eventKills;
		desiredKills = SaveManager.Instance.state.desiredEventKills;
		float num = (float)currentKills / (float)desiredKills;
		progressBar.transform.localScale = new Vector3(fullScale.x * num, fullScale.y);
		progress.text = currentKills + "/" + desiredKills;
		if (currentKills == desiredKills)
		{
			progress.text += " (UNLOCKED)";
			unlockText.SetActive(false);
		}
	}
}
