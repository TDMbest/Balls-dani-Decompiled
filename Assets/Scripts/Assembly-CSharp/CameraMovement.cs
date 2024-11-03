using System;
using Game;
using Player;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform player;

	private float speed = 0.15f;

	private Vector2 velSpeed;

	private float defaultZoom = 4f;

	private float zoomSpeed = 0.3f;

	private float velZoom;

	private float height;

	private float yMin;

	private Transform bossPos;

	private Vector2 startPos;

	private float bossZoom;

	public static CameraMovement Instance { get; set; }

	private void Start()
	{
		Instance = this;
		startPos = base.transform.position;
	}

	private void Update()
	{
		if (GameController.Instance.lobby)
		{
			Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, 6f, ref velZoom, 0.1f);
			base.transform.position = startPos;
			return;
		}
		if (bossPos != null)
		{
			base.transform.position = Vector2.SmoothDamp(base.transform.position, bossPos.position, ref velSpeed, speed);
			Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, bossZoom, ref velZoom, 0.25f);
			return;
		}
		Vector2 zero = Vector2.zero;
		Vector2 vector = PlayerMovement.Instance.GetRb().velocity / 3f;
		vector = new Vector2(vector.x, vector.y / 1.25f);
		if (Input.GetMouseButton(0) && PlayerMovement.Instance.readyToJump && GameController.Instance.playing && Time.timeScale < 1f)
		{
			Vector2 launchDirection = PlayerMovement.Instance.GetLaunchDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			new Vector2(launchDirection.x / 1.3f, launchDirection.y / 2f);
		}
		Vector2 target = (Vector2)player.position + vector;
		base.transform.position = Vector2.SmoothDamp(base.transform.position, target, ref velSpeed, speed);
		float num = Vector2.Distance(base.transform.position, player.transform.position) * 3f;
		float num2 = Math.Abs(player.transform.position.y / 8f);
		if (num2 > 4f)
		{
			num2 = 4f;
		}
		float num3 = defaultZoom + num + num2;
		if (num3 > 12f)
		{
			num3 = 12f;
		}
		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, num3, ref velZoom, zoomSpeed);
		if (base.transform.position.y < yMin && player.transform.position.y > -20f)
		{
			base.transform.position = new Vector3(base.transform.position.x, yMin, base.transform.position.z);
		}
	}

	public void SetPosition(Transform pos, float zoom)
	{
		bossPos = pos;
		bossZoom = zoom;
	}

	public void ResetGame()
	{
		base.transform.position = Vector2.zero;
		bossPos = null;
	}

	public Vector2 GetScorePos()
	{
		return new Vector2(base.transform.position.x, base.transform.position.y + Camera.main.orthographicSize);
	}

	public void StartPos()
	{
		base.transform.position = startPos;
	}
}
