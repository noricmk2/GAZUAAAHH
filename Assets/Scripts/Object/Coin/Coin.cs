using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin
{
    CoinMarketInfo _marketInfo;
    public Image GraphUI
    {
        get; set;
    }

    public CoinMarketInfo MarketInfo //코인의 시장정보. 가격, 시세차, 시세등의 정보가 들어있음
    {
        get { return _marketInfo; }
    }

    public CoinName Name //코인의 이름
    {
        get; set;
    }

    public int CoinAmount //코인의 총량
    {
        get; set;
    }

    public int SelectAmount //배틀중에 선택한 코인의 양
    {
        get; set;
    }

    public CoinBattleType BattleType //배틀중에 선택한 코인의 전투타입
    {
        get; set;
    }

    public BaseSkill CoinSkill //코인이 가진 개별적인 스킬
    {
        get; set;
    }

    public float Seed1 //시세값 조정을 위한 랜덤시드
    {
        get; set;
    }

    public float Seed2 //시세값 조정을 위한 랜덤시드
    {
        get; set;
    }

    public Coin(float price, CoinName name, BaseSkill skill)
    {
        _marketInfo = new CoinMarketInfo(price);
        Name = name;
        CoinSkill = skill;
        Seed1 = Random.Range(0, 100);
        Seed2 = Seed1;
    }
}
