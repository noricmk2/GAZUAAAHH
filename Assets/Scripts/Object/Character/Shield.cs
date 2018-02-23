using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseObject
{
    public BaseEffect shieldEffect { get; set; }

    public void CreateShield(float playTime)
    {
        shieldEffect = EffectManager.Instance.CreateEffect(transform.position, playTime, ConstValue.ShiledEffect);
    }

    public void BreakShield()
    {
        EffectManager.Instance.CreateEffect(transform.position, 1, ConstValue.ShiledBreak);
        EffectManager.Instance.DestroyEffect(shieldEffect);
    }


}
