using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CellsController : MonoBehaviour
{
	[SerializeField] BlockColorsInfo _blockColorsInfo;
	[SerializeField] GameBalance _gameBalance;
	[SerializeField] CellGridGenerator _cellGridGenerator;
	[SerializeField] MoveCounter _moveCounter;
	[SerializeField] MatchedCellsChecker _matchedCellsChecker;
	[SerializeField] GameOverChecker _gameOverChecker;
	Cell[,] _cellsGrid;

	void Start()
	{
		_cellsGrid = _cellGridGenerator.GetCellsGrid();
		for (int i = 0; i < _cellsGrid.GetLength(0); i++)
		{
			for (int j = 0; j < _cellsGrid.GetLength(1); j++)
			{
				_cellsGrid[i, j].OnTryMoveCell += TryToSwipeCells;
			}
		}
	}

	void OnDestroy()
	{
		for (int i = 0; i < _cellsGrid.GetLength(0); i++)
		{
			for (int j = 0; j <  _cellsGrid.GetLength(1); j++)
			{
				_cellsGrid[i, j].OnTryMoveCell -= TryToSwipeCells;
			}
		}
	}

	public void ShiftGridRows()
	{
		int rows = _cellsGrid.GetLength(0);
		int columns = _cellsGrid.GetLength(1);
        
		Cell[] tempRow = new Cell[columns];
		for (int j = 0; j < columns; j++)
		{
			tempRow[j] = _cellsGrid[rows - 1, j];
		}
        
		for (int i = rows - 1; i > 0; i--)
		{
			for (int j = 0; j < columns; j++)
			{
				(_cellsGrid[i, j], _cellsGrid[i - 1, j]) = (_cellsGrid[i - 1, j], _cellsGrid[i, j]);
				(_cellsGrid[i, j].XCoordinate, _cellsGrid[i - 1, j].XCoordinate) = (_cellsGrid[i - 1, j].XCoordinate, _cellsGrid[i, j].XCoordinate);
				(_cellsGrid[i, j].YCoordinate, _cellsGrid[i - 1, j].YCoordinate) = (_cellsGrid[i - 1, j].YCoordinate, _cellsGrid[i, j].YCoordinate);
				(_cellsGrid[i, j].GetCellTransform().localPosition, _cellsGrid[i - 1, j].GetCellTransform().localPosition) = 
					(_cellsGrid[i - 1, j].GetCellTransform().localPosition, _cellsGrid[i, j].GetCellTransform().localPosition);
			}
		}
        
		for (int j = 0; j < columns; j++)
		{
			_cellsGrid[0, j] = tempRow[j];
			_cellsGrid[0, j].XCoordinate = tempRow[j].XCoordinate;
			_cellsGrid[0, j].YCoordinate = tempRow[j].YCoordinate;
			_cellsGrid[0, j].GetCellTransform().localPosition = tempRow[j].GetCellTransform().localPosition;
		}
		
		PrintArray();
	}
	
	public void GenerateLine()
	{
		int gridWidth = _cellsGrid.GetLength(1);
		MoveData moveData = _gameBalance.GetGameBalanceInfo.GetMoveData(_moveCounter.MoveNumber);
		int blocksInLine = _gameBalance.GetGameBalanceInfo.GetNumberOfBlocks(moveData);
		List<int> indexesWithActiveBlock = Utilities.GetRandomIndexes(gridWidth, blocksInLine);
        
		if (indexesWithActiveBlock == null)
		{
			Debug.Log("indexesWithActiveBlock list is null!");
			return;
		}
        
		for (int i = 0; i < gridWidth; i++)
		{
			if (!_cellsGrid[0, i].GetCellGameObject().activeSelf)
			{
				_cellsGrid[0, i].SetBlockActivity(false);
				_cellsGrid[0, i].UnsubscribeFromCollisionAction();
			}
		}
        
		foreach (int index in indexesWithActiveBlock)
		{
			if (!_cellsGrid[0, index].GetCellGameObject().activeSelf)
			{
				_cellsGrid[0, index].SetBlockActivity(true);
				_cellsGrid[0, index].SubscribeToCollisionAction();
				int blockNumber = Random.Range(moveData.MinBlockNumber, moveData.MaxBlockNumber);
				_cellsGrid[0, index].BlockNumber = blockNumber;
				_cellsGrid[0, index].BlockColor = _blockColorsInfo.GetColorByNumber(blockNumber);
			}
		}
	}
	
	void TryToSwipeCells(Cell targetCell, Vector2 direction)
    {
         Cell neighbourCellFromTryingToMovePart = Utilities.GetNeighborCell(_cellsGrid, new Vector2(targetCell.XCoordinate, targetCell.YCoordinate), new Vector2(-direction.y, direction.x));
        
         if (neighbourCellFromTryingToMovePart != null && neighbourCellFromTryingToMovePart.GetCellGameObject().activeSelf == false)
         {
             (_cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate], _cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate]) = 
                 (_cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate], _cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate]);
             
             (_cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate].XCoordinate, _cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate].XCoordinate) = 
                 (_cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate].XCoordinate, _cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate].XCoordinate);
             
             (_cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate].YCoordinate, _cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate].YCoordinate) = 
                 (_cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate].YCoordinate, _cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate].YCoordinate);
             
             (_cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate].GetCellTransform().localPosition, _cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate].GetCellTransform().localPosition) = 
                 (_cellsGrid[neighbourCellFromTryingToMovePart.XCoordinate, neighbourCellFromTryingToMovePart.YCoordinate].GetCellTransform().localPosition, _cellsGrid[targetCell.XCoordinate, targetCell.YCoordinate].GetCellTransform().localPosition);
         }
         
         _matchedCellsChecker.DeactivateMatchingCells();
         _gameOverChecker.CheckForGameOver();
    }

	public void PrintArray()
	{
		Console.Clear();
		int rows = _cellsGrid.GetLength(0);
		int cols = _cellsGrid.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			string row = "";
			for (int j = 0; j < cols; j++)
			{
				row += _cellsGrid[i, j].BlockNumber + " ";
			}
			Debug.Log(row.Trim());
		}
	}
}
