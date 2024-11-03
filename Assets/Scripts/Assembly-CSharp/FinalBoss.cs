using System.Collections.Generic;
using Audio;
using Game;
using Player;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
	private int phase;

	private int tick;

	public float health = 50f;

	private bool dead;

	public HpBar healthBar;

	private GameObject currentKey;

	public GameObject missile;

	public GameObject key;

	public GameObject pentagon;

	public List<GameObject> missiles;

	private List<int> phases;

	private List<int> allPhases = new List<int> { 1, 2, 3 };

	private SpriteRenderer[] sprites;

	public Transform spike;

	private Vector2 defaultSpikeScale;

	private Vector2 desiredSpikeScale;

	private Vector2 scaleVel;

	private Vector2 minSpikeScale;

	private Color currentColor;

	private Color vulnerableColor;

	private Color damageColor;

	private Color hitColor;

	private Color desiredColor;

	private Color missileColor;

	private float rVel;

	private float gVel;

	private float bVel;

	private Rigidbody2D rb;

	private bool damage;

	private Vector2 targetDir;

	private Vector2 desiredScale;

	private Vector2 totalScaleVel;

	public GameObject killFx;

	public GameObject scoreFx;

	private float scaleSpeed;

	public Collider2D bossCollider;

	public int bounceLength = 490;

	public int missileSpeed = 120;

	public int missileLength;

	public int bounceSpeed = 600;

	public int bossNr = 1;

	public LineRenderer lr;

	private Transform spawnPos;

	[TextArea]
	public string text1;

	[TextArea]
	public string text2;

	public static FinalBoss Instance { get; set; }

	private void Start()
	{
		spawnPos = base.transform;
		Instance = this;
		phases = allPhases;
		missiles = new List<GameObject>();
		desiredScale = base.transform.localScale;
		base.transform.localScale = Vector2.zero;
		rb = GetComponent<Rigidbody2D>();
		tick = 0;
		phase = 0;
		scaleSpeed = 0.6f;
		damageColor = Color.red;
		vulnerableColor = new Color(1f, 0.5f, 0.4f, 0.9f);
		missileColor = new Color(1f, 0.5f, 0.65f, 0.9f);
		hitColor = new Color(1f, 0.85f, 0.85f, 0.9f);
		currentColor = damageColor;
		desiredColor = currentColor;
		sprites = GetComponentsInChildren<SpriteRenderer>();
		defaultSpikeScale = spike.localScale;
		minSpikeScale = defaultSpikeScale * 0.6f;
		desiredSpikeScale = minSpikeScale;
		spike.localScale = Vector2.zero;
		currentColor = vulnerableColor;
		ChangeColor(vulnerableColor);
		desiredColor = vulnerableColor;
		health *= SaveManager.Instance.state.GetDifficultyScale();
		MonoBehaviour.print(health);
		healthBar.SetStartHP((int)health);
	}

	private void FixedUpdate()
	{
		if (phase != 1)
		{
			rb.AddForce((spawnPos.position - base.transform.position) * 650f * Time.deltaTime);
		}
		if (phase == 0)
		{
			base.transform.localScale = Vector2.SmoothDamp(base.transform.localScale, desiredScale, ref totalScaleVel, scaleSpeed);
			if (dead && tick == 100)
			{
				return;
			}
			if (tick == 125)
			{
				BossController.Instance.SetDialogue(text1);
				CameraShake.ShakeOnce(0.5f, 15f);
				AudioManager.Instance.Play("Speech");
			}
			if (tick == 250)
			{
				BossController.Instance.SetDialogue(text2);
				CameraShake.ShakeOnce(0.5f, 15f);
				AudioManager.Instance.Play("Speech");
			}
			if (tick == 350)
			{
				NewPhase();
				GameController.Instance.cutscene = false;
				CameraShake.ShakeOnce(0.5f, 15f);
				BossController.Instance.SetDialogue("");
			}
		}
		if (phase == 1)
		{
			if (tick == 1)
			{
				desiredSpikeScale = defaultSpikeScale;
				desiredColor = damageColor;
				currentColor = damageColor;
			}
			if (tick > 40 && tick < 80)
			{
				damage = true;
				lr.startWidth = 10f;
				lr.enabled = true;
				lr.positionCount = 2;
				lr.SetPosition(0, base.transform.position);
				lr.SetPosition(1, PlayerMovement.Instance.gameObject.transform.position);
			}
			if (tick > 80 && tick < 100)
			{
				if (lr.startWidth <= 0f)
				{
					lr.startWidth = 0f;
				}
				else
				{
					lr.startWidth -= 20f * Time.fixedDeltaTime;
				}
			}
			if (tick == 80)
			{
				targetDir = PlayerMovement.Instance.gameObject.transform.position - base.transform.position;
			}
			if (tick == 120)
			{
				Bounce();
				lr.enabled = false;
			}
			if (tick == bounceLength)
			{
				damage = false;
				desiredSpikeScale = minSpikeScale;
				desiredColor = vulnerableColor;
				currentColor = vulnerableColor;
				NewPhase();
				rb.drag = 3f;
			}
		}
		if (phase == 2)
		{
			if (tick == 5)
			{
				desiredColor = vulnerableColor;
				currentColor = vulnerableColor;
			}
			if (tick == 130)
			{
				NewPhase();
			}
		}
		if (phase == 3)
		{
			if (tick == 1)
			{
				desiredColor = missileColor;
				currentColor = missileColor;
				ChangeColor(missileColor);
			}
			if (tick % missileSpeed == 0 && tick > 20 && tick < 250 + missileLength)
			{
				LaunchMissile();
			}
			if (tick == 350 + missileLength)
			{
				NewPhase();
			}
		}
		if (sprites[0].color != hitColor)
		{
			float r = Mathf.SmoothDamp(currentColor.r, desiredColor.r, ref rVel, 1f);
			float g = Mathf.SmoothDamp(currentColor.g, desiredColor.g, ref gVel, 1f);
			float b = Mathf.SmoothDamp(currentColor.b, desiredColor.b, ref bVel, 1f);
			ChangeColor(new Color(r, g, b));
		}
		spike.localScale = Vector2.SmoothDamp(spike.localScale, desiredSpikeScale, ref scaleVel, 0.5f);
		tick++;
	}

	private void LaunchMissile()
	{
		GameObject gameObject = Object.Instantiate(missile, base.transform.position, Quaternion.identity);
		Vector2 vector = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
		gameObject.GetComponent<Rigidbody2D>().AddForce(((Vector2)(PlayerMovement.Instance.transform.position - base.transform.position).normalized + vector) * 550f);
		missiles.Add(gameObject);
		EnemyMissile obj = (EnemyMissile)gameObject.GetComponent(typeof(EnemyMissile));
		obj.SetTarget(bossCollider);
		Physics2D.IgnoreCollision(obj.GetCollider(), bossCollider, true);
	}

	private void NewPhase()
	{
		tick = 0;
		phases = new List<int> { 1, 2, 3 };
		if (phase == 0)
		{
			phases.Remove(2);
		}
		else
		{
			phases.Remove(phase);
		}
		MonoBehaviour.print("made a new list. previous phase was: " + phase + ", new list of phases: ");
		PrintPhases();
		phase = phases[Random.Range(0, phases.Count)];
	}

	public void PrintPhases()
	{
		for (int i = 0; i < phases.Count; i++)
		{
			MonoBehaviour.print(phases[i]);
		}
	}

	private void Bounce()
	{
		rb.AddForce(targetDir.normalized * bounceSpeed, ForceMode2D.Impulse);
		rb.AddTorque(50f);
		rb.drag = 0f;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			switch (phase)
			{
			case 1:
				if (damage)
				{
					PlayerMovement.Instance.KillPlayer();
				}
				break;
			case 2:
				DamageBoss();
				break;
			case 3:
				DamageBoss();
				break;
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && GameController.Instance.playing)
		{
			AudioManager.Instance.Play("Hit");
			CameraShake.ShakeOnce(0.4f, 12f);
		}
	}

	public void DamageBoss()
	{
		if (!dead)
		{
			CancelInvoke("ResetColor");
			ChangeColor(hitColor);
			Invoke("ResetColor", Time.fixedDeltaTime * 2f);
			health -= 1f;
			CameraShake.ShakeOnce(0.3f, 6f);
			AudioManager.Instance.PlayHitEnemy();
			PlayerMovement.Instance.Heal(20);
			if (health <= 0f)
			{
				KillBoss();
				dead = true;
			}
			healthBar.UpdateHp((int)health);
		}
	}

	private void KillBoss()
	{
		BossController.Instance.BossFinished(bossNr);
		Object.Instantiate(killFx, base.transform.position, Quaternion.identity);
		Score.Instance.AddScore(1000000 * bossNr);
		ScorePop obj = (ScorePop)Object.Instantiate(scoreFx, base.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent(typeof(ScorePop));
		obj.SetText(string.Concat(1000000));
		obj.parent.transform.localScale *= 4f;
		Object.Instantiate(key, base.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddTorque(50f);
		Object.Instantiate(pentagon, base.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddTorque(50f);
		desiredScale = Vector2.zero;
		scaleSpeed = 0.3f;
		phase = 0;
		tick = 0;
		rb.velocity = Vector2.zero;
		GameController.Instance.cutscene = true;
		CameraShake.ShakeOnce(1f, 8f);
		CameraMovement.Instance.SetPosition(base.transform, 9f);
		Time.timeScale = 0.05f;
		AudioManager.Instance.Play("BossKill");
		Invoke("ResetTime", 0.01f);
		CameraShake.ShakeOnce(1f, 20f);
		foreach (GameObject missile in missiles)
		{
			Object.Destroy(missile);
		}
		missiles = new List<GameObject>();
		Warp.Instance.OpenHomePortal(base.transform.position);
	}

	private void ResetTime()
	{
	}

	private void ResetColor()
	{
		ChangeColor(currentColor);
	}

	private void ChangeColor(Color c)
	{
		for (int i = 0; i < sprites.Length; i++)
		{
			sprites[i].color = c;
		}
	}

	public Collider2D GetCollider()
	{
		return bossCollider;
	}
}
