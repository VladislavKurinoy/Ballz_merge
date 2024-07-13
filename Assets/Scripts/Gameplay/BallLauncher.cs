using System;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] Transform _ballLauncherObject;
    [SerializeField] GameObject _ball;
    [SerializeField] InputDetector _inputDetector;

    public void MouseButtonPress()
    {
        Vector2 direction = _inputDetector.MousePosition - (Vector2)_ballLauncherObject.transform.position;
        Ball ball = Instantiate(_ball, _ballLauncherObject.transform.position, Quaternion.identity).GetComponent<Ball>();
        ball.Shot(direction.normalized);
        _ballLauncherObject.gameObject.SetActive(false);
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySound(Constants.HitSound);
    }
}
