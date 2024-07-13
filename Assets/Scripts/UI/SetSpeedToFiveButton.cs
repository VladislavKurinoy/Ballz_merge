using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSpeedToFiveButton : MonoBehaviour
{
    [SerializeField] Button _setX5SpeedButton;
    [SerializeField] GameEvent OnSetGameSpeedToFive;
    void Start()
    {
        _setX5SpeedButton.onClick.AddListener(() => OnSetGameSpeedToFive?.Invoke());
    }
}
