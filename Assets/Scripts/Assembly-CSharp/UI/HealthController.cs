using Player;
using UnityEngine;

namespace UI
{
	public class HealthController : MonoBehaviour
	{
		public RectTransform green;

		private Vector2 defaultScale;

		private Vector3 velScale;

		private float scaleSpeed = 0.05f;

		public static HealthController Instance { get; set; }

		private void Start()
		{
			Instance = this;
			defaultScale = green.localScale;
		}

		private void Update()
		{
			float healthRatio = PlayerMovement.Instance.GetHealthRatio();
			Vector2 vector = new Vector2(defaultScale.x * healthRatio, defaultScale.y);
			if (green.localScale.x < 0.01f && vector.x < 0.01f)
			{
				green.localScale = new Vector2(0f, defaultScale.y);
			}
			else
			{
				green.localScale = Vector3.SmoothDamp(green.transform.localScale, vector, ref velScale, scaleSpeed * Time.timeScale);
			}
		}
	}
}
