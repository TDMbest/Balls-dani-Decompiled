using TMPro;
using UnityEngine;

public class ScorePop : MonoBehaviour
{
	public Transform score;

	public TextMeshProUGUI textMesh;

	public GameObject parent;

	private Vector3 desiredPosition;

	private Vector3 velPos;

	private Vector3 velScale;

	private float velAlpha;

	private Vector3 desiredScale;

	private float moveSpeed = 1f;

	private float scaleSpeed = 1f;

	private float fadeSpeed = 0.4f;

	private void Start()
	{
		desiredPosition = base.transform.position + new Vector3(0f, 2f);
		desiredScale = score.transform.localScale / 1.5f;
		Invoke("DestroySelf", 2.5f);
	}

	private void Update()
	{
		score.position = Vector3.SmoothDamp(score.position, desiredPosition, ref velPos, moveSpeed);
		score.localScale = Vector3.SmoothDamp(score.localScale, desiredScale, ref velScale, scaleSpeed);
		textMesh.CrossFadeAlpha(0f, 1f, false);
	}

	private void DestroySelf()
	{
		Object.Destroy(parent);
	}

	public void SetText(string s)
	{
		textMesh.text = s;
	}
}
