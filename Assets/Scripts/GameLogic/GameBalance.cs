using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBalance : MonoBehaviour
{
    [SerializeField] GameBalanceInfo _gameBalanceInfo;

    public GameBalanceInfo GetGameBalanceInfo => _gameBalanceInfo;
}
