using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoSingleton<CoinManager>
{
    Dictionary<CoinName, Coin> DicCoin = new Dictionary<CoinName, Coin>();

    public void TestInit()
    {
        DicCoin[CoinName.NEETCOIN] = new Coin(10000.0f, CoinName.NEETCOIN, new BaseSkill());
        DicCoin[CoinName.ATHURIUM] = new Coin(1000.0f, CoinName.ATHURIUM, new BaseSkill());
    }

    public void CoinInit() //코인 정보 초기화
    {

    }

    public Coin CreateCoin(CoinName name) //이름을 받아오면 외부에서 읽어온 데이터를 토대로 코인 생성
    {
        Coin coin = null;

        //TODO 파싱한 데이터를 토대로 스킬, 가격, 총량 결정
        switch (name)
        {
            case CoinName.NEETCOIN:
                break;
            case CoinName.ATHURIUM:
                break;
            case CoinName.SUFFLE:
                break;
            case CoinName.AOS:
                break;
        }

        return coin;
    }

    public Dictionary<CoinName, Coin> GetCoinDictionary() //코인딕셔너리의 getter
    {
        return DicCoin;
    }
}
