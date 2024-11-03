using Audio;
using Game;
using Player;
using UnityEngine;

namespace Entities
{
	public class DeterminedDirEnemy : BounceEntity
	{
		private void Start()
		{
			if (SpawnEnemies.Instance.CubeZone())
			{
				GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.cube;
			}
			else if (SaveManager.Instance.state.love)
			{
				GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.heart;
			}
			base.transform.Rotate(new Vector3(0f, 0f, 1f), Random.Range(0, 180));
			base.transform.Rotate(new Vector3(0f, 0f, 1f), Random.Range(0, 360));
		}

		public override void Bounce(PlayerMovement player)
		{
			if (!player.ignoreBounce)
			{
				Rigidbody2D rb = player.GetRb();
				rb.transform.position = base.transform.position;
				rb.velocity = Vector2.zero;
				rb.AddForce(base.transform.right * 1500f);
				AudioManager.Instance.Play("DirChange");
			}
		}

		public override int GetScore()
		{
			return 600;
		}
	}
}
