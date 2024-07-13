using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
	public ObservableField<EGameState> GameState = new ObservableField<EGameState>(EGameState.GameNormalSpeed);

	public void GameOver()
	{
		GameState.Value = EGameState.GameOver;
	}

	public void SetGameSpeedToFive()
	{
		GameState.Value = EGameState.GameX5Speed;
	}
}