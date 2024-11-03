using System.Collections.Generic;
using Audio;
using Entities;
using Game;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		private float constantDamage = 3f;

		private float maxHealth = 100f;

		private float currentHealth;

		private float deafultGravity = 1.5f;

		private bool grounded;

		public bool readyToJump;

		private float jumpForce = 100f;

		private Vector3 startHoldPosition;

		private float maxLaunchSpeed;

		private Rigidbody2D rb;

		public LayerMask whatIsEnemy;

		public LayerMask whatIsGround;

		private float slowmoScale = 0.05f;

		private float velSlowmo;

		private Vector2 currLook;

		private Vector2 dirVel;

		public GameObject sprite;

		public GameObject face;

		private SpriteRenderer spriteObject;

		public SpriteRenderer extraSprite;

		private Vector3 squashScale;

		private Vector3 defaultScale;

		private Vector3 velSquash;

		private Vector3 velSquashConstant;

		public LineRenderer lr;

		private int extra;

		public GameObject hitFx;

		public GameObject killFx;

		public GameObject dust;

		public GameObject jumpFx;

		public GameObject startJumpFx;

		public PostProcessingProfile profile;

		private ChromaticAberrationModel.Settings defaultChromaticSettings;

		private VignetteModel.Settings defaultVignetteSettings;

		private VignetteModel.Settings currentVignetteSettings;

		private float velVignette;

		private float desiredVignette = 0.4f;

		public GameObject scoreFx;

		private Camera gameCamera;

		private int kills;

		public GameObject missile;

		public GameObject explosion;

		private List<GameObject> missiles;

		private int missileCounter;

		private int missileThreshold;

		private bool missileExplosion;

		private float explosionScale = 1f;

		private float splitChance;

		private float missileChance;

		private float explosionChance;

		private bool spikeCrusher;

		private bool superSpikeCrusher;

		private bool readyToAuto = true;

		public bool autoBouncing;

		public GameObject autoBounceBtn;

		private float bounceUpgrade;

		private float autoCooldown = 20f;

		private Color defaultColor;

		private bool closeToGround;

		private bool blackHoleThing;

		private bool invincible;

		public bool ignoreBounce;

		private bool pull;

		private float sensitivity = 1f;

		public static PlayerMovement Instance { get; set; }

		private void Start()
		{
			Instance = this;
			kills = 0;
			pull = false;
			rb = GetComponent<Rigidbody2D>();
			squashScale = new Vector3(0.8f, 0.5f, 1f);
			defaultScale = sprite.transform.localScale;
			currentHealth = maxHealth;
			defaultChromaticSettings = profile.chromaticAberration.settings;
			defaultChromaticSettings.intensity = 0.5f;
			defaultVignetteSettings = profile.vignette.settings;
			defaultVignetteSettings.intensity = 0.25f;
			currentVignetteSettings = profile.vignette.settings;
			gameCamera = Camera.main;
			maxLaunchSpeed = SaveManager.Instance.state.GetLaunchSpeed();
			spriteObject = sprite.GetComponent<SpriteRenderer>();
			defaultColor = SaveManager.Instance.state.GetColor();
			missiles = new List<GameObject>();
			ignoreBounce = false;
		}

		private void LateUpdate()
		{
			SquashPlayer();
			if (autoBouncing)
			{
				float intensity = Mathf.SmoothDamp(currentVignetteSettings.intensity, desiredVignette, ref velVignette, 0.25f);
				currentVignetteSettings.intensity = intensity;
				profile.vignette.settings = currentVignetteSettings;
			}
			if (GameController.Instance.cutscene || autoBouncing || !GameController.Instance.playing)
			{
				return;
			}
			closeToGround = Physics2D.OverlapCircle(base.transform.position, 1.1f, whatIsGround);
			if (closeToGround)
			{
				Heal((int)(100f * Time.deltaTime));
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StartAutoBounce();
			}
			missileCounter++;
			if (missileCounter > missileThreshold)
			{
				if (!GameController.Instance.obstacle)
				{
					SpawnMissile();
				}
				missileCounter = 0;
			}
			if (currentHealth <= 0f)
			{
				if (Time.timeScale < 1f)
				{
					ResetSlowmo();
				}
				return;
			}
			if (!closeToGround)
			{
				DamagePlayer(constantDamage * Time.deltaTime);
			}
			if (readyToJump)
			{
				Vector3 vector = gameCamera.ScreenToWorldPoint(Input.mousePosition);
				if (Input.GetMouseButtonDown(0))
				{
					StartJump(vector);
				}
				if (Input.GetMouseButton(0))
				{
					HoldJump(vector);
				}
				if (Input.GetMouseButtonUp(0))
				{
					ReleaseJump(vector);
				}
			}
		}

		public void StartAutoBounce()
		{
			if (readyToAuto && GameController.Instance.playing)
			{
				AutoButton.Instance.SetActive(false);
				AutoBounce.Instance.Activate(true);
				Invoke("SuperBounce", Time.fixedDeltaTime * 2f);
				Time.timeScale = 1f;
				autoBouncing = true;
				Invoke("StopBounce", bounceUpgrade);
				ChromaticAberrationModel.Settings settings = profile.chromaticAberration.settings;
				settings.intensity = 1f;
				profile.chromaticAberration.settings = settings;
				AudioManager.Instance.SetFreq(600f);
				desiredVignette = 0.5f;
				spriteObject.color = Color.white;
			}
		}

		private void SuperBounce()
		{
			Transform closestTarget = AutoBounce.Instance.GetClosestTarget();
			if (closestTarget == null)
			{
				StopBounce();
				return;
			}
			rb.velocity = Vector3.zero;
			rb.gravityScale = 0f;
			rb.velocity = jumpForce * Time.fixedDeltaTime * 18f * (closestTarget.position - base.transform.position).normalized;
		}

		private void StopBounce()
		{
			CancelInvoke("SuperBounce");
			autoBouncing = false;
			readyToAuto = false;
			rb.gravityScale = 1.5f;
			Invoke("ResetAuto", autoCooldown);
			ResetSlowmo();
			desiredVignette = 0.4f;
			spriteObject.color = defaultColor;
			AutoBounce.Instance.Activate(false);
			Heal(100);
			invincible = true;
			Invoke("ResetInvincibility", 1.5f);
		}

		private void ResetAuto()
		{
			readyToAuto = true;
			AutoButton.Instance.SetActive(true);
		}

		private void SpawnMissile()
		{
			GameObject gameObject = Object.Instantiate(missile, base.transform.position, Quaternion.identity);
			Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
			component.velocity = rb.velocity * 0.8f;
			component.AddForce(new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 250f);
			missiles.Add(gameObject);
		}

		private void SquashPlayer()
		{
			if (!grounded)
			{
				Vector3 vector = rb.velocity;
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				sprite.transform.rotation = Quaternion.AngleAxis(num - 90f, Vector3.forward);
				float num2 = rb.velocity.magnitude;
				if (num2 < 7f)
				{
					num2 = 1f;
				}
				else
				{
					if (num2 > 16f)
					{
						num2 = 16f;
					}
					num2 /= 10f;
				}
				Vector3 target = new Vector3(defaultScale.x / num2, defaultScale.y, defaultScale.z);
				sprite.transform.localScale = Vector3.SmoothDamp(sprite.transform.localScale, target, ref velSquashConstant, 0.2f);
			}
			else
			{
				sprite.transform.localScale = defaultScale;
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				grounded = true;
				readyToJump = true;
				AudioManager.Instance.Play("Hit");
				Object.Instantiate(dust, base.transform.position, Quaternion.identity);
				Heal(30);
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				HitEntity(other.gameObject, true, false);
				if (autoBouncing)
				{
					Invoke("SuperBounce", Time.fixedDeltaTime * 2f);
				}
			}
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				grounded = false;
			}
		}

		private void StartJump(Vector2 mousepos)
		{
			if (!(currentHealth < 0f))
			{
				startHoldPosition = mousepos;
				AudioManager.Instance.Play("slowmo");
				ChromaticAberrationModel.Settings settings = profile.chromaticAberration.settings;
				settings.intensity = 1f;
				profile.chromaticAberration.settings = settings;
				AudioManager.Instance.SetFreq(300f);
			}
		}

		private void HoldJump(Vector2 mousepos)
		{
			if (currentHealth < 0f)
			{
				ReleaseJump(mousepos);
				MonoBehaviour.print("releasing");
				return;
			}
			DamagePlayer(1.5f * Time.unscaledDeltaTime / Time.timeScale);
			Time.timeScale = Mathf.SmoothDamp(Time.timeScale, slowmoScale, ref velSlowmo, 0.1f);
			DrawLine(mousepos);
			sprite.transform.localScale = Vector3.SmoothDamp(sprite.transform.localScale, squashScale, ref velSquash, 0.1f);
			Vector3 vector = mousepos;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			sprite.transform.rotation = Quaternion.Euler(0f, 0f, num + 270f);
			float intensity = Mathf.SmoothDamp(currentVignetteSettings.intensity, desiredVignette, ref velVignette, 0.25f);
			currentVignetteSettings.intensity = intensity;
			profile.vignette.settings = currentVignetteSettings;
		}

		private void ReleaseJump(Vector3 mousePos)
		{
			ResetSlowmo();
			if (!(GetLaunchDirection(mousePos).magnitude < 1f))
			{
				DamagePlayer(15f);
				rb.velocity = Vector3.zero;
				rb.velocity = jumpForce * Time.fixedDeltaTime * GetLaunchDirection(mousePos);
				Object.Instantiate(jumpFx, base.transform.position, base.transform.rotation);
				readyToJump = true;
			}
		}

		public void ResetSlowmo()
		{
			profile.chromaticAberration.settings = defaultChromaticSettings;
			profile.vignette.settings = defaultVignetteSettings;
			currentVignetteSettings.intensity = 0.25f;
			lr.enabled = false;
			AudioManager.Instance.SetFreq(22000f);
			AudioManager.Instance.Play("Jump");
			Time.timeScale = 1f;
		}

		public Vector2 GetLaunchDirection(Vector3 mousePos)
		{
			float num = maxLaunchSpeed;
			if (GameController.Instance.obstacle)
			{
				num = 9f;
			}
			Vector3 vector = (mousePos - base.transform.position) * 1.5f;
			if (Mathf.Abs(vector.x) > num)
			{
				float num2 = num / vector.x;
				float x = num;
				if (vector.x < 0f)
				{
					num2 = 0f - num2;
					x = 0f - num;
				}
				vector = new Vector2(x, vector.y * num2);
			}
			if (Mathf.Abs(vector.y) > num)
			{
				float num3 = num / vector.y;
				float y = num;
				if (vector.y < 0f)
				{
					num3 = 0f - num3;
					y = 0f - num;
				}
				vector = new Vector2(vector.x * num3, y);
			}
			currLook = Vector2.SmoothDamp(currLook, vector, ref dirVel, 0.014f * sensitivity);
			return currLook;
		}

		public void HitEntity(GameObject entity, bool hitPlayer, bool missileHit)
		{
			float num = Vector2.Distance(entity.transform.position, Camera.main.transform.position);
			if (!GameController.Instance.obstacle && Random.Range(0f, 1f) < explosionChance && (hitPlayer || (missileHit && missileExplosion)))
			{
				if (hitPlayer)
				{
					rb.AddForce(Vector2.up * 200f);
				}
				MakeExplosion(entity.transform.position);
			}
			if (Random.Range(0f, 1f) < splitChance)
			{
				SplitEntity(entity);
			}
			if (Random.Range(0f, 1f) < missileChance)
			{
				SpawnMissile();
			}
			kills++;
			IEntity entity2 = (IEntity)entity.GetComponent(typeof(IEntity));
			entity2.Act(this, hitPlayer);
			if (rb.velocity.y < 0f && hitPlayer)
			{
				rb.velocity = new Vector2(rb.velocity.x, (0f - rb.velocity.y) / 2.5f);
			}
			float num2 = entity2.GetScore() * Score.Instance.combo;
			if (SpawnEnemies.Instance.CubeZone())
			{
				num2 *= 2f;
			}
			Score.Instance.AddScore(num2);
			if (num < 25f)
			{
				if (hitPlayer)
				{
					CameraShake.ShakeOnce(0.2f, 4f);
				}
				AudioManager.Instance.PlayHitEnemy();
				((ScorePop)Object.Instantiate(scoreFx, entity.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent(typeof(ScorePop))).SetText(string.Concat(num2));
				Color color = entity2.GetColor();
				if (color != Color.clear)
				{
					Object.Instantiate(hitFx, entity.transform.position, Quaternion.identity).GetComponent<ParticleSystem>().startColor = color;
				}
			}
			if (num2 > 0f)
			{
				Object.Destroy(entity);
			}
		}

		private void SplitEntity(GameObject e)
		{
			if (!e.CompareTag("NoSplit") && !GameController.Instance.obstacle)
			{
				float num = 800f;
				GameObject gameObject = Object.Instantiate(e, e.transform.position + Vector3.right * 3f, Quaternion.identity);
				GameObject gameObject2 = Object.Instantiate(e, e.transform.position - Vector3.right * 3f, Quaternion.identity);
				GameObject gameObject3 = Object.Instantiate(e, e.transform.position + Vector3.up * 3f, Quaternion.identity);
				GameObject gameObject4 = Object.Instantiate(e, e.transform.position - Vector3.up * 3f, Quaternion.identity);
				SpawnEnemies.Instance.entities.Add(gameObject);
				SpawnEnemies.Instance.entities.Add(gameObject2);
				SpawnEnemies.Instance.entities.Add(gameObject3);
				SpawnEnemies.Instance.entities.Add(gameObject4);
				gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * num);
				gameObject2.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * num);
				gameObject3.GetComponent<Rigidbody2D>().AddForce(Vector2.up * num);
				gameObject4.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * num);
			}
		}

		private void MakeExplosion(Vector2 pos)
		{
			Transform child = Object.Instantiate(explosion, pos, Quaternion.identity).transform.GetChild(0);
			for (int i = 0; i < child.childCount; i++)
			{
				child.GetChild(i).transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);
			}
			AudioManager.Instance.Play("Explosion");
		}

		private void DrawLine(Vector3 mousePos)
		{
			lr.enabled = true;
			Vector2 vector = jumpForce * Time.fixedDeltaTime * GetLaunchDirection(mousePos);
			if (!(vector.magnitude < 1f))
			{
				float num = base.transform.position.x;
				float num2 = base.transform.position.y;
				float num3 = vector.y;
				float x = vector.x;
				float num4 = 9.81f * rb.gravityScale;
				int num5 = 1500 / (int)vector.magnitude;
				float num6 = 1f;
				lr.positionCount = num5;
				for (int i = 0; i < num5; i++)
				{
					lr.SetPosition(i, new Vector2(num, num2));
					num3 -= num4 * 0.02f * num6;
					num2 += num3 * 0.02f * num6;
					num += x * 0.02f * num6;
				}
			}
		}

		public void DamagePlayer(float damage)
		{
			currentHealth -= damage;
			if (currentHealth < 0f)
			{
				currentHealth = 0f;
			}
		}

		public void KillPlayer()
		{
			if (!autoBouncing && !invincible)
			{
				currentHealth = 0f;
				CameraShake.ShakeOnce(0.5f, 8f);
				AudioManager.Instance.SetFreq(600f);
				Object.Instantiate(killFx, base.transform.position, Quaternion.identity);
				base.gameObject.SetActive(false);
				Time.timeScale = 1f;
				AudioManager.Instance.Play("KillPlayer");
				GameController.Instance.PlayerDied();
				Score.Instance.CheckHighscore();
			}
		}

		public float GetHealthRatio()
		{
			return currentHealth / maxHealth;
		}

		public void Heal(int h)
		{
			currentHealth += h;
			if (currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
		}

		public void RestartGame()
		{
			base.transform.position = Vector2.zero;
			maxHealth = SaveManager.Instance.state.GetJuice();
			currentHealth = maxHealth;
			rb.velocity = Vector2.zero;
			base.gameObject.SetActive(true);
			profile.chromaticAberration.settings = defaultChromaticSettings;
			profile.vignette.settings = defaultVignetteSettings;
			readyToJump = false;
			kills = 0;
			maxLaunchSpeed = SaveManager.Instance.state.GetLaunchSpeed();
			slowmoScale = SaveManager.Instance.state.GetSlowmoSpeed();
			missileCounter = 0;
			missileThreshold = SaveManager.Instance.state.GetMissileThreshold();
			splitChance = SaveManager.Instance.state.GetSplitChance();
			missileChance = SaveManager.Instance.state.GetMissileChance();
			bounceUpgrade = SaveManager.Instance.state.GetAutoUpgrade();
			explosionChance = SaveManager.Instance.state.GetExplosionChance();
			missileExplosion = SaveManager.Instance.state.missileExplosion;
			explosionScale = SaveManager.Instance.state.GetExplosionSize();
			spikeCrusher = SaveManager.Instance.state.spike;
			superSpikeCrusher = SaveManager.Instance.state.superSpike;
			blackHoleThing = SaveManager.Instance.state.key;
			spriteObject.sprite = SaveManager.Instance.state.GetShape();
			spriteObject.color = SaveManager.Instance.state.GetColor();
			defaultColor = SaveManager.Instance.state.GetColor();
			foreach (GameObject missile in missiles)
			{
				Object.Destroy(missile);
			}
			missiles = new List<GameObject>();
			CancelInvoke();
			AudioManager.Instance.SetFreq(22000f);
			ResetAuto();
			invincible = false;
			pull = SaveManager.Instance.state.pull;
			sensitivity = SaveManager.Instance.state.GetSensitivity();
			extra = SaveManager.Instance.state.currentFace;
			UpdateFace();
		}

		private void UpdateFace()
		{
			extraSprite.sprite = SpriteManager.Instance.GetExtraSprite(extra);
		}

		public int GetKills()
		{
			return kills;
		}

		public Rigidbody2D GetRb()
		{
			return rb;
		}

		public bool GetSpikeCrusher()
		{
			return spikeCrusher;
		}

		public bool GetSuperSpikeCrusher()
		{
			return superSpikeCrusher;
		}

		private void ResetInvincibility()
		{
			invincible = false;
		}

		public void ChangeGravity()
		{
			rb.gravityScale = 0f;
			Invoke("ResetGravity", 15f);
		}

		private void ResetGravity()
		{
			rb.gravityScale = deafultGravity;
		}
	}
}
