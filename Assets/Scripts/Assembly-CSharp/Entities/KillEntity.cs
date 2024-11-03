using Player;

namespace Entities
{
	public class KillEntity : BounceEntity
	{
		public override void Act(PlayerMovement player, bool isPlayer)
		{
			if (isPlayer && !player.autoBouncing)
			{
				player.KillPlayer();
			}
		}

		public override int GetScore()
		{
			return 1;
		}
	}
}
