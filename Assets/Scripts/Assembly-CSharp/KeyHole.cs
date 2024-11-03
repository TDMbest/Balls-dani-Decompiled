using Audio;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
	public int keyHoleNr = 1;

	public GameObject warp;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (keyHoleNr == 1 && SaveManager.Instance.state.key)
			{
				base.gameObject.SetActive(false);
				AudioManager.Instance.Play("Cash");
			}
			if (keyHoleNr == 2 && SaveManager.Instance.state.key2)
			{
				base.gameObject.SetActive(false);
				AudioManager.Instance.Play("Cash");
			}
			if (keyHoleNr == 3 && SaveManager.Instance.state.key3)
			{
				base.gameObject.SetActive(false);
				AudioManager.Instance.Play("Cash");
			}
		}
	}

	public void Restart()
	{
		base.gameObject.SetActive(true);
		if (warp != null)
		{
			warp.SetActive(true);
		}
	}
}
