using System;
using Audio;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTab : MonoBehaviour
{
	public GameObject juiceBtn;

	public GameObject slowmoBtn;

	public GameObject comboBtn;

	public GameObject speedBtn;

	public GameObject missileBtn;

	public GameObject splitBtn;

	public GameObject bMissileBtn;

	public GameObject autoBounceBtn;

	public GameObject cubeBtn;

	public GameObject spikeBtn;

	public GameObject superSpikeBtn;

	public GameObject triangleBtn;

	public GameObject explosionBtn;

	public GameObject missileExplosionBtn;

	public GameObject goldBtn;

	public GameObject loveBtn;

	public Image buyBtn;

	public TextMeshProUGUI money;

	public TextMeshProUGUI shoptext;

	public GameObject upgradeFx;

	private GameObject currentBtn;

	private string currentString;

	private int max = 9;

	public static UpgradeTab Instance { get; set; }

	private void Awake()
	{
		Instance = this;
	}

	private void OnEnable()
	{
		UpdateTab();
	}

	public void UpdateTab()
	{
		UpdateButton(juiceBtn, SaveManager.Instance.state.juiceUpgrade, 50, 3f);
		UpdateButton(slowmoBtn, SaveManager.Instance.state.slowmoUpgrade, 50, 3f);
		UpdateButton(comboBtn, SaveManager.Instance.state.comboUpgrade, 150, 3f);
		UpdateButton(speedBtn, SaveManager.Instance.state.speedUpgrade, 50, 3f);
		UpdateButton(missileBtn, SaveManager.Instance.state.missileUpgrade, 500, 3f);
		UpdateButton(splitBtn, SaveManager.Instance.state.splitUpgrade, 500, 3f);
		UpdateButton(bMissileBtn, SaveManager.Instance.state.missileBounceUpgrade, 500, 3f);
		UpdateButton(autoBounceBtn, SaveManager.Instance.state.autoUpgrade, 2000, 3f);
		UpdateButton(cubeBtn, SaveManager.Instance.state.cube);
		UpdateButton(triangleBtn, SaveManager.Instance.state.triangle);
		UpdateButton(spikeBtn, SaveManager.Instance.state.spike);
		UpdateButton(superSpikeBtn, SaveManager.Instance.state.superSpike);
		UpdateButton(explosionBtn, SaveManager.Instance.state.explosionUpgrade, 50000, 1.8f);
		UpdateButton(missileExplosionBtn, SaveManager.Instance.state.missileExplosion);
		UpdateButton(goldBtn, SaveManager.Instance.state.goldMultiplier);
		UpdateButton(loveBtn, SaveManager.Instance.state.love);
		money.text = "$" + SaveManager.Instance.state.money;
	}

	public void UpdateButton(GameObject button, int upgradeAmount, int baseCost, float e)
	{
		string text = CalculateAmount(baseCost, upgradeAmount, e) + "$";
		if (upgradeAmount >= max)
		{
			text = "sold out..";
			button.GetComponent<Image>().color = Colors.fadedColor.Color;
			button.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Colors.fadedColor.Color;
		}
		button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
		if (upgradeAmount > max)
		{
			upgradeAmount = max;
		}
		for (int i = 2; i < 2 + upgradeAmount; i++)
		{
			button.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
		}
	}

	public void UpdateButton(GameObject btn, bool bought)
	{
		if (bought)
		{
			SoldOut(btn);
		}
	}

	private int CalculateAmount(int baseCost, int a, float e)
	{
		return (int)((double)baseCost * Math.Pow(e, a));
	}

	public void SendButton(GameObject b)
	{
		DeselectButton();
		buyBtn.color = Color.white;
		currentBtn = b;
		b.GetComponent<Image>().color = Color.gray;
		b.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.gray;
	}

	public void DeselectButton()
	{
		if (currentBtn != null)
		{
			currentBtn.GetComponent<Image>().color = Color.white;
			currentBtn.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
		}
		currentBtn = null;
		currentString = null;
		shoptext.text = "";
		buyBtn.color = Color.gray;
	}

	public void SendString(string n)
	{
		currentString = n;
	}

	public void BuyUpgrade(string n)
	{
		switch (n)
		{
		case "spike":
			BuySpike();
			return;
		case "cube":
			BuyCube();
			return;
		case "triangle":
			BuyTriangle();
			return;
		case "superSpike":
			BuySuperSpike();
			return;
		case "explosiveMissiles":
			BuyMissileExplosion();
			return;
		case "gold":
			BuyGold();
			return;
		case "love":
			BuyLove();
			return;
		}
		if (currentBtn == null)
		{
			return;
		}
		int num = SaveManager.Instance.state.money;
		int upgradeLevel = SaveManager.Instance.state.GetUpgradeLevel(n);
		int baseCost = SaveManager.Instance.state.getBaseCost(n);
		float baseE = SaveManager.Instance.state.getBaseE(n);
		if (upgradeLevel < max)
		{
			int num2 = CalculateAmount(baseCost, upgradeLevel, baseE);
			if (num >= num2)
			{
				SaveManager.Instance.state.BuyUpgrade(n, num2);
				AudioManager.Instance.Play("Buy");
				CameraShake.ShakeOnce(0.2f, 2f);
				UpdateTab();
				UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			}
			else
			{
				BuyDeny();
			}
		}
	}

	public void BuyCube()
	{
		if (SaveManager.Instance.state.cube)
		{
			SaveManager.Instance.state.currentShape = "cube";
			SaveManager.Instance.Save();
			AudioManager.Instance.Play("Buy");
		}
		else if (SaveManager.Instance.state.money >= 100000 && !SaveManager.Instance.state.cube)
		{
			SaveManager.Instance.state.BuyCube();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3.3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(cubeBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	public void BuyTriangle()
	{
		if (SaveManager.Instance.state.triangle)
		{
			SaveManager.Instance.state.currentShape = "triangle";
			SaveManager.Instance.Save();
			AudioManager.Instance.Play("Buy");
		}
		else if (SaveManager.Instance.state.money >= 500000 && !SaveManager.Instance.state.triangle)
		{
			SaveManager.Instance.state.BuyTriangle();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(triangleBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	public void BuySpike()
	{
		if (SaveManager.Instance.state.money >= 50000 && !SaveManager.Instance.state.spike)
		{
			SaveManager.Instance.state.BuySpike();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(spikeBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	public void BuySuperSpike()
	{
		if (SaveManager.Instance.state.money >= 150000 && !SaveManager.Instance.state.superSpike)
		{
			SaveManager.Instance.state.BuySuperSpike();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(superSpikeBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	public void BuyMissileExplosion()
	{
		if (SaveManager.Instance.state.money >= 1000000 && !SaveManager.Instance.state.missileExplosion)
		{
			SaveManager.Instance.state.BuyMissileExplosion();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(missileExplosionBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	public void BuyGold()
	{
		if (SaveManager.Instance.state.money >= 2000000 && !SaveManager.Instance.state.goldMultiplier)
		{
			SaveManager.Instance.state.BuyGold();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(goldBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	public void BuyLove()
	{
		if (SaveManager.Instance.state.money >= 10000000 && !SaveManager.Instance.state.love)
		{
			SaveManager.Instance.state.BuyLove();
			AudioManager.Instance.Play("Buy");
			CameraShake.ShakeOnce(0.4f, 3f);
			UpdateTab();
			UnityEngine.Object.Instantiate(upgradeFx, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			SoldOut(loveBtn);
		}
		else
		{
			BuyDeny();
		}
	}

	private void SoldOut(GameObject btn)
	{
		btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "sold out";
		btn.GetComponent<Image>().color = Colors.fadedColor.Color;
		btn.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Colors.fadedColor.Color;
	}

	private void BuyDeny()
	{
		AudioManager.Instance.Play("Error");
		CameraShake.ShakeOnce(0.5f, 2f);
	}

	public void SetShopText(string s)
	{
		shoptext.text = s;
	}

	public void BuyCurrentItem()
	{
		BuyUpgrade(currentString);
	}
}
