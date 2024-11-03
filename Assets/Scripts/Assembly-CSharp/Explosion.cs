using Player;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			PlayerMovement.Instance.HitEntity(other.gameObject, false, false);
		}
		else if (other.gameObject.CompareTag("EnemyMissile"))
		{
			Object.Destroy(other);
		}
	}

	private void Update()
	{
		if (base.transform.parent.childCount == 1)
		{
			Object.Destroy(base.transform.parent.parent.gameObject);
		}
	}
}
