using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksController : MonoBehaviour
{
    const int GridWidth = 5;
    const int GridHeight = 7;
    int _gridHeight;
    const float OffsetX = .152f;
    const float OffsetY = .123f;
    Cell[,] _cellsGrid;
    
    [SerializeField] CellView _cellPrefab;
    [SerializeField] Transform _cellsParent;

    int _moveNumber;
    [SerializeField] GameBalanceInfo _gameBalanceInfo;
    [SerializeField] BlockColorsInfo _blockColorsInfo;
    void Start()
    {
        CreateCellsGrid();
        NextMove();
    }

    void CreateCellsGrid()
    {
        // one for spawn, one for game over
        _gridHeight = GridHeight + 2;
        
        _cellsGrid = new Cell[_gridHeight, GridWidth];
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < GridWidth; j++)
            {
                GameObject cell = Instantiate(_cellPrefab.gameObject, _cellsParent);
                cell.name = "cell" + i + j;
                cell.transform.localPosition = new Vector3(j + j * OffsetX, -i - i * OffsetY);
                CellView cellView = cell.GetComponent<CellView>();
                CollisionDetector collisionDetector = cell.GetComponent<CollisionDetector>();
                _cellsGrid[i, j] = new Cell(i, j, Color.clear, 0, cellView, collisionDetector);
                _cellsGrid[i, j].OnTryMoveCell += TryToMoveTheCell;
                _cellsGrid[i, j].SetBlockActivity(false);
            }
        }
    }
    
    public void NextMove()
    {
        _moveNumber++;
        GenerateLine();
        ShiftGridRows();
        DeactivateMatchingCells();
        CheckForGameOver();
    }

    void ShiftGridRows()
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
    }

    void GenerateLine()
    {
        MoveData moveData = _gameBalanceInfo.GetMoveData(_moveNumber);
        int blocksInLine = _gameBalanceInfo.GetNumberOfBlocks(moveData);
        List<int> indexesWithActiveBlock = Utilities.GetRandomIndexes(GridWidth, blocksInLine);
        
        if (indexesWithActiveBlock == null)
        {
            Debug.Log("indexesWithActiveBlock list is null!");
            return;
        }
        
        for (int i = 0; i < GridWidth; i++)
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
    
    void TryToMoveTheCell(Cell targetCell, Vector2 direction)
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
         
         DeactivateMatchingCells();
    }
    
    public void DeactivateMatchingCells()
    {
        int rows = _cellsGrid.GetLength(0);
        int cols = _cellsGrid.GetLength(1);
        bool[,] visited = new bool[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!visited[i, j])
                {
                    List<Cell> connectedCells = GetConnectedCells(i, j, _cellsGrid[i, j].BlockNumber, visited);
                    if (connectedCells.Count >= 2)
                    {
                        StartCoroutine(DisableMatchedCells(connectedCells));
                    }
                }
            }
        }
    }

    IEnumerator DisableMatchedCells(List<Cell> matchedCells)
    {
        yield return new WaitForSeconds(1f);
        
        foreach (Cell cell in matchedCells)
        {
            cell.GetCellGameObject().SetActive(false);
        }
    }

    List<Cell> GetConnectedCells(int startX, int startY, int targetNumber, bool[,] visited)
    {
        int rows = _cellsGrid.GetLength(0);
        int cols = _cellsGrid.GetLength(1);
        List<Cell> connectedCells = new List<Cell>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int x = current.x;
            int y = current.y;

            connectedCells.Add(_cellsGrid[x, y]);

            for (int i = 0; i < 4; i++)
            {
                int newX = x + dx[i];
                int newY = y + dy[i];

                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !visited[newX, newY] && _cellsGrid[newX, newY].GetCellGameObject().activeSelf && _cellsGrid[newX, newY].BlockNumber == targetNumber)
                {
                    queue.Enqueue(new Vector2Int(newX, newY));
                    visited[newX, newY] = true;
                }
            }
        }

        return connectedCells;
    }

    void CheckForGameOver()
    {
        if(IsAnyActiveInLastRow())
            Debug.Log("GameOver");
    }
    
    public bool IsAnyActiveInLastRow()
    {
        int lastRowIndex = _cellsGrid.GetLength(0) - 1;

        for (int j = 0; j < _cellsGrid.GetLength(1); j++)
        {
            if (_cellsGrid[lastRowIndex, j].GetCellGameObject().activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    void OnDestroy()
    {
        for (int i = 0; i < _cellsGrid.GetLength(0); i++)
        {
            for (int j = 0; j <  _cellsGrid.GetLength(1); j++)
            {
                _cellsGrid[i, j].OnTryMoveCell -= TryToMoveTheCell;
            }
        }
    }
}