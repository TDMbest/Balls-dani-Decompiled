using Audio;
using Game;
using Player;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	private float totalXp;

	private float xpForNextLevel;

	private float xpGained;

	private float startXp;

	private float desiredXp;

	private float time;

	private float speed;

	public GameObject xpBar;

	public GameObject levelFx;

	private Vector2 defaultScale;

	private Vector2 scaleVel;

	private Vector2 scaleSpeed;

	public TextMeshProUGUI text;

	public TextMeshProUGUI levelText;

	public ParticleSystem xpFx;

	public Transform fxPos;

	public Transform middle;

	private int currentLevel;

	public TextMeshProUGUI killsT;

	public TextMeshProUGUI distanceT;

	public TextMeshProUGUI scoreT;

	private int desiredKills;

	private int desiredDist;

	private int desiredSCore;

	private int currentKills;

	private int currentDist;

	private int currentScore;

	private float distVel;

	private float killsVel;

	private float scoreVel;

	private float currentXp;

	private float desiredScaleXp;

	private float xpVel;

	private Vector2 defaultEntireScale;

	private Vector2 totalScaleVel;

	private float t;

	public TextMeshProUGUI moneyText;

	public Vector2 defaultMoneyScale;

	public Vector2 moneyScaleVel;

	public Vector2 moneyScaleMax;

	public GameObject moneyFx;

	public Transform moneyFxPos;

	private Vector2 maxLevelScale;

	private Vector2 defaultLevelScale;

	private Vector2 levelScaleVel;

	public GameObject rewardBtn;

	public TextMeshProUGUI moneyRewardText;

	private bool filler;

	public Transform distMeter;

	private Vector2 desiredDistVector;

	private Vector2 desiredDistVel;

	private int currentMoney;

	public static LevelController Instance { get; set; }

	private void Awake()
	{
		Instance = this;
		defaultScale = new Vector2(2.1375f, 0.1752397f);
		defaultLevelScale = levelText.transform.localScale;
		defaultMoneyScale = moneyText.transform.localScale;
		maxLevelScale = defaultLevelScale * 1.5f;
		moneyScaleMax = defaultMoneyScale * 1.5f;
		defaultEntireScale = base.transform.localScale;
	}

	public void SetXp()
	{
		filler = false;
		totalXp = SaveManager.Instance.state.xp;
		if (totalXp > 60000000f)
		{
			totalXp = 60000000f;
		}
		SaveManager.Instance.state.deaths++;
		currentLevel = GetLevel();
		desiredDist = SpawnEnemies.Instance.GetDistTravelled() / 10;
		desiredKills = PlayerMovement.Instance.GetKills();
		desiredSCore = Score.Instance.GetScore() / 100;
		SaveManager.Instance.state.UpdateEvent(desiredKills);
		EventScript.Instance.UpdateProgress();
		base.transform.localScale = Vector2.zero;
		long num = desiredKills + desiredDist + desiredSCore;
		if (num > 60000000)
		{
			num = 60000000L;
		}
		text.text = "+" + xpGained;
		xpGained = num;
		time = 0.8f;
		startXp = totalXp;
		desiredXp = totalXp + xpGained;
		currentXp = 0f;
		desiredScaleXp = xpGained;
		scoreT.text = "+" + 0;
		killsT.text = "+" + 0;
		distanceT.text = "+" + 0;
		currentDist = 0;
		currentScore = 0;
		currentKills = 0;
		t = 0f;
		xpFx.Stop();
		float barRatio = GetBarRatio();
		xpBar.transform.localScale = new Vector2(defaultScale.x * barRatio, defaultScale.y);
		SaveManager.Instance.state.xp += num;
		SaveManager.Instance.state.kills += PlayerMovement.Instance.GetKills();
		SaveManager.Instance.state.distanceTravelled += SpawnEnemies.Instance.GetDistTravelled();
		SaveManager.Instance.state.totalScore += Score.Instance.GetScore();
		SaveManager.Instance.state.money += (int)xpGained;
		SaveManager.Instance.Save();
		currentMoney = SaveManager.Instance.state.money;
		if (currentMoney > 1000000000)
		{
			currentMoney = 1000000000;
		}
		moneyText.text = "$" + currentMoney;
		levelText.text = "level " + GetLevel();
		rewardBtn.SetActive(true);
		UpdateRewards();
		ShowDistance();
		int levelMoney = GetLevelMoney((int)startXp, (int)xpGained);
		SaveManager.Instance.state.money += levelMoney;
		SaveManager.Instance.Save();
	}

	private void Update()
	{
		distMeter.localPosition = Vector2.SmoothDamp(distMeter.transform.localPosition, desiredDistVector, ref desiredDistVel, 1.6f);
		levelText.transform.localScale = Vector2.SmoothDamp(levelText.transform.localScale, defaultLevelScale, ref levelScaleVel, 0.3f);
		moneyText.transform.localScale = Vector2.SmoothDamp(moneyText.transform.localScale, defaultMoneyScale, ref moneyScaleVel, 0.3f);
		base.transform.localScale = Vector2.SmoothDamp(base.transform.localScale, defaultEntireScale, ref totalScaleVel, 0.3f);
		t += Time.unscaledDeltaTime * 1.5f;
		if (Input.touchCount > 0 && t > 1f)
		{
			t = 5f;
		}
		if (desiredScaleXp - currentXp > 0.5f)
		{
			currentKills = (int)Mathf.Lerp(0f, desiredKills, t - 1f);
			currentDist = (int)Mathf.Lerp(0f, desiredDist, t - 2f);
			currentScore = (int)Mathf.Lerp(0f, desiredSCore, t - 3f);
			currentXp = (int)Mathf.Lerp(0f, desiredScaleXp, t / 3f - 0.33f);
			scoreT.text = "+" + currentScore;
			killsT.text = "+" + currentKills;
			distanceT.text = "+" + currentDist;
			text.text = "+" + (int)Mathf.Floor(currentXp);
			return;
		}
		if (!filler)
		{
			filler = true;
			AudioManager.Instance.Play("Filling");
		}
		if (xpFx.isStopped)
		{
			xpFx.Play();
		}
		if (xpGained - (totalXp - startXp) < 0.5f)
		{
			text.text = "+0";
			xpFx.Stop();
			return;
		}
		if (GetLevel() > currentLevel)
		{
			LevelUp();
		}
		levelText.text = "level " + GetLevel();
		xpFx.transform.position = fxPos.position;
		totalXp = Mathf.SmoothDamp(totalXp, desiredXp, ref speed, time);
		text.text = "+" + (int)Mathf.Floor(xpGained - (totalXp - startXp));
		float barRatio = GetBarRatio();
		xpBar.transform.localScale = new Vector2(defaultScale.x * barRatio, defaultScale.y);
	}

	public int GetLevel()
	{
		float num = 0f;
		for (int i = 1; i < 99; i++)
		{
			num += Mathf.Floor((float)i + 300f * Mathf.Pow(2f, (float)i / 7f));
			if (num >= totalXp)
			{
				return i;
			}
		}
		return 99;
	}

	public int GetLevel(int dXp)
	{
		float num = 0f;
		for (int i = 1; i < 99; i++)
		{
			num += Mathf.Floor((float)i + 300f * Mathf.Pow(2f, (float)i / 7f));
			if (num >= (float)dXp)
			{
				return i;
			}
		}
		return 99;
	}

	public int GetXpForLevel()
	{
		int level = GetLevel();
		if (level >= 99)
		{
			return 0;
		}
		return (int)Mathf.Floor((float)level + 300f * Mathf.Pow(2f, (float)level / 7f));
	}

	public int GetXpOnCurrentLevel()
	{
		float num = 0f;
		for (int i = 1; i < 99; i++)
		{
			num += Mathf.Floor((float)i + 300f * Mathf.Pow(2f, (float)i / 7f));
			if (num > totalXp)
			{
				break;
			}
		}
		return GetXpForLevel() - (int)(num - totalXp);
	}

	public float GetBarRatio()
	{
		if (GetLevel() > 98)
		{
			return 1f;
		}
		return (float)GetXpOnCurrentLevel() / (float)GetXpForLevel();
	}

	public int GetLevelMoney(int myStartXp, int endXp)
	{
		int num = GetLevel(myStartXp) + 1;
		int num2 = GetLevel(myStartXp + endXp) + 1;
		int num3 = 0;
		for (int i = 0; i < num2 - num; i++)
		{
			num3 += (int)((float)((num + i) * 100) + Mathf.Pow(1.11f, num + i) + Mathf.Pow(num + i, 2f) * 30f);
		}
		return num3;
	}

	private void LevelUp()
	{
		currentLevel = GetLevel();
		Object.Instantiate(levelFx, middle.position, Quaternion.identity);
		CameraShake.ShakeOnce(0.4f, 2f);
		AudioManager.Instance.Play("LevelUp");
		levelText.transform.localScale = maxLevelScale;
		int levelMoney = (int)((float)(currentLevel * 100) + Mathf.Pow(1.11f, currentLevel) + Mathf.Pow(currentLevel, 2f) * 30f);
		AddMoney(levelMoney);
	}

	private void AddMoney(int levelMoney)
	{
		currentMoney += levelMoney;
		moneyText.text = "$" + currentMoney;
		moneyText.transform.localScale = moneyScaleMax;
		ScorePop obj = (ScorePop)Object.Instantiate(moneyFx, moneyText.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent(typeof(ScorePop));
		obj.SetText("$" + levelMoney);
		obj.transform.localScale *= 1.5f;
		AudioManager.Instance.Play("Coin2");
	}

	public void SetXp(int xpG, int moneyG)
	{
		MonoBehaviour.print("money before boost: " + SaveManager.Instance.state.money);
		filler = false;
		totalXp = SaveManager.Instance.state.xp;
		currentMoney = SaveManager.Instance.state.money;
		currentLevel = GetLevel();
		text.text = "+" + xpG;
		xpGained = xpG;
		time = 0.8f;
		t = 0f;
		startXp = totalXp;
		desiredXp = totalXp + xpGained;
		desiredDist = 0;
		desiredKills = 0;
		desiredSCore = 0;
		currentXp = 0f;
		desiredScaleXp = xpGained;
		xpFx.Stop();
		float barRatio = GetBarRatio();
		xpBar.transform.localScale = new Vector2(defaultScale.x * barRatio, defaultScale.y);
		SaveManager.Instance.state.xp += xpG;
		SaveManager.Instance.Save();
		levelText.text = "level " + GetLevel();
		rewardBtn.SetActive(false);
		AddMoney(GetCash());
		int levelMoney = GetLevelMoney((int)startXp, (int)xpGained);
		SaveManager.Instance.state.money += levelMoney + GetCash();
		MonoBehaviour.print("money after boost: " + SaveManager.Instance.state.money);
		SaveManager.Instance.Save();
	}

	private void UpdateRewards()
	{
		moneyRewardText.text = "+ $" + GetCash();
	}

	public int GetRewardXp()
	{
		return GetXpForLevel() - GetXpOnCurrentLevel() + 1;
	}

	private void ShowDistance()
	{
		float num = SpawnEnemies.Instance.GetCurrentDistance() / 1200f;
		float x = 70f * num;
		desiredDistVector = new Vector2(x, distMeter.localPosition.y);
		distMeter.localPosition = new Vector2(0f, distMeter.localPosition.y);
	}

	public int GetCash()
	{
		return (int)((float)((currentLevel + 1) * 100) + Mathf.Pow(1.11f, currentLevel + 1) + Mathf.Pow(currentLevel + 1, 2f) * 30f);
	}
}
