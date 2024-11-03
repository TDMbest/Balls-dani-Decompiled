using Audio;
using Player;
using UnityEngine;

namespace Entities
{
	public class CashEntity : BounceEntity, IEntity
	{
		public GameObject moneyFx;

		public override int GetScore()
		{
			return 5000;
		}

		protected override void SpecificAct(PlayerMovement player)
		{
			float num = SaveManager.Instance.state.GetLevel();
			int num2 = 0;
			if (num > 15f)
			{
				num2 = (int)(Mathf.Pow(num, 2.8f) * 0.5f);
			}
			int num3 = 200 * (int)num + num2;
			if (num3 > 40000)
			{
				num3 = 40000;
			}
			ScorePop obj = (ScorePop)Object.Instantiate(moneyFx, base.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity).transform.GetChild(0).GetComponent(typeof(ScorePop));
			obj.SetText("$" + num3);
			obj.transform.localScale *= 1.5f;
			AudioManager.Instance.Play("Cash");
			SaveManager.Instance.state.money += num3;
			SaveManager.Instance.Save();
		}
	}
}
