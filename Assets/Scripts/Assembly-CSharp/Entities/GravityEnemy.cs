using Player;

namespace Entities
{
	public class GravityEnemy : BounceEntity
	{
		protected override void SpecificAct(PlayerMovement player)
		{
			PlayerMovement.Instance.ChangeGravity();
		}

		public override int GetScore()
		{
			return 1000;
		}
	}
}
