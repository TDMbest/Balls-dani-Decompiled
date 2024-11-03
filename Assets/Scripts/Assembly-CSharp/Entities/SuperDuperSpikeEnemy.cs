using Player;

namespace Entities
{
	public class SuperDuperSpikeEnemy : KillEntity
	{
		public override void Act(PlayerMovement player, bool isPlayer)
		{
			if (isPlayer)
			{
				player.KillPlayer();
			}
			else
			{
				player.Heal(25);
			}
		}

		public override int GetScore()
		{
			return 2500;
		}
	}
}
