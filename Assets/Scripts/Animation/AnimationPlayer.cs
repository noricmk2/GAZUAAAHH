using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : BaseObject
{ //플레이어,에너미 오브젝트에 추가해야 함

    AnimationType animationType;
    Animator Animator;
 
    public override void Init() // 애니메이션 실행할 오브젝트를 받아옴
    {
        animationType = AnimationType.TYPE_IDLE; //기본 타입 IDLE로 설정
        Animator = GetComponent<Animator>();
    }

    public void PlayAnimation(AnimationType type)//실행할 애니메이션 타입 선택
    {
        animationType = type; //실행할 애니메이션 타입을 받아옴 
        Animator.SetInteger("State", (int)animationType);
    }
}

