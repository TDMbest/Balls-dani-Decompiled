using UnityEngine;

public class StartLavaWall : MonoBehaviour
{
	public GameObject lavaWall;

	private bool activated;

	private void Update()
	{
		if (activated)
		{
			CameraShake.ShakeOnce(0.2f, 0.1f);
			lavaWall.transform.position = new Vector3(lavaWall.transform.position.x + Time.deltaTime * 3.5f, lavaWall.transform.position.y);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		CameraShake.ShakeOnce(0.2f, 5f);
		if (other.gameObject.CompareTag("Player"))
		{
			activated = true;
		}
	}
}
