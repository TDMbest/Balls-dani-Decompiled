using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour
{
	private string gameId = "3258040";

	private string videoAd = "video";

	private string rewardAd = "rewardedVideo";

	private bool testMode;

	public static InitializeAds Instance { get; private set; }

	private void Start()
	{
		Instance = this;
		Advertisement.Initialize(gameId, testMode);
	}

	public void ShowAd()
	{
	}

	public void ShowRewardedVideo()
	{
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("Video completed - Offer a reward to the player");
			RewardPlayer();
			break;
		case ShowResult.Skipped:
			Debug.LogWarning("Video was skipped - Do NOT reward the player");
			break;
		case ShowResult.Failed:
			Debug.LogError("Video failed to show");
			break;
		}
	}

	private void RewardPlayer()
	{
		int rewardXp = LevelController.Instance.GetRewardXp();
		int cash = LevelController.Instance.GetCash();
		LevelController.Instance.SetXp(rewardXp, cash);
	}
}
