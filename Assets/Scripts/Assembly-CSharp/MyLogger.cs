using TMPro;
using UnityEngine;

public class MyLogger : MonoBehaviour
{
	public TextMeshProUGUI text;

	public static MyLogger Instance { get; set; }

	private void Awake()
	{
		Instance = this;
	}

	public void Append(string t)
	{
		TextMeshProUGUI textMeshProUGUI = text;
		textMeshProUGUI.text = textMeshProUGUI.text + t + "\n";
	}
}
