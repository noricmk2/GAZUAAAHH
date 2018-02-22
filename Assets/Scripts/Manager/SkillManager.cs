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
                    return baseSkill;
                }
            case SkillType.TYPE_ATTACK:
                {
                    AttackSkill attackSkill = new AttackSkill();                  
                    return attackSkill;
                }

            case SkillType.TYPE_DEFFENCE:
                {
                    DeffenceSkill deffenceSkill = new DeffenceSkill();                    
                    return deffenceSkill;
                }
        }

        return null;
    }

}
