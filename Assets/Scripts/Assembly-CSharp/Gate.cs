using Player;
using UnityEngine;

public class Gate : MonoBehaviour
{
	private SpriteRenderer sprite;

	private BoxCollider2D boxCollider;

	private Color openColor;

	private Color closedColor;

	public string gateKey;

	private void Start()
	{
		openColor = new Color(0f, 1f, 0f, 0.4f);
		closedColor = new Color(1f, 0f, 0f, 0.4f);
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
		boxCollider = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (SaveManager.Instance.state.GetCurrentShape() == gateKey)
			{
				GateChange(true);
			}
			else
			{
				GateChange(false);
			}
		}
	}

	private void GateChange(bool b)
	{
		if (b)
		{
			sprite.color = openColor;
		}
		else
		{
			sprite.color = closedColor;
		}
		Collider2D component = PlayerMovement.Instance.gameObject.GetComponent<Collider2D>();
		Physics2D.IgnoreCollision(boxCollider, component, b);
	}
}
