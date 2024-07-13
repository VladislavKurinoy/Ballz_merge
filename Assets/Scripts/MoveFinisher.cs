using UnityEngine;

public class MoveFinisher : MonoBehaviour
{
    [SerializeField] BottomBorder _bottomBorder;
    [SerializeField] MoveCounter _moveCounter;
    [SerializeField] GameObject _ballLauncherGO;
    [SerializeField] BallLauncher _ballLauncher;
    
    void OnEnable()
    {
        if(_bottomBorder)
            _bottomBorder.OnBallCollided += OnMoveEnded;
    }

    void OnDisable()
    {
        if(_bottomBorder)
            _bottomBorder.OnBallCollided -= OnMoveEnded;
    }

    void OnMoveEnded(Vector2 ballPosition)
    {
        if (_ballLauncherGO)
        {
            _ballLauncherGO.transform.position = new Vector3(ballPosition.x, _ballLauncher.transform.position.y, 0);
            _ballLauncherGO.gameObject.SetActive(true);
        }
        
        _ballLauncher.gameObject.SetActive(true);
        _moveCounter.NextMove();
    }
}
