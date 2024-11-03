using UnityEngine;

public class CollectEntities : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag("NoCollect"))
		{
			Object.Destroy(other.gameObject);
		}
	}
}
