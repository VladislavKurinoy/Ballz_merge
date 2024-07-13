using TMPro;
using UnityEngine;

public class GameProgressionView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _moveNumberText;
    [SerializeField] TextMeshProUGUI _bestScoreText;

    public void ChangeMoveNumberText(int move)
    {
        _moveNumberText.text = "Move : " + move;
    }

    public void ChangeBestScoreText(int score)
    {
        _bestScoreText.text = "Best score : " + score;
    }
}
