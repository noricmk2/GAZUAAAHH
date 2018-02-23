using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenceSkill : BaseSkill
{
    public override float SkillApply(Coin usingCoin, float point)
    {
        if(usingCoin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
            return 1.2f * point;

        return point;
    }

}
