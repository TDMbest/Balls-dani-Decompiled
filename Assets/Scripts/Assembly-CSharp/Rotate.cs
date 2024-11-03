using UnityEngine;

public class Rotate : MonoBehaviour
{
	public float speed = 0.5f;

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, 1f));
	}
}
