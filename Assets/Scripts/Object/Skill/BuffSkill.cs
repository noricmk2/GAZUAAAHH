using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : BaseSkill
{
    public BuffSkill()
    {
        skillType = SkillType.TYPE_BUFF;
    }

    public override float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin)
    {
        foreach(Coin coin in listMyCoin)
        {
            coin.SkillAffectPrice *= 1.2f;
        }

        return 0;
    }
}
