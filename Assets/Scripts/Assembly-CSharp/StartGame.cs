using Game;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
	public GameObject defaultUI;

	public bool perf;

	public bool pull;

	private bool selectedG;

	private bool selectedC;

	public Image pBtn;

	public Image gBtn;

	public Image startBtn;

	public Image pullBtn;

	public Image pushBtn;

	private void Start()
	{
		if (SaveManager.Instance.state.startedSaveFile)
		{
			base.gameObject.SetActive(false);
			defaultUI.SetActive(true);
			return;
		}
		pBtn.color = Color.white;
		gBtn.color = Color.white;
		pull = true;
		pullBtn.color = Color.white;
		pushBtn.color = Color.gray;
	}

	public void SelectButton(string c)
	{
		startBtn.color = Color.white;
		selectedG = true;
		if (c == "perf")
		{
			pBtn.color = Color.white;
			gBtn.color = Color.gray;
			perf = true;
		}
		if (c == "grap")
		{
			pBtn.color = Color.gray;
			gBtn.color = Color.white;
			perf = false;
		}
		GameController.Instance.SetPerformanceMode(perf);
	}

	public void SelectControls(string c)
	{
		selectedC = true;
		if (c == "pull")
		{
			pullBtn.color = Color.white;
			pushBtn.color = Color.gray;
			pull = true;
		}
		if (c == "push")
		{
			pullBtn.color = Color.gray;
			pushBtn.color = Color.white;
			pull = false;
		}
	}

	public void GoToLobby()
	{
		if (selectedG)
		{
			GameController.Instance.SetPerformanceMode(perf);
			SaveManager.Instance.state.startedSaveFile = true;
			SaveManager.Instance.state.pull = pull;
			SaveManager.Instance.state.performance = perf;
			SaveManager.Instance.Save();
			base.gameObject.SetActive(false);
			defaultUI.SetActive(true);
		}
	}
}
