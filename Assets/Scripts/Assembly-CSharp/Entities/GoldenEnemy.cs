using Audio;
using Player;

namespace Entities
{
	public class GoldenEnemy : BounceEntity, IEntity
	{
		public override int GetScore()
		{
			return 2000;
		}

		protected override void SpecificAct(PlayerMovement player)
		{
			AudioManager.Instance.Play("Coin");
		}
	}
}
