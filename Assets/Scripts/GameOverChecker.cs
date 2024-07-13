using UnityEngine;

public class GameOverChecker : MonoBehaviour
{
    [SerializeField] CellGridGenerator _cellGridGenerator;
    [SerializeField] GameEvent _gameOverEvent;
    Cell[,] _cellsGrid;

    void Start()
    {
        _cellsGrid = _cellGridGenerator.GetCellsGrid();
    }

    public void CheckForGameOver()
    {
        if (IsAnyActiveInLastRow())
        {
            _gameOverEvent.Invoke();
        }
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
}
