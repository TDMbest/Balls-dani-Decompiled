using Audio;
using Player;
using UnityEngine;

namespace Entities
{
	public class DirectionEnemy : BounceEntity
	{
		public override void Bounce(PlayerMovement player)
		{
			if (!player.ignoreBounce)
			{
				Rigidbody2D rb = player.GetRb();
				rb.velocity = Vector2.zero;
				rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 1500f);
				AudioManager.Instance.Play("DirChange");
			}
		}

		public override int GetScore()
		{
			return 500;
		}
	}
}
