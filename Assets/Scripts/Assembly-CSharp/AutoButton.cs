using UnityEngine;

public class AutoButton : MonoBehaviour
{
	public SpriteRenderer sr;

	public static AutoButton Instance { get; set; }

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	public void SetActive(bool b)
	{
		if (b)
		{
			sr.color = Color.white;
		}
		else
		{
			sr.color = Color.gray;
		}
	}
}
