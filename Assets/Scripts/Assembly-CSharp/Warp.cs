using UnityEngine;

public class Warp : MonoBehaviour
{
	public GameObject homeWarp;

	public GameObject[] standardWarps;

	public static Warp Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void Restart()
	{
		homeWarp.SetActive(false);
		for (int i = 0; i < standardWarps.Length; i++)
		{
			standardWarps[i].SetActive(true);
		}
	}

	public void OpenHomePortal(Vector2 pos)
	{
		homeWarp.SetActive(true);
		homeWarp.transform.position = pos;
	}
}
