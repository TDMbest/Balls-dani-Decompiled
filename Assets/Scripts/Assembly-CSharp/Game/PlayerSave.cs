using System;
using UnityEngine;

namespace Game
{
	public class PlayerSave
	{
		public long highScore;

		public int money;

		public float xp;

		public int juice = 15;

		public Color currentColor = new Color(0.317f, 0.761f, 1f, 1f);

		public int deaths;

		public int sensitivity = 2;

		public int difficulty = 1;

		public int juiceUpgrade;

		public int slowmoUpgrade;

		public int comboUpgrade;

		public int speedUpgrade;

		public int missileUpgrade;

		public int missileBounceUpgrade;

		public int splitUpgrade;

		public int autoUpgrade;

		public int explosionUpgrade;

		public bool missileExplosion;

		public bool goldMultiplier;

		public bool love;

		public string currentShape = "ball";

		public int currentShapeInt;

		public bool cube;

		public bool triangle;

		public bool pentagon;

		public bool heart;

		public bool blob;

		public bool spike;

		public bool superSpike;

		public bool key;

		public bool key2;

		public bool key3;

		public bool gravity;

		public bool performance = true;

		public bool startedSaveFile;

		public bool red = true;

		public bool green = true;

		public bool blue = true;

		public bool yellow = true;

		public bool pink = true;

		public bool orange = true;

		public bool face;

		public int currentFace;

		public int kills;

		public int distanceTravelled;

		public int totalScore;

		public bool pull = true;

		public int currentEventIteration;

		public int eventIteration;

		public int eventKills;

		public int desiredEventKills = 1500;

		public int GetLevel()
		{
			float num = 0f;
			for (int i = 1; i < 99; i++)
			{
				num += Mathf.Floor((float)i + 300f * Mathf.Pow(2f, (float)i / 7f));
				if (num > xp)
				{
					return i;
				}
			}
			return 99;
		}

		public float GetSensitivity()
		{
			int num = Math.Abs(2 - sensitivity);
			float num2 = 1f;
			for (int i = 0; i < num; i++)
			{
				num2 = ((sensitivity <= 2) ? (num2 * 1.5f) : (num2 * 0.5f));
			}
			return num2;
		}

		public float GetDifficultyScale()
		{
			return 0.5f + (float)difficulty * 0.5f;
		}

		public Color GetColor()
		{
			return currentColor;
		}

		public int GetJuice()
		{
			return juice + 15 * juiceUpgrade;
		}

		public int GetMaxCombo()
		{
			return 2 + comboUpgrade * 2;
		}

		public float GetLaunchSpeed()
		{
			return 12 + speedUpgrade * 2;
		}

		public float GetSlowmoSpeed()
		{
			if (slowmoUpgrade > 6)
			{
				return 0.05f - ((float)slowmoUpgrade - 6f) / 200f;
			}
			return 0.35f - (float)slowmoUpgrade / 20f;
		}

		public Sprite GetShape()
		{
			return SpriteManager.Instance.GetShapeSprite(currentShapeInt);
		}

		public int GetMissileThreshold()
		{
			if (missileUpgrade < 1)
			{
				return 999999;
			}
			return 1300 / missileUpgrade;
		}

		public float GetSplitChance()
		{
			return (float)splitUpgrade / 100f * 2f;
		}

		public float GetMissileChance()
		{
			return (float)missileBounceUpgrade / 100f * 1.5f;
		}

		public float GetAutoUpgrade()
		{
			return 2f + (float)autoUpgrade * 0.6f;
		}

		public float GetExplosionChance()
		{
			return (float)explosionUpgrade / 100f * 4f;
		}

		public float GetExplosionSize()
		{
			return 1f + (float)explosionUpgrade / 9f;
		}

		public float GetGoldMultiplier()
		{
			if (goldMultiplier)
			{
				return 3f;
			}
			return 1f;
		}

