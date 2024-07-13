using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MatchedCellsChecker : MonoBehaviour
{
    [SerializeField] CellGridGenerator _cellGridGenerator;
    Cell[,] _cellsGrid;

    void Start()
    {
        _cellsGrid = _cellGridGenerator.GetCellsGrid();
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
                        foreach (Cell cell in connectedCells)
                        {
                            cell.GetCellTransform().DOShakePosition(0.5f, new Vector2(0.1f, 0)).onComplete += 
                                () =>
                                {
                                    cell.GetCellGameObject().SetActive(false);
                                    cell.BlockColor = Color.clear;
                                    cell.BlockNumber = 0;
                                };
                        }
        
                        AudioManager.Instance.PlaySound(Constants.BlockDestroySound);
                    }
                }
            }
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
}
