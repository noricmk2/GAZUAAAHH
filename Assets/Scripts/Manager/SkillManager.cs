using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager> {

    public BaseSkill SkillInit(SkillType skillType)
    {
        switch(skillType)
        {
            case SkillType.TYPE_NONE:
                {
                    BaseSkill baseSkill = new BaseSkill();
                    baseSkill.skillType = SkillType.TYPE_NONE;
                    return baseSkill;
                }
            case SkillType.TYPE_ATTACK:
                {
                    AttackSkill attackSkill = new AttackSkill();
                    attackSkill.skillType = SkillType.TYPE_ATTACK;
                    return attackSkill;
                }

            case SkillType.TYPE_DEFFENCE:
                {
                    DeffenceSkill deffenceSkill = new DeffenceSkill();
                    deffenceSkill.skillType = SkillType.TYPE_DEFFENCE;
                    return deffenceSkill;
                }
        }

        return null;
    }

}
