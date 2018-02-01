using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Grapher CoinGraph;

    private void Awake()
    {
        CoinManager.Instance.TestInit();
        MarketManager.Instance.MarketInit();
    }

    private void Update() //모든 커스텀업데이트를 이곳에 돌리는 것으로 컨트롤함
    {
        MarketManager.Instance.CustomUpdate();
    }
}
