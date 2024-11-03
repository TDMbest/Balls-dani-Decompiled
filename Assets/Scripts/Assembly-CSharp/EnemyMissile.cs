using System.Collections.Generic;
using Audio;
using Player;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
	private List<Transform> entities;

	private Rigidbody2D rb;

	private Transform target;

	public GameObject explosion;

	private Collider2D myCollider;

	private Collider2D targetCollider;

	private bool ready;

	private void Awake()
	{
		myCollider = GetComponent<Collider2D>();
		rb = GetComponent<Rigidbody2D>();
		entities = new List<Transform>();
		Invoke("GetReady", 0.75f);
		Invoke("DestroySelf", 8f);
		target = PlayerMovement.Instance.transform;
	}

	private void FixedUpdate()
	{
		Vector3 vector = rb.velocity;
		if (ready)
		{
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			base.transform.rotation = Quaternion.AngleAxis(num - 90f, Vector3.forward);
			if (target != null)
			{
				rb.AddForce((target.position - base.transform.position) / 2f * 3f);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			PlayerMovement.Instance.KillPlayer();
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
		{
			if (other.gameObject.CompareTag("Pentagon"))
			{
				FinalBoss.Instance.DamageBoss();
			}
			else
			{
				CircleBoss.Instance.DamageBoss();
			}
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Object.Destroy(other.gameObject);
		}
		if (Vector2.Distance(base.transform.position, Camera.main.transform.position) < 35f)
		{
			Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
			AudioManager.Instance.Play("Rocket");
		}
		Object.Destroy(base.gameObject);
	}

	private void DestroySelf()
	{
		Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
		Object.Destroy(base.gameObject);
	}

	public void SetTarget(Collider2D c)
	{
		if (!(c == null))
		{
			targetCollider = c;
		}
	}

	private void GetReady()
	{
		if (targetCollider == null)
		{
			DestroySelf();
			return;
		}
		ready = true;
		AudioManager.Instance.Play("Hiss");
		Physics2D.IgnoreCollision(targetCollider, myCollider, false);
	}

	public Collider2D GetCollider()
	{
		return myCollider;
	}
}
