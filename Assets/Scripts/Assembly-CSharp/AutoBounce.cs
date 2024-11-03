using System.Collections.Generic;
using UnityEngine;

public class AutoBounce : MonoBehaviour
{
	public List<GameObject> enemies;

	public static AutoBounce Instance { get; set; }

	private void Awake()
	{
		Instance = this;
		enemies = new List<GameObject>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			enemies.Add(other.gameObject);
		}
	}

	public Transform GetClosestTarget()
	{
		float num = float.PositiveInfinity;
		Transform transform = null;
		foreach (GameObject enemy in enemies)
		{
			if (!(enemy == null))
			{
				float num2 = Vector2.Distance(enemy.transform.position, base.transform.parent.position);
				if (num2 < num)
				{
					num = num2;
					transform = enemy.transform;
				}
			}
		}
		if (transform != null)
		{
			enemies.Remove(transform.gameObject);
		}
		return transform;
	}

	public void Activate(bool b)
	{
		base.gameObject.SetActive(b);
	}
}
