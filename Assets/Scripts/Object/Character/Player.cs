using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    float _currentProperty; 
    public float CurrentProperty //현재 플레이어의 자산
    {
        get { return _currentProperty; }
        set { _currentProperty = value; }
    }

    public override void Init()
    {
        base.Init();
        characterType = CharacterType.TYPE_PLAYER;
        DicCoin = CoinManager.Instance.GetCoinDictionary(); // 임시
        _currentProperty = 5000000f;//테스트용 자산 초기화
    }  
    public void SetView(bool isView)
    {
        SkinnedMeshRenderer skin = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();        
        skin.enabled = isView;       
    }

    //public override void SelectCoin() //사용할 코인 선택
    //{
    //    float attackRate = Random.Range(0.2f, 0.8f); //공격 비중을 랜덤으로 지정
    //    float deffenceRate = (1 - attackRate); // 공격 비중에 따라 방어 비중 지정
    //    float attackCost = cost * attackRate;
    //    float deffenceCost = cost * deffenceRate;

    //    foreach (KeyValuePair<CoinName, Coin> pair in DicCoin)
    //    {
    //        Coin coin = pair.Value;
    //        int type; //코인을 방어에 쓸지 공격에 쓸지 랜덤으로 선택

    //        if (coin.MarketInfo.CurrentPrice < attackCost && coin.MarketInfo.CurrentPrice < deffenceCost)
    //            type = Random.Range(0, 1);
    //        else if (coin.MarketInfo.CurrentPrice < attackCost)
    //            type = 0;
    //        else if (coin.MarketInfo.CurrentPrice < deffenceCost)
    //            type = 1;
    //        else
    //            continue;

    //        if (type == 0)
    //        {
    //            coin.BattleType = CoinBattleType.TYPE_ATTACK_COIN;
    //            attackCost -= coin.MarketInfo.CurrentPrice;
    //        }
    //        if (type == 1)
    //        {
    //            coin.BattleType = CoinBattleType.TYPE_DEFFENCE_COIN;
    //            deffenceCost -= coin.MarketInfo.CurrentPrice;
    //        }

    //        listSelectCoins.Add(coin);
    //    }

    //    //코인을 선택했으면 캐릭터 상태를 Battle로 바꿈
    //    characterState = CharacterState.TYPE_BATTLE;
    //}
}
