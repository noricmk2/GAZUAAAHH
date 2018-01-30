using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoSingleton<MarketManager>
{
    Dictionary<CoinName, Coin> DicCoin; //코인이름으로 코인을 저장하는 딕셔너리
    int graphY = 0;

    public GameObject graphCube;

    public void MarketInit()
    {
        DicCoin = CoinManager.Instance.GetCoinDictionary();
        graphCube = Resources.Load("Prefab/Cube") as GameObject;
    }

    public override void CustomUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            ChangeMarketInfo(CoinName.NEETCOIN);
            RenderGraph(CoinName.NEETCOIN);
        }
    }

    public CoinMarketInfo GetMarketInfo(CoinName name) //이름을 전달하여 코인의 시장정보를 받아오는 메서드
    {
        if (DicCoin.ContainsKey(name) == true)
        {
            return DicCoin[name].MarketInfo;
        }
        else
            return null;
    }

    public void ChangeMarketInfo(CoinName name) //시세변동 메서드
    {
        CoinMarketInfo cmInfo;
        if (DicCoin.ContainsKey(name) == true)
            cmInfo = DicCoin[name].MarketInfo;
        else
        {
            Debug.LogError(name.ToString() + " is not exist");
            return;
        }

        cmInfo.PrevPrice = cmInfo.CurrentPrice;
        cmInfo.CurrentPrice = CalculRandomNoise(CoinName.NEETCOIN);
        Debug.Log(cmInfo.CurrentPrice);

        float differ = cmInfo.CurrentPrice - cmInfo.PrevPrice;
        cmInfo.DifferPrice = differ;

        if (differ < 0)
            cmInfo.CurrentTrend = CoinTrend.TREND_DOWN;
        else if (differ > 0)
            cmInfo.CurrentTrend = CoinTrend.TREND_UP;
        else
            cmInfo.CurrentTrend = CoinTrend.TREND_EQUAL;
    }

    float CalculRandomNoise(CoinName name) //시세의 랜덤값을 계산해주는 메서드
    {
        Coin coin;
        float n1, n2, result;

        if (DicCoin.ContainsKey(name) == true)
        {
            coin = DicCoin[name];
        }
        else
        {
            Debug.LogError(name.ToString() + " is not exist");
            return 0;
        }

        coin.Seed1 += 1;
        coin.Seed2 += 0.1f;

        n1 = Mathf.PerlinNoise(coin.Seed1, 1);
        n2 = Mathf.PerlinNoise(coin.Seed2, 1);

        result = (n1 + n2) / 2;

        result = Mathf.Lerp(0, 10, Mathf.InverseLerp(0, 1, result));

        return result;
    }

    public void RenderGraph(CoinName name) //시세 그래프를 그리는 메서드
    {
        CoinMarketInfo cmInfo;

        if (DicCoin.ContainsKey(name) == true)
        {
            cmInfo = DicCoin[name].MarketInfo;
        }
        else
        {
            Debug.LogError(name.ToString() + " is not exist");
            return;
        }

        Vector3 graphX = new Vector3(++graphY, cmInfo.CurrentPrice, 0);
        Instantiate(graphCube, graphX, Quaternion.identity);
    }
}
