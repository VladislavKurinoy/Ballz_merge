using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] Transform _ballLauncherObject;
    [SerializeField] GameObject _ball;
    [SerializeField] InputDetector _inputDetector;
    Vector2 _launchDirection;
    public void MouseButtonPress()
    {
        if (!_ballLauncherObject)
            return;

        Vector2 direction = _launchDirection - (Vector2)_ballLauncherObject.transform.position;
        Ball ball = Instantiate(_ball, _ballLauncherObject.transform.position, Quaternion.identity).GetComponent<Ball>();
        ball.Shot(direction.normalized);
        _ballLauncherObject.gameObject.SetActive(false);
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySound(Constants.HitSound);
    }

    public void SetDirection(Vector2 direction)
    {
        _launchDirection = direction;
    }
}
