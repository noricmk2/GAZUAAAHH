using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Init()
    {
        listSelectCoins = new List<Coin>();
        DicCoin = CoinManager.Instance.GetCoinDictionary(); // 임시
        characterType = CharacterType.TYPE_ENEMY1;
        base.Init();
        //마켓 매니저에서 코인을 구매해 보유 코인 세팅
    }

    public override void SelectCoin() //사용할 코인 선택
    {
        cost = BattleManager.Instance.CurrentTurnCost;
        float attackRate = Random.Range(0.2f,0.8f); //공격 비중을 랜덤으로 지정
        float deffenceRate = (1 - attackRate); // 공격 비중에 따라 방어 비중 지정
        float attackCost = cost* attackRate; 
        float deffenceCost = cost * deffenceRate;

        foreach (KeyValuePair<CoinName, Coin> pair in DicCoin)
        {
            Coin coin = pair.Value;
            int aa = Random.Range(0, 5); //코인을 방어에 쓸지 공격에 쓸지 랜덤으로 선택
            if (aa >3)
            {
                coin.BattleType = CoinBattleType.TYPE_ATTACK_COIN;
                if (coin.CoinAmount > attackCost)
                    coin.CoinAmountInBattle = (int)attackCost;
                else
                {
                    coin.CoinAmountInBattle = coin.CoinAmount;
                    attackCost -= coin.CoinAmountInBattle;
                }
            }
            else
            {
                coin.BattleType = CoinBattleType.TYPE_DEFFENCE_COIN;
                if (coin.CoinAmount > deffenceCost)
                    coin.CoinAmountInBattle = (int)deffenceCost;
                else
                {
                    coin.CoinAmountInBattle = coin.CoinAmount;
                    deffenceCost -= coin.CoinAmountInBattle;
                }
            }

            listSelectCoins.Add(coin);
        }

    }
    //코인을 선택했으면 캐릭터 상태를 Battle로 바꿈
}   


