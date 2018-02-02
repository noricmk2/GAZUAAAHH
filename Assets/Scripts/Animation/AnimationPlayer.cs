using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : BaseObject{

    AnimationType animationType;
    Animator targetAnimator;
    bool animationEnd;
    public bool AnimationEnd
    {
       get { return animationEnd;}
    }
    float time;

   
	public void Init(GameObject _target) // 애니메이션 실행할 오브젝트를 받아옴
    {
        targetAnimator = _target.GetComponent<Animator>();
        animationType = AnimationType.TYPE_IDLE; //기본 타입 IDLE로 설정
        
        targetAnimator.SetInteger("State", (int)animationType);
        time = 0;
        animationEnd = false;

    }

    public void Update()
    {
        CustomUpdate();
    }

    public void PlayAnimation(AnimationType type)//실행할 애니메이션 타입 선택
    {
        animationType = type; //실행할 애니메이션 타입을 받아옴
        targetAnimator.SetInteger("State", (int)animationType);        
    }

    public override void CustomUpdate()//애니메이션 실행
    {
        animationEnd = false;
        if (animationType == AnimationType.TYPE_DEAD) //사망 애니메이션
            return;

        if (animationType != AnimationType.TYPE_IDLE) 
        {
            time += Time.deltaTime;
            AnimatorStateInfo animInfo = targetAnimator.GetCurrentAnimatorStateInfo(0);
            
            if (time>= animInfo.length) // 받아온 애니메이션의 실행이 끝나면 IDLE로 돌아옴
            {
                targetAnimator.SetInteger("State", (int)AnimationType.TYPE_IDLE);
                time = 0;
                animationEnd = true;
            }         
        }
    }
}
