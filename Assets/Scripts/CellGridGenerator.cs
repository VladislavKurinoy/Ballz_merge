using UnityEngine;

public class CellGridGenerator : MonoBehaviour
{
    [SerializeField] GameBalance _gameBalance;
    [SerializeField] CellsController _cellsController;
    
    int _gridRealHeight;
    
    Cell[,] _cellsGrid;
    
    [SerializeField] CellView _cellPrefab;
    [SerializeField] Transform _cellsParent;
    
    void Awake()
    {
        CreateCellsGrid();
    }

    void CreateCellsGrid()
    {
        if (_cellsParent == null)
        {
            Debug.Log("Assign _cellsParent transform for cells!");
            return;
        }
        
        // one for spawn, one for game over
        _gridRealHeight = Constants.GridHeight + 2;
        
        _cellsGrid = new Cell[_gridRealHeight, Constants.GridWidth];
        for (int i = 0; i < _gridRealHeight; i++)
        {
            for (int j = 0; j < Constants.GridWidth; j++)
            {
                GameObject cell = Instantiate(_cellPrefab.gameObject, _cellsParent);
                cell.name = "cell" + i + j;
                cell.transform.localPosition = new Vector3(j * (Constants.CellWidth + Constants.CellOffsetX), -i *(Constants.CellHeight + Constants.CellOffsetY));
                CellView cellView = cell.GetComponent<CellView>();
                CollisionDetector collisionDetector = cell.GetComponent<CollisionDetector>();
                _cellsGrid[i, j] = new Cell(i, j, Color.clear, 0, cellView, collisionDetector);
                _cellsGrid[i, j].SetBlockActivity(false);
            }
        }
    }

    public Cell[,] GetCellsGrid()
    {
        return _cellsGrid;
    }
}
