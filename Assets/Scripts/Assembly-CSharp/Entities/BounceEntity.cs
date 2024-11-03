using Game;
using Player;
using UnityEngine;

namespace Entities
{
	public class BounceEntity : MonoBehaviour, IEntity
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
		}

		public virtual void Bounce(PlayerMovement player)
		{
			if (!player.ignoreBounce)
			{
				player.GetRb().AddForce(Vector2.up * 500f);
			}
		}

		public virtual void Act(PlayerMovement player, bool isPlayer)
		{
			player.Heal(25);
			if (isPlayer)
			{
				Bounce(player);
			}
			SpecificAct(player);
		}

		protected virtual void SpecificAct(PlayerMovement player)
		{
		}

		public Color GetColor()
		{
			return GetComponent<SpriteRenderer>().color;
		}

		public virtual int GetScore()
		{
			return 100;
		}
	}
}
