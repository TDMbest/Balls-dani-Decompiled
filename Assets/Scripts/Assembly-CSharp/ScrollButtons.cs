using Game;
using UnityEngine;

public class ScrollButtons : MonoBehaviour
{
	private int index;

	private int max = 2;

	private Vector3 desiredPos;

	private Vector3 posVel;

	private Vector3 defaultPos;

	public GameObject newText;

	private bool started;

	private void Start()
	{
		started = true;
		index = 0;
		desiredPos = base.transform.localPosition;
		defaultPos = base.transform.localPosition;
	}

	private void OnEnable()
	{
		index = 0;
		if (started)
		{
			base.transform.localPosition = defaultPos;
			desiredPos = defaultPos;
		}
	}

	private void Update()
	{
		if (!GameController.Instance.playing)
		{
			Camera.main.orthographicSize = 6f;
			CameraMovement.Instance.StartPos();
			base.transform.localPosition = Vector3.SmoothDamp(base.transform.localPosition, desiredPos, ref posVel, 0.25f);
		}
	}

	public void MoveShop(int dir)
	{
		if ((dir >= 0 || index >= 0) && (dir <= 0 || index < max))
		{
			index += dir;
			MonoBehaviour.print(index);
			if (index == -1)
			{
				newText.SetActive(false);
			}
			desiredPos += new Vector3(-20 * dir, 0f);
		}
	}
}
