using Game;
using Player;
using UnityEngine;

namespace Entities
{
	public class MissileEntity : BounceEntity
	{
		private Transform target;

		private int tick;

		public GameObject missile;

		public Collider2D myCollider;

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
			if (PlayerMovement.Instance != null)
			{
				target = PlayerMovement.Instance.transform;
			}
		}

		private void Update()
		{
			if (!(target == null) && Vector2.Distance(base.transform.position, target.position) < 15f)
			{
				tick++;
				if (tick % 200 == 75)
				{
					SpawnMissile();
				}
			}
		}

		private void SpawnMissile()
		{
			GameObject obj = Object.Instantiate(missile, base.transform.position, Quaternion.identity);
			Vector2 vector = new Vector2(Random.Range(0, 2) * 2 - 1, Random.Range(0, 2) * 2 - 1);
			obj.GetComponent<Rigidbody2D>().AddForce(vector * 450f);
			EnemyMissile obj2 = (EnemyMissile)obj.GetComponent(typeof(EnemyMissile));
			obj2.SetTarget(myCollider);
			Physics2D.IgnoreCollision(obj2.GetCollider(), myCollider, true);
		}

		public override int GetScore()
		{
			return 1000;
		}
	}
}
