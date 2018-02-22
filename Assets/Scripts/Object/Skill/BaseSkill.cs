using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill 
{
    //public float scale { get; set; }
    public Coin targetCoin;

    public void Init(Coin coin)
    {
        targetCoin = coin;
    }

    public virtual float SkillApply(Coin usingCoin, List<Coin> listMyCoin, List<Coin> listTargetCoin) 
    {
        return 1;
    }

}
