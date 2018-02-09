using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill 
{
    public SkillType skillType { get; set; }
    public float scale { get; set; }

    public virtual float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin) 
    {
        return 0;
    }

}
