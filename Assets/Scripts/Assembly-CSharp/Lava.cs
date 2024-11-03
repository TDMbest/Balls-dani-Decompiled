using Player;
using UnityEngine;

public class Lava : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			PlayerMovement.Instance.KillPlayer();
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Object.Destroy(other.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			PlayerMovement.Instance.KillPlayer();
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Object.Destroy(other.gameObject);
		}
	}
}
