using UnityEngine;

public class Pulse : MonoBehaviour
{
	private Vector2 velP;

	private Vector2 defaultScale;

	public float spasm = 0.07f;

	public float difference = 1.05f;

	public float speed = 25f;

	public float baseStart = 1f;

	private void Start()
	{
		defaultScale = base.transform.localScale;
	}

	private void Update()
	{
		float num = Mathf.PingPong(Time.time * speed, difference) + baseStart;
		Vector2 target = defaultScale * num;
		base.transform.localScale = Vector2.SmoothDamp(base.transform.localScale, target, ref velP, spasm);
	}
}
