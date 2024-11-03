using UnityEngine;

public class SpriteManager : MonoBehaviour
{
	public Sprite circle;

	public Sprite cube;

	public Sprite triangle;

	public Sprite pentagon;

	public Sprite heart;

	public Sprite blob;

	public Sprite[] extra;

	public Sprite[] shapes;

	public static SpriteManager Instance { get; set; }

	private void Awake()
	{
		Instance = this;
	}

	public Sprite GetExtraSprite(int i)
	{
		return extra[i];
	}

	public Sprite GetShapeSprite(int i)
	{
		return shapes[i];
	}
}
