using System;
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    [SerializeField] GameStateMachine _gameStateMachine;

    void OnEnable()
    {
        _gameStateMachine.GameState.OnValueChanged += SetGameSpeed;
    }

    void Start()
    {
        SetGameSpeed(EGameState.GameNormalSpeed);
    }

    void OnDisable()
    {
        _gameStateMachine.GameState.OnValueChanged -= SetGameSpeed;
    }

    public void SetGameSpeed(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.GameNormalSpeed :
                Time.timeScale = 1f;
                break;
            case EGameState.GameX5Speed :
                Time.timeScale = 5f;
                break;
            case EGameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }
}
