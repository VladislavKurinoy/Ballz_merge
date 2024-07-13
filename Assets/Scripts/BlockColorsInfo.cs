using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockColors", menuName = "BlocksInfo/Colors")]
public class BlockColorsInfo : ScriptableObject
{
	[SerializeField] NumberColor[] _numbersColors;

	public Color GetColorByNumber(int number)
	{
		Color color = _numbersColors.FirstOrDefault(item => item.Number == number)!.Color;
		return color;
	}
}

[Serializable]
public class NumberColor
{
	public int Number;
	public Color Color;
}
