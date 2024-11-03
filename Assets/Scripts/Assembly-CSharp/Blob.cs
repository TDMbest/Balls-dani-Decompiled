using Audio;
using UnityEngine;

public class Blob : MonoBehaviour
{
	public GameObject scoreFx;

	private void OnTriggerEnter2D(Collider2D other)
	{
		SaveManager.Instance.state.blob = true;
		Object.Destroy(base.gameObject);
		((ScorePop)Object.Instantiate(scoreFx, base.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent(typeof(ScorePop))).SetText("You found a new shape!");
		AudioManager.Instance.Play("Cash");
	}
}
