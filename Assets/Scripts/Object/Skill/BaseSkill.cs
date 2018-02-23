using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill 
{
    public SkillType skillType;
    public Coin targetCoin;

    public void Init(Coin coin)
    {
        targetCoin = coin;
    }

    public virtual float SkillApply(Coin usingCoin, float point) 
    {
        return point;
    }

}
