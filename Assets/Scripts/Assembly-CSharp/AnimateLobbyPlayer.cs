using UnityEngine;

public class AnimateLobbyPlayer : MonoBehaviour
{
	private float speed = 2f;

	private Vector2 defaultScale;

	private Vector2 desiredScale;

	private Vector2 squashScale;

	private Vector2 scaleVel;

	private void Start()
	{
		defaultScale = base.transform.localScale;
		squashScale = new Vector2(defaultScale.x, defaultScale.y / 2f);
		desiredScale = squashScale;
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, 1f), -0.06f);
		if (Mathf.Abs(desiredScale.y - base.transform.localScale.y) < 0.01f)
		{
			if (desiredScale.y < 0.3f)
			{
				desiredScale = defaultScale;
			}
			else
			{
				desiredScale = squashScale;
			}
		}
		base.transform.localScale = Vector2.SmoothDamp(base.transform.localScale, desiredScale, ref scaleVel, speed);
	}
}
