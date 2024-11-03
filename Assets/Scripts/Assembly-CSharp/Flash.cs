using UnityEngine;

public class Flash : MonoBehaviour
{
	public SpriteRenderer sr;

	private void Update()
	{
		float a = Mathf.PingPong(Time.time * 0.5f, 0.3f) + 0.15f;
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
	}
}
