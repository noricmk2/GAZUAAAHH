using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Start()
    {
        CoinManager.Instance.TestInit();
        MarketManager.Instance.MarketInit();
    }

    private void Update()
    {
        MarketManager.Instance.CustomUpdate();
    }
}
