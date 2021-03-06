﻿using System.Collections;
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

    public string ShortName
    {
        get;set;
    }

    public int CoinAmount //캐릭터가 가진 코인의 총량
    {
        get; set;
    }

    public int CoinAmountInBattle //배틀중에 캐릭터가 가진 코인의 총량
    {
        get; set;
    }

    public float SkillAffectPrice //코인의 스킬에 영향을 받은 가격
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

    public Coin(float price, CoinName name, BaseSkill skill, string shortName)
    {
        _marketInfo = new CoinMarketInfo(price);
        Name = name;
        ShortName = shortName;
        CoinSkill = skill;
        Seed1 = Random.Range(0, 100);
        Seed2 = Seed1;
        BattleType = CoinBattleType.TYPE_NORMAL_COIN;
    }

    public Coin(Coin coin)
    {
        _marketInfo = coin.MarketInfo;
        Name = coin.Name;
        ShortName = coin.ShortName;
        CoinSkill = coin.CoinSkill;
        Seed1 = coin.Seed1;
        Seed2 = coin.Seed2;
        BattleType = CoinBattleType.TYPE_NORMAL_COIN;
    }
}
