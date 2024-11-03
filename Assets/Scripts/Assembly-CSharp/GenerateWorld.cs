using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
	public GameObject worldPiece;

	private float distance;

	private void Start()
	{
		for (int i = 0; i < 40; i++)
		{
			distance += Random.Range(20, 60);
			Transform obj = Object.Instantiate(worldPiece, new Vector2(distance, base.transform.position.y), base.transform.rotation).transform;
			obj.localScale *= new Vector2(Random.Range(0.2f, 0.7f), 1f);
		}
		distance = 0f;
		for (int j = 0; j < 40; j++)
		{
			distance -= Random.Range(20, 60);
			Transform obj2 = Object.Instantiate(worldPiece, new Vector2(distance, base.transform.position.y), base.transform.rotation).transform;
			obj2.localScale *= new Vector2(Random.Range(0.2f, 0.7f), 1f);
		}
	}
}
