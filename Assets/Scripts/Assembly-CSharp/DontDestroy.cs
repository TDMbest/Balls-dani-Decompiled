using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void LoadGame()
	{
		SceneManager.LoadScene("SampleScene");
	}
}
