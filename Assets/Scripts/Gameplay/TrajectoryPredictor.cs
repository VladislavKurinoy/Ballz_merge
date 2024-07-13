using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    [SerializeField] GameStateMachine _gameStateMachine;
    [SerializeField] Transform _gameLauncherObject;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] int _predictedTrajectoryDistance;
    [SerializeField] int _numberOfReflections;
    
    Vector3 _position;
    Vector3 _directionEmission;
    Vector2 _inputDirection;
    bool _loopActive = true;

    void OnEnable()
    {
        _gameStateMachine.GameState.OnValueChanged += ShowTrajectory;
    }

    void OnDisable()
    {
        _gameStateMachine.GameState.OnValueChanged -= ShowTrajectory;
    }

    void ShowTrajectory(EGameState state)
    {
        _lineRenderer.enabled = state != EGameState.GameOver;
    }

    void Update()
    {
        PredictTrajectory();
    }

    void PredictTrajectory()
    {
        _loopActive = true;
        int count = 1;
        _position = _gameLauncherObject.transform.position;
        _directionEmission = _inputDirection - (Vector2)_position;
        _lineRenderer.positionCount = count;
        _lineRenderer.SetPosition(0, _position);
        while (_loopActive)
        {
            RaycastHit2D hit = Physics2D.Raycast(_position, _directionEmission, _predictedTrajectoryDistance);
            if (hit)
            {
                count++;
                _lineRenderer.positionCount = count;
                _directionEmission = Vector3.Reflect(_directionEmission, hit.normal);
                _position = ((Vector2)_directionEmission.normalized + hit.point);
                _lineRenderer.SetPosition(count - 1, hit.point);
            }
            else
            {
                count++;
                _lineRenderer.positionCount = count;
                if (count == _numberOfReflections)
                    _lineRenderer.SetPosition(count - 1, _position + (_directionEmission.normalized * _predictedTrajectoryDistance));
                else
                    _lineRenderer.SetPosition(count - 1, _position + (_directionEmission.normalized));
                _loopActive = false;
            }

            if (count > _numberOfReflections)
            {
                _loopActive = false;
            }
        }
    }

    public void SetEmissionDirection(Vector2 mousePosition)
    {
        _inputDirection = mousePosition;
    }
}
