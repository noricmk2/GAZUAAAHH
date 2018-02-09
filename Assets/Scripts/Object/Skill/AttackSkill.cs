using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : BaseSkill
{
    public AttackSkill()
    {
        skillType = SkillType.TYPE_ATTACK;
    }

    public override float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin)
    {
        if (usingCoin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
            return usingCoin.MarketInfo.CurrentPrice;

        return 0;
    }
}
