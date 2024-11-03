using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeTab : MonoBehaviour
{
	public TextMeshProUGUI description;

	public Image blue;

	public Image green;

	public Image red;

	public Image yellow;

	public Image pink;

	public Image orange;

	private bool[] shapeUnlocks;

	private bool[] extraUnlocks;

	private int currentExtra;

	public Image shape;

	public Sprite[] shapes;

	private int currentShape;

	private int currentFace;

	public Image displaySprite;

	public GameObject[] colorButtons;

	public GameObject shapeButtons;

	public GameObject extraButtons;

	public static CustomizeTab Instance { get; private set; }

	private void Start()
	{
		Instance = this;
		UpdateTab();
	}

	private void OnEnable()
	{
		UpdateTab();
	}

	private void UpdateTab()
	{
		blue.color = Colors.blue.Color;
		green.color = Colors.green.Color;
		red.color = Colors.red.Color;
		yellow.color = Colors.yellow.Color;
		pink.color = Colors.pink.Color;
		orange.color = Colors.orange.Color;
		shapeUnlocks = SaveManager.Instance.state.GetShapeUnlocks();
		extraUnlocks = SaveManager.Instance.state.GetExtraUnlocks();
		currentShape = ShapeToInt(SaveManager.Instance.state.currentShape);
		currentExtra = SaveManager.Instance.state.currentFace;
		UpdateButtons();
		UpdateShape();
		UpdateExtra();
	}

	public void UpdateButtons()
	{
		for (int i = 0; i < shapeButtons.transform.childCount; i++)
		{
			if (shapeUnlocks[i])
			{
				shapeButtons.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				shapeButtons.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
			}
		}
		for (int j = 0; j < extraButtons.transform.childCount; j++)
		{
			if (extraUnlocks[j])
			{
				extraButtons.transform.GetChild(j).transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				extraButtons.transform.GetChild(j).transform.GetChild(1).gameObject.SetActive(true);
			}
		}
	}

	public void SelectColor(GameObject n)
	{
		MonoBehaviour.print("color pressed");
		if (SaveManager.Instance.state.IsColorUnlocked(n.name))
		{
			SaveManager.Instance.state.currentColor = Colors.GetColor(n.name);
			shape.color = Colors.GetColor(n.name);
			SaveManager.Instance.Save();
			description.text = "";
			DeselectAll(colorButtons);
			n.transform.GetChild(0).gameObject.GetComponent<Image>().color = Colors.selectedColor.Color;
		}
		else
		{
			description.text = "You need 50 kills to unlock this color..";
		}
	}

	private void DeselectAll(GameObject[] list)
	{
		for (int i = 0; i < list.Length; i++)
		{
			list[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = Colors.fadedColor.Color;
		}
	}

	private void DeselectAll(GameObject list)
	{
		for (int i = 0; i < list.transform.childCount; i++)
		{
			list.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().color = Colors.fadedColor.Color;
		}
	}

	public void ChangeShape(int c)
	{
		if (shapeUnlocks[c])
		{
			currentShape = c;
			UpdateShape();
		}
	}

	public void ChangeExtra(int c)
	{
		if (extraUnlocks[c])
		{
			currentExtra = c;
			UpdateExtra();
		}
	}

	private void UpdateExtra()
	{
		DeselectAll(extraButtons);
		extraButtons.transform.GetChild(currentExtra).GetChild(0).gameObject.GetComponent<Image>().color = Colors.selectedColor.Color;
		displaySprite.sprite = SpriteManager.Instance.GetExtraSprite(currentExtra);
		if (currentExtra == 0)
		{
			displaySprite.enabled = false;
		}
		else
		{
			displaySprite.enabled = true;
		}
		SaveManager.Instance.state.currentFace = currentExtra;
		SaveManager.Instance.Save();
	}

	private void UpdateShape()
	{
		DeselectAll(shapeButtons);
		shapeButtons.transform.GetChild(currentShape).GetChild(0).gameObject.GetComponent<Image>().color = Colors.selectedColor.Color;
		shape.sprite = SpriteManager.Instance.GetShapeSprite(currentShape);
		shape.color = SaveManager.Instance.state.currentColor;
		SaveManager.Instance.state.currentShapeInt = currentShape;
		SaveManager.Instance.state.currentShape = IntToShape(currentShape);
		SaveManager.Instance.Save();
	}

	private string IntToShape(int i)
	{
		switch (i)
		{
		case 0:
			return "ball";
		case 1:
			return "cube";
		case 2:
			return "triangle";
		case 3:
			return "heart";
		case 4:
			return "blob";
		default:
			return "ball";
		}
	}

	public int ShapeToInt(string s)
	{
		switch (s)
		{
		case "ball":
			return 0;
		case "cube":
			return 1;
		case "triangle":
			return 2;
		case "heart":
			return 3;
		case "blob":
			return 4;
		default:
			return 0;
		}
	}
}
