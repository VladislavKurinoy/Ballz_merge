using System;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
	[SerializeField] BinarySaveSystem _saveSystem;
	[SerializeField] GameStateMachine _gameStateMachine;
	[SerializeField] CellsController _cellsController;
	[SerializeField] MatchedCellsChecker _matchedCellsChecker;
	[SerializeField] GameOverChecker _gameOverChecker;
	[SerializeField] GameEvent OnMoveNumberChanged;
	[SerializeField] GameEvent OnBestScoreChanged;
	
	int _moveNumber;
	public int MoveNumber => _moveNumber;

	int _bestScore;

	void OnEnable()
	{
		_gameStateMachine.GameState.OnValueChanged += GameStateChange;
	}

	void GameStateChange(EGameState state)
	{
		if (state == EGameState.GameOver)
		{
			CheckForBestScore();
		}
	}

	void OnDisable()
	{
		_gameStateMachine.GameState.OnValueChanged -= GameStateChange;
	}

	void Start()
	{
		Invoke(nameof(NextMove), .1f);
		_gameStateMachine.GameState.Value = EGameState.GameNormalSpeed;
		_bestScore = _saveSystem.LoadData().IntValue;
		OnBestScoreChanged?.Invoke(_bestScore);
	}

	public void NextMove()
	{
		_moveNumber++;
		OnMoveNumberChanged?.Invoke(_moveNumber);
		
		_cellsController.GenerateLine();
		_cellsController.ShiftGridRows();
		_matchedCellsChecker.DeactivateMatchingCells();
		_gameOverChecker.CheckForGameOver();
	}

	public void CheckForBestScore()
	{
		if (_moveNumber > _bestScore)
		{
			_saveSystem.SaveData(new SaveData
			{
				IntValue = _moveNumber
			});
		}
	}
}
