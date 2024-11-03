using UnityEngine;

public class Move : MonoBehaviour
{
	public float dx;

	public float dy;

	public float speedX;

	public float speedY;

	public Vector2 offset;

	private Vector2 startPos;

	private void Start()
	{
		startPos = base.transform.position;
	}

	private void Update()
	{
		float x = startPos.x + Mathf.PingPong(offset.x + Time.time * speedX, dx);
		float y = startPos.y + Mathf.PingPong(offset.y + Time.time * speedY, dy);
		base.transform.position = new Vector3(x, y);
	}
}
