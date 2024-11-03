using System.Collections.Generic;
using Audio;
using Player;
using UnityEngine;

public class Missile : MonoBehaviour
{
	private List<Transform> entities;

	private Rigidbody2D rb;

	private Transform target;

	public GameObject explosion;

	private bool ready;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		entities = new List<Transform>();
		Invoke("GetReady", 0.5f);
		Invoke("DestroySelf", 5f);
		GetComponent<SpriteRenderer>().color = SaveManager.Instance.state.GetColor();
		if (SaveManager.Instance.state.love)
		{
			GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.heart;
		}
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
				rb.velocity = (target.position - base.transform.position).normalized * 15f;
			}
			else
			{
				FindNewTarget();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		int layer = other.gameObject.layer;
		if (layer == LayerMask.NameToLayer("Enemy") || layer == LayerMask.NameToLayer("Boss"))
		{
			entities.Add(other.transform);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			PlayerMovement.Instance.HitEntity(other.gameObject, false, true);
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
		if (Vector2.Distance(base.transform.position, Camera.main.transform.position) < 30f)
		{
			AudioManager.Instance.Play("Rocket");
			Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
		}
		Object.Destroy(base.gameObject);
	}

	private void FindNewTarget()
	{
		if (!(target == null))
		{
			return;
		}
		foreach (Transform entity in entities)
		{
			if (entity != null)
			{
				target = entity;
			}
		}
	}

	private void DestroySelf()
	{
		if (Vector2.Distance(base.transform.position, Camera.main.transform.position) < 25f)
		{
			Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
		}
		Object.Destroy(base.gameObject);
	}

	private void GetReady()
	{
		ready = true;
		AudioManager.Instance.Play("Hiss");
	}
}
