using Player;

namespace Entities
{
	public class SpikeEntity : KillEntity
	{
		public override void Act(PlayerMovement player, bool isPlayer)
		{
			if (isPlayer)
			{
				if (!player.GetSpikeCrusher() && !player.autoBouncing)
				{
					player.KillPlayer();
					return;
				}
				Bounce(player);
				player.Heal(25);
			}
			else
			{
				player.Heal(25);
			}
		}

		public override int GetScore()
		{
			return 400;
		}
	}
}
