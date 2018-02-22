using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenceSkill : BaseSkill
{
    public override float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin)
    {
        if(usingCoin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
            return 1.2f;

        return 1;
    }

}
