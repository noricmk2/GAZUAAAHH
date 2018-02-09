using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenceSkill : BaseSkill
{
    public DeffenceSkill()
    {
        skillType = SkillType.TYPE_DEFFENCE;
    }

    public override float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin)
    {
        if(usingCoin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
            return usingCoin.MarketInfo.CurrentPrice;

        return 0;
    }

}
