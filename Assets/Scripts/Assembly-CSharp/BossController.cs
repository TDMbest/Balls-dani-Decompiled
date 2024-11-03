using UnityEngine;

public class BossController : MonoBehaviour
{
	public BossGates[] bossGates;

	public static BossController Instance { get; set; }

	private void Start()
	{
		Instance = this;
	}

	public void ResetBosses()
	{
		for (int i = 0; i < bossGates.Length; i++)
		{
			bossGates[i].ResetBoss();
		}
	}

	public void SetDialogue(string s)
	{
		bossGates[0].bosstext.text = s;
	}

	public void BossFinished(int i)
	{
		bossGates[i].BossFinished();
	}
}
