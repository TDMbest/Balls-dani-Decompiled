using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
	private bool started;

	public TextMeshProUGUI levelText;

	public TextMeshProUGUI moneyText;

	public Image playerSprite;

	public Image extraSprite;

	private void Start()
	{
		UpdateLevel();
		started = true;
	}

	private void OnEnable()
	{
		if (started)
		{
			UpdateLevel();
		}
	}

	private void UpdateLevel()
	{
		levelText.text = "lvl " + SaveManager.Instance.state.GetLevel();
		moneyText.text = "$" + SaveManager.Instance.state.money;
		playerSprite.sprite = SaveManager.Instance.state.GetShape();
		extraSprite.sprite = SpriteManager.Instance.GetExtraSprite(SaveManager.Instance.state.currentFace);
		if (extraSprite.sprite == null)
		{
			extraSprite.enabled = false;
		}
		else
		{
			extraSprite.enabled = true;
		}
		playerSprite.color = SaveManager.Instance.state.GetColor();
	}
}