		public int GetUpgradeLevel(string n)
		{
			switch (n)
			{
			case "juice":
				return juiceUpgrade;
			case "slowmo":
				return slowmoUpgrade;
			case "combo":
				return comboUpgrade;
			case "speed":
				return speedUpgrade;
			case "missile":
				return missileUpgrade;
			case "split":
				return splitUpgrade;
			case "bmissile":
				return missileBounceUpgrade;
			case "autoBounce":
				return autoUpgrade;
			case "explosion":
				return explosionUpgrade;
			default:
				return 0;
			}
		}

		public int getBaseCost(string n)
		{
			switch (n)
			{
			case "juice":
			case "slowmo":
			case "speed":
				return 50;
			case "combo":
				return 150;
			case "missile":
			case "split":
			case "bmissile":
				return 500;
			case "autoBounce":
				return 2000;
			case "explosion":
				return 50000;
			default:
				return 100;
			}
		}

		public float getBaseE(string n)
		{
			switch (n)
			{
			case "juice":
			case "slowmo":
			case "speed":
				return 3f;
			case "combo":
				return 3f;
			case "missile":
			case "split":
			case "bmissile":
				return 3f;
			case "autoBounce":
				return 3f;
			case "explosion":
				return 1.8f;
			default:
				return 3f;
			}
		}

		public void BuyUpgrade(string n, int p)
		{
			SaveManager.Instance.state.money -= p;
			switch (n)
			{
			case "juice":
				juiceUpgrade++;
				break;
			case "slowmo":
				slowmoUpgrade++;
				break;
			case "combo":
				comboUpgrade++;
				break;
			case "speed":
				speedUpgrade++;
				break;
			case "missile":
				missileUpgrade++;
				break;
			case "split":
				splitUpgrade++;
				break;
			case "bmissile":
				missileBounceUpgrade++;
				break;
			case "autoBounce":
				autoUpgrade++;
				break;
			case "explosion":
				explosionUpgrade++;
				break;
			}
			SaveManager.Instance.Save();
		}

		public void BuyCube()
		{
			cube = true;
			currentShape = "cube";
			currentShapeInt = 1;
			SaveManager.Instance.state.money -= 100000;
			SaveManager.Instance.Save();
		}

		public void BuyTriangle()
		{
			currentShapeInt = 2;
			triangle = true;
			currentShape = "triangle";
			SaveManager.Instance.state.money -= 500000;
			SaveManager.Instance.Save();
		}

		public void BuySpike()
		{
			spike = true;
			SaveManager.Instance.state.money -= 50000;
			SaveManager.Instance.Save();
		}

		public void BuySuperSpike()
		{
			superSpike = true;
			SaveManager.Instance.state.money -= 150000;
			SaveManager.Instance.Save();
		}

		public void BuyMissileExplosion()
		{
			missileExplosion = true;
			SaveManager.Instance.state.money -= 1000000;
			SaveManager.Instance.Save();
		}

		public void BuyGold()
		{
			goldMultiplier = true;
			SaveManager.Instance.state.money -= 2000000;
			SaveManager.Instance.Save();
		}

		public void BuyLove()
		{
			love = true;
			heart = true;
			SaveManager.Instance.state.money -= 10000000;
			SaveManager.Instance.Save();
		}

		public bool IsColorUnlocked(string n)
		{
			switch (n)
			{
			case "blue":
				return blue;
			case "green":
				return green;
			case "yellow":
				return yellow;
			case "purple":
				return pink;
			case "red":
				return red;
			case "orange":
				return orange;
			default:
				return true;
			}
		}

		public string GetCurrentShape()
		{
			return currentShape;
		}

		public void UpdateEvent(int kills)
		{
			if (eventKills != desiredEventKills)
			{
				eventKills += kills;
				if (eventKills > desiredEventKills)
				{
					eventKills = desiredEventKills;
					face = true;
				}
			}
		}

		public bool[] GetShapeUnlocks()
		{
			return new bool[6] { true, cube, triangle, pentagon, heart, blob };
		}

		public bool[] GetExtraUnlocks()
		{
			return new bool[6] { true, face, false, false, false, false };
		}
	}
}
