using Player;
using UnityEngine;

namespace Entities
{
	public interface IEntity
	{
		void Act(PlayerMovement player, bool isPlayer);

		Color GetColor();

		int GetScore();
	}
}
