using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoSingleton<MarketManager>
{
    Dictionary<CoinName, Coin> DicCoin; //코인이름으로 코인을 저장하는 딕셔너리

    public void MarketInit() //초기화
    {
        DicCoin = CoinManager.Instance.GetCoinDictionary();
    }

    public override void CustomUpdate() //마켓매니저의 커스텀업데이트
    {
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            ChangeMarketInfo(CoinName.NEETCOIN);
            RenderLineGraph(CoinName.NEETCOIN);
        }

        if (Input.GetKeyDown(KeyCode.F1) == true)
        {
            DicCoin[CoinName.NEETCOIN].MarketInfo.CurrentMacroTrend = MacroCoinTrend.TREND_UP_MACRO;
        }
        if (Input.GetKeyDown(KeyCode.F2) == true)
        {
            DicCoin[CoinName.NEETCOIN].MarketInfo.CurrentMacroTrend = MacroCoinTrend.TREND_DOWN_MACRO;
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

        //시세에 따른 변동폭 변동량
        int digit = Util.CalculDigit((int)cmInfo.CurrentPrice);
        int fluc = 0;

        switch (digit)
        {
            case 1:
            case 2:
            case 3:
                fluc = 10;
                break;
            case 4:
                fluc = 100;
                break;
            case 5:
                fluc = 1000;
                break;
            case 6:
                fluc = 10000;
                break;
            case 7:
                fluc = 100000;
                break;
        }
        
        //거시적 시세가 상승세면 변동폭을 올리고, 하락세면 변동폭을 내린다
        switch (cmInfo.CurrentMacroTrend)
        {
            case MacroCoinTrend.TREND_UP_MACRO:
                cmInfo.MaxFlucRange += fluc;
                cmInfo.MinFlucRange += fluc;
                break;
            case MacroCoinTrend.TREND_DOWN_MACRO:
                cmInfo.MaxFlucRange -= fluc;
                cmInfo.MinFlucRange -= fluc;
                break;
            case MacroCoinTrend.TREND_EQUAL_MACRO:
                break;
        }

        //코인의 가격 대입
        cmInfo.PrevPrice = cmInfo.CurrentPrice;
        float setPrice = CalculRandomNoise(name);
        if (setPrice < 10)
        {
            cmInfo.CurrentMacroTrend = MacroCoinTrend.TREND_UP_MACRO;
            setPrice = 10;
        }
        cmInfo.CurrentPrice = setPrice;
        cmInfo.AddPriceList(setPrice);
        Debug.Log(cmInfo.CurrentPrice);

        //이전 시세와의 차를 계산
        float differ = cmInfo.CurrentPrice - cmInfo.PrevPrice;
        cmInfo.DifferPrice = differ;

        //이전 시세와의 변동치가 음수면 트렌드를 하락세, 양수면 상승세로 설정
        if (differ < 0)
            cmInfo.CurrentMicroTrend = MicroCoinTrend.TREND_DOWN_MICRO;
        else if (differ > 0)
            cmInfo.CurrentMicroTrend = MicroCoinTrend.TREND_UP_MICRO;
        else
            cmInfo.CurrentMicroTrend = MicroCoinTrend.TREND_EQUAL_MICRO;
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

        //그래프를 그리기 위한 시드값 변경
        coin.Seed1 += 1;
        coin.Seed2 += 0.2f;

        n1 = Mathf.PerlinNoise(coin.Seed1, 1);
        n2 = Mathf.PerlinNoise(coin.Seed2, 1);

        //두 랜덤값을 더해서 보정해준뒤 정규화
        result = (n1 + n2) / 2;

        //1~0사이인 결과값을 inverselerp로 퍼센테이지로 변경, lerp로 실제값으로 다시 변경
        result = Mathf.Lerp(coin.MarketInfo.MinFlucRange, coin.MarketInfo.MaxFlucRange, Mathf.InverseLerp(0, 1, result));

        return result;
    }

    public void RegistLineGraph(CoinName name, Transform target) //UI정보와 코인매개변수를 받아 그래프를 세팅하는 메서드
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

        target.gameObject.AddComponent<Grapher>();
        Grapher grapher = target.GetComponent<Grapher>();

        grapher.SetGraphTarget(GraphTarget.GRAPH_COIN, DicCoin[name], target);
    }

    public void RenderLineGraph(CoinName name) //그래퍼에게 그래프 값을 갱신하며 렌더를 요청하는 메서드
    {
        Coin coin;

        if (DicCoin.ContainsKey(name) == true)
        {
            coin = DicCoin[name];
        }
        else
        {
            Debug.LogError(name.ToString() + " is not exist");
            return;
        }

        Grapher grapher = coin.GraphUI.gameObject.GetComponent<Grapher>();
        grapher.SetDraw = true;
        grapher.RenewValue(coin.MarketInfo, GraphTarget.GRAPH_COIN);
    }

    public bool TradeCoin(Character target, CoinName name, float tradeAmount)
    {
        return false;
    }
}
