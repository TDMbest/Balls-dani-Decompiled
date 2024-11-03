using System;
using System.Globalization;
using Audio;
using Game;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
	private float totalScore;

	private float highScore;

	[NonSerialized]
	public int combo = 1;

	private int maxCombo;

	public GameObject scoreFx;

	public GameObject newScore;

	public TextMeshProUGUI textMeshScore;

	public TextMeshProUGUI textMeshCombo;

	public TextMeshProUGUI textMeshHigh;

	public Transform scoreText;

	public Transform comboText;

	public Transform highText;

	private Vector2 defaultScale;

	private Vector2 maxScale;

	private Vector2 velScale;

	private float scaleSpeed = 0.5f;

	private Vector2 defaultComboScale;

	private Vector2 maxComboScale;

	private Vector2 velComboScale;

	private Vector2 defaultHighScale;

	private Vector2 desiredHighScale;

	private Vector2 velHigh;

	public static Score Instance { get; set; }

	private void Start()
	{
		highScore = SaveManager.Instance.state.highScore;
		Instance = this;
		defaultScale = scoreText.localScale;
		defaultComboScale = comboText.localScale;
		maxComboScale = defaultComboScale * 2.5f;
		maxScale = defaultScale * 2.5f;
		defaultHighScale = highText.localScale;
		desiredHighScale = Vector2.zero;
		highText.localScale *= 0f;
	}

	private void Update()
	{
		scoreText.localScale = Vector2.SmoothDamp(scoreText.localScale, defaultScale, ref velScale, scaleSpeed);
		comboText.localScale = Vector2.SmoothDamp(comboText.localScale, defaultComboScale, ref velComboScale, scaleSpeed);
		highText.localScale = Vector2.SmoothDamp(highText.localScale, desiredHighScale, ref velHigh, 0.25f);
	}

	public void AddScore(float score)
	{
		if (GameController.Instance.playing)
		{
			CancelInvoke();
			combo++;
			if (combo > maxCombo)
			{
				combo = maxCombo;
			}
			textMeshCombo.text = "x" + combo;
			comboText.transform.localScale *= 1.25f;
			if (comboText.localScale.x > maxComboScale.x)
			{
				comboText.localScale = maxComboScale;
			}
			switch (combo)
			{
			case 1:
				AudioManager.Instance.Play("Combo1");
				break;
			case 2:
				AudioManager.Instance.Play("Combo2");
				break;
			case 3:
				AudioManager.Instance.Play("Combo3");
				break;
			case 4:
				AudioManager.Instance.Play("Combo4");
				break;
			case 5:
				AudioManager.Instance.Play("Combo5");
				break;
			}
			scoreText.transform.localScale *= 1.25f;
			if (scoreText.localScale.x > maxScale.x)
			{
				scoreText.localScale = maxScale;
			}
			totalScore += score;
			if (totalScore > 1E+11f)
			{
				totalScore = 1E+11f;
			}
			textMeshScore.text = "SCORE: " + decimal.Parse(string.Concat(totalScore), NumberStyles.Float);
			Invoke("ResetCombo", 1.5f);
		}
	}

	private void ResetCombo()
	{
		combo = 1;
		textMeshCombo.text = "x" + combo;
		comboText.transform.localScale *= 1.5f;
		if (comboText.localScale.x > maxComboScale.x)
		{
			comboText.localScale = maxComboScale;
		}
		AudioManager.Instance.Play("ComboLost");
	}

	public void ResetScore()
	{
		maxCombo = SaveManager.Instance.state.GetMaxCombo();
		totalScore = 0f;
		textMeshScore.text = "Score: " + totalScore;
		CancelInvoke();
		ResetCombo();
		desiredHighScale = Vector2.zero;
	}

	public void CheckHighscore()
	{
		if (totalScore > highScore)
		{
			highScore = totalScore;
			textMeshHigh.text = "Highscore: " + decimal.Parse(string.Concat(highScore), NumberStyles.Float);
			SaveManager.Instance.SetHighscore((int)highScore);
			newScore.SetActive(true);
		}
		else
		{
			textMeshHigh.text = "Highscore: " + decimal.Parse(string.Concat(highScore), NumberStyles.Float);
			newScore.SetActive(false);
		}
		desiredHighScale = defaultHighScale;
	}

	public int GetScore()
	{
		return (int)totalScore;
	}

	public void ButtonSound()
	{
		AudioManager.Instance.PlayButton();
	}
}
