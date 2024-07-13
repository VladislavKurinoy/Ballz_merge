using UnityEngine;
using UnityEngine.UI;

public class TryAgainButton : MonoBehaviour
{
    [SerializeField] Button _tryAgainButton;
    [SerializeField] GameEvent OnTryAgainPressed;
    
    void Start()
    {
        _tryAgainButton.onClick.AddListener(() => OnTryAgainPressed?.Invoke());
    }
}
