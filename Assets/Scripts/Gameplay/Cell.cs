using System;
using UnityEngine;

public class Cell
{
	int _xCoordinate;

	public int XCoordinate
	{
		get => _xCoordinate;
		set
		{
			_xCoordinate = value;
		}
	}
	
	int _yCoordinate;

	public int YCoordinate
	{
		get => _yCoordinate;
		set
		{
			_yCoordinate = value;
		}
	}
	
	Color _blockColor;

	public Color BlockColor
	{
		get => _blockColor;

		set
		{
			_blockColor = value;
			_cellView.BlockSprite.color = _blockColor;
		}
	}
	
	int _blockNumber;
	public int BlockNumber
	{
		get => _blockNumber;

		set
		{
			_blockNumber = value;
			_cellView.BlockNumberText.text = _blockNumber.ToString();
		}
	}
	
	CellView _cellView;
	
	CollisionDetector _collisionDetector;

	public Action<Cell, Vector2> OnTryMoveCell;
	public Cell(int xCoordinate, int yCoordinate, Color blockColor, int blockNumber, CellView cellView, CollisionDetector collisionDetector)
	{
		_xCoordinate = xCoordinate;
		_yCoordinate = yCoordinate;
		_blockColor = blockColor;
		_blockNumber = blockNumber;
		_cellView = cellView;
		_collisionDetector = collisionDetector;
	}

	public void SubscribeToCollisionAction()
	{
		_collisionDetector.OnTryToMoveBlock += TryMoveBlock;
	}

	public void UnsubscribeFromCollisionAction()
	{
		_collisionDetector.OnTryToMoveBlock -= TryMoveBlock;
	}
	public void SetBlockActivity(bool active)
	{
		_cellView.gameObject.SetActive(active);
	}

	public GameObject GetCellGameObject()
	{
		return _cellView.gameObject;
	}

	public Transform GetCellTransform()
	{
		return _cellView.gameObject.transform;
	}

	public CellView GetCellView()
	{
		return _cellView;
	}

	public CollisionDetector GetCellCollisionDetector()
	{
		return _collisionDetector;
	}

	public void TryMoveBlock(Vector2 moveDirection)
	{
		OnTryMoveCell?.Invoke(this, moveDirection);
	}
}