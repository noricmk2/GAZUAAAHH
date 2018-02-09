using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSkill : BaseSkill
{
    public CoinName TargetName { get;set; }//카운터칠 코인 결정
    public CounterSkill()
    {
        skillType = SkillType.TYPE_COUNTER;
    }

    public override float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin)
    {

        if(usingCoin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
        {
            foreach(Coin coin in listTargetCoin)
            {
                if (coin.Name == TargetName && coin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
                {
                    coin.SkillAffectPrice = 0; 
                    return usingCoin.MarketInfo.CurrentPrice;
                }
            }
        }

        if (usingCoin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
        {
            foreach (Coin coin in listTargetCoin)
            {
                if (coin.Name == TargetName && coin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
                {
                    coin.SkillAffectPrice = 0;
                    return usingCoin.MarketInfo.CurrentPrice;
                }
            }
        }

        return 0;
    }


}
