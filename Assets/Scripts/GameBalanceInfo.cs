using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "BalanceInfo", menuName = "Game/BalanceInfo")]
public class GameBalanceInfo : ScriptableObject
{
	[Range(0, 4)] [SerializeField] int _maximumBlocksInARow = 4;
	[SerializeField] MoveData[] _moveData;

	public MoveData GetMoveData(int move)
	{
		MoveData moveData = _moveData.FirstOrDefault(item => move >= item.MinMove && move <= item.MaxMove);
		return moveData;
	}

	public int GetNumberOfBlocks(MoveData moveData)
	{
		List<int> chancesForNumbers = new List<int>();
		
		for (int i = 1; i < _maximumBlocksInARow + 1; i++)
		{
			int chanceNumber = (int)moveData.NumberOfBlocksChances.Evaluate(i);
			chancesForNumbers.Add(chanceNumber);
		}
		
		int totalChance = 0;
		
		foreach (int chance in chancesForNumbers)
		{
			totalChance += chance;
		}

		float randomValue = Random.Range(0f, totalChance);
		float cumulativeChance = 0f;

		for (int i = 0; i < chancesForNumbers.Count; i++)
		{
			cumulativeChance += chancesForNumbers[i];
			if (randomValue < cumulativeChance)
			{
				return i + 1;
			}
		}

		return 0;
	} 
}

[Serializable]
public struct MoveData
{
	public string MoveName;
	
	public int MinMove;
	public int MaxMove;

	public int MinBlockNumber;
	public int MaxBlockNumber;
	
	public AnimationCurve NumberOfBlocksChances;
}
