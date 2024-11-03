using Game;
using Player;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	public Transform desiredPosition;

	public bool obstacleCourse;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			MovePlayer();
		}
	}

	private void MovePlayer()
	{
		PlayerMovement.Instance.GetRb().velocity = Vector2.zero;
		PlayerMovement.Instance.gameObject.transform.position = desiredPosition.position;
		base.gameObject.SetActive(false);
		if (SpawnEnemies.Instance.GetDistTravelled() < 4000 && !obstacleCourse)
		{
			SpawnEnemies.Instance.SetDistTravelled(4000);
		}
		if (obstacleCourse)
		{
			GameController.Instance.obstacle = true;
		}
		else
		{
			GameController.Instance.obstacle = false;
		}
	}
}
