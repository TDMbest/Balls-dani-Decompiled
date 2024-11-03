using UnityEngine;

public class Arena : MonoBehaviour
{
	public GameObject arenaEnemies;

	public void StartArena()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		Object.Instantiate(arenaEnemies, base.transform.position, Quaternion.identity).transform.parent = base.transform;
	}
}
