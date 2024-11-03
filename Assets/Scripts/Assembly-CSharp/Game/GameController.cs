using Player;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

namespace Game
{
	public class GameController : MonoBehaviour
	{
		public bool playing;

		public bool cutscene;

		public bool obstacle;

		public bool lobby = true;

		private int gameCounter;

		public Arena[] arenas;

		public KeyHole[] keyHoles;

		public GameObject score;

		public GameObject health;

		public GameObject startButton;

		public GameObject healthBar;

		public GameObject autoBounceBtn;

		public Image overlay;

		public PostProcessingBehaviour pp;

		public PostProcessingProfile graphics;

		public PostProcessingProfile performance;

		public PostProcessingProfile menu;

		public bool performanceMode;

		public static GameController Instance { get; set; }

		private void Start()
		{
			gameCounter = 0;
			Instance = this;
			pp.profile = menu;
			performanceMode = SaveManager.Instance.state.performance;
			UpdatePP();
			Time.timeScale = 1f;
		}

		public void StartGame()
		{
			if (!playing)
			{
				playing = true;
				RestartGame();
				StartPlaying();
			}
		}

		public void UpdatePP()
		{
			pp.enabled = !performanceMode;
		}

		public void SetPerformanceMode(bool p)
		{
			performanceMode = p;
			UpdatePP();
		}

		public void Performance(bool p)
		{
			if (p)
			{
				performanceMode = true;
			}
			else
			{
				performanceMode = false;
			}
		}

		private void StartPlaying()
		{
			Time.timeScale = 1f;
			health.SetActive(true);
			score.SetActive(true);
			lobby = false;
		}

		public void PlayerDied()
		{
			startButton.SetActive(true);
			LevelController.Instance.SetXp();
			playing = false;
			healthBar.SetActive(false);
			overlay.CrossFadeAlpha(1f, 0.5f, true);
			if (!performanceMode)
			{
				pp.profile = menu;
			}
			autoBounceBtn.SetActive(false);
			IncrementGameCounter();
		}

		public void RestartGame()
		{
			SpawnEnemies.Instance.ResetGame();
			PlayerMovement.Instance.RestartGame();
			CameraMovement.Instance.ResetGame();
			Score.Instance.ResetScore();
			healthBar.SetActive(true);
			overlay.CrossFadeAlpha(0f, 0.5f, true);
			BossController.Instance.ResetBosses();
			if (!performanceMode)
			{
				pp.profile = graphics;
			}
			else
			{
				pp.profile = performance;
			}
			if (SaveManager.Instance.state.autoUpgrade > 0)
			{
				autoBounceBtn.SetActive(true);
			}
			lobby = false;
			cutscene = false;
			ResetKeyHoles();
			ResetArenas();
			Warp.Instance.Restart();
			obstacle = false;
		}

		private void ResetKeyHoles()
		{
			for (int i = 0; i < keyHoles.Length; i++)
			{
				keyHoles[i].Restart();
			}
		}

		private void ResetArenas()
		{
			for (int i = 0; i < arenas.Length; i++)
			{
				arenas[i].StartArena();
			}
		}

		public void EnterLobby()
		{
			lobby = true;
		}

		private void IncrementGameCounter()
		{
			gameCounter++;
			if (gameCounter > 6)
			{
				InitializeAds.Instance.ShowAd();
				gameCounter = 0;
			}
		}

		public void RewardAd()
		{
			InitializeAds.Instance.ShowRewardedVideo();
		}
	}
}
