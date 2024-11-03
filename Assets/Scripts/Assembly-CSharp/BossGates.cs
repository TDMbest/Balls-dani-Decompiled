using Audio;
using Game;
using Player;
using TMPro;
using UnityEngine;

public class BossGates : MonoBehaviour
{
	public GameObject gate1;

	public GameObject gate2;

	private bool triggered;

	public TextMeshProUGUI bosstext;

	public GameObject boss;

	public GameObject currentBoss;

	public Transform spawnPos;

	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!triggered && other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			gate1.SetActive(true);
			gate2.SetActive(true);
			triggered = true;
			SpawnBoss();
		}
	}

	private void SpawnBoss()
	{
		PlayerMovement.Instance.ResetSlowmo();
		AudioManager.Instance.Play("BossSpawn");
		currentBoss = Object.Instantiate(boss, spawnPos.position, Quaternion.identity);
		CameraMovement.Instance.SetPosition(spawnPos, 21f);
		GameController.Instance.cutscene = true;
		CameraShake.ShakeOnce(2f, 7f);
	}

	public void BossFinished()
	{
		gate1.SetActive(false);
		gate2.SetActive(false);
		Invoke("ResetTime", 0.2f);
	}

	private void ResetTime()
	{
		GameController.Instance.cutscene = false;
		Time.timeScale = 1f;
		CameraMovement.Instance.SetPosition(null, 10f);
	}

	public void ResetBoss()
	{
		gate1.SetActive(false);
		gate2.SetActive(false);
		triggered = false;
		bosstext.text = "";
		Object.Destroy(currentBoss);
	}
}
