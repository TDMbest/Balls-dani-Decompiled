using UnityEngine;

public class HpBar : MonoBehaviour
{
	public Transform redBar;

	public Transform greenBar;

	private Vector2 defaultScale;

	private float maxHp;

	private void Start()
	{
		defaultScale = greenBar.transform.localScale;
	}

	public void SetStartHP(int hp)
	{
		maxHp = hp;
	}

	public void UpdateHp(int hp)
	{
		if (maxHp != 0f)
		{
			float num = (float)hp / maxHp;
			if (num < 0f)
			{
				num = 0f;
			}
			MonoBehaviour.print("hp: " + hp + ", max: " + maxHp + ", r: " + num);
			greenBar.transform.localScale = defaultScale * num;
		}
	}
}
