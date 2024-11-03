using Player;

namespace Entities
{
	public class HealEntity : BounceEntity, IEntity
	{
		protected override void SpecificAct(PlayerMovement player)
		{
			player.Heal(100);
		}
	}
}
