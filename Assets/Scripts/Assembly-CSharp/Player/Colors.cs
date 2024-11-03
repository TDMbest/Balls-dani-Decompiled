using UnityEngine;

namespace Player
{
	public class Colors
	{
		public static readonly Colors blue = new Colors("Blue", new Color(0.317f, 0.761f, 1f, 1f));

		public static readonly Colors green = new Colors("Green", new Color(0.353f, 1f, 0.353f, 1f));

		public static readonly Colors yellow = new Colors("Yellow", new Color(0.949f, 1f, 0.4392f, 1f));

		public static readonly Colors pink = new Colors("Pink", new Color(0.9765f, 0.572f, 1f, 1f));

		public static readonly Colors red = new Colors("Red", new Color(1f, 0.2588f, 0.26666f, 1f));

		public static readonly Colors orange = new Colors("Orange", new Color(1f, 0.5685f, 0.1451f, 1f));

		public static readonly Colors selectedColor = new Colors("Selected", new Color(0.95f, 0.95f, 0.95f, 1f));

		public static readonly Colors fadedColor = new Colors("Faded", new Color(0.3f, 0.3f, 0.3f, 1f));

		public string Name { get; private set; }

		public Color Color { get; private set; }

		public Colors(string name, Color color)
		{
			Name = name;
			Color = color;
		}

		public static Color GetColor(string n)
		{
			switch (n)
			{
			case "blue":
				return blue.Color;
			case "green":
				return green.Color;
			case "yellow":
				return yellow.Color;
			case "purple":
				return pink.Color;
			case "red":
				return red.Color;
			case "orange":
				return orange.Color;
			default:
				return blue.Color;
			}
		}
	}
}
