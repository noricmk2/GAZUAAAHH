using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffSkill : BaseSkill
{
    public DebuffSkill()
    {
        skillType = SkillType.TYPE_DEBUFF;
    }

    public override float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin)
    {
        foreach (Coin coin in listTargetCoin)
        {
            coin.SkillAffectPrice *= 0.8f;
        }

        return 0;
    }


}
