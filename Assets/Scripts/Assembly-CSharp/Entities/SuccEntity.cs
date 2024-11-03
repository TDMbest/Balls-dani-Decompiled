using Player;
using UnityEngine;

namespace Entities
{
	public class SuccEntity : BounceEntity, IEntity
	{
		public override void Bounce(PlayerMovement player)
		{
			if (!player.ignoreBounce)
			{
				Rigidbody2D rb = player.GetRb();
				rb.transform.position = base.transform.position;
				rb.velocity = Vector2.zero;
				rb.AddForce(Vector2.down * 1000f);
				player.DamagePlayer(10000f);
			}
		}
	}
}
