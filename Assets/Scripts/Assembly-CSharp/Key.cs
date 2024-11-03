using Audio;
using UnityEngine;

public class Key : MonoBehaviour
{
	public GameObject scoreFx;

	public int keyNr = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (keyNr == 1)
			{
				SaveManager.Instance.state.key = true;
			}
			if (keyNr == 2)
			{
				SaveManager.Instance.state.key2 = true;
			}
			if (keyNr == 3)
			{
				SaveManager.Instance.state.key3 = true;
			}
			SaveManager.Instance.Save();
			((ScorePop)Object.Instantiate(scoreFx, base.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent(typeof(ScorePop))).SetText("You found a key..");
			AudioManager.Instance.Play("Cash");
			Object.Destroy(base.gameObject);
		}
	}
}
