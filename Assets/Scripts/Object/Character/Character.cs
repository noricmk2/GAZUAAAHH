using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseObject
{
    protected AnimationPlayer animationPlayer;
    public Character target;
    protected Dictionary<CoinName, Coin> DicCoin;
    public float mentalPoint; // 멘탈치 HP개념
    protected int cost;
    float Asset; //자산

    public List<Coin> listSelectCoins //공격할 코인을 저장하는 변수
    {
        get; set;
    }


    public void Start()
    {
        //초기 자산 책정
    }
  

    public CharacterState characterState; //캐릭터 상태(배틀매니저가 사용)

    public override void Init()
    {
        animationPlayer = new AnimationPlayer();
        animationPlayer.Init(gameObject);
        //TODO 코인 매니저에서 코인정보 획득

       

        mentalPoint = 100; //임시 값
        cost = 3; //임시 값
        characterState = CharacterState.TYPE_IDLE;
    }

    public virtual void SelectCoin() //드로우한 코인중 사용할 코인 선택
    {      

    }
        

    public virtual void Attack() //매니저에서 전투 계산 후 자신의 값이 높을 경우 공격 실행
    { 
        // 배틀매니저에서 계산 결과를 받아옴

        animationPlayer.PlayAnimation(AnimationType.TYPE_ATTACK);

        
        if (animationPlayer.AnimationEnd)//애니메이션 끝나면 캐릭터 상태 변경
            characterState = CharacterState.TYPE_IDLE;
    }

    public virtual void GetDamage(float damage) //매니저에서 전투 계산 후 자신의 값이 낮을 경우 피격 실행
    {
       
        animationPlayer.PlayAnimation(AnimationType.TYPE_DAMAGE);
        //이펙트 출력
        mentalPoint -= damage;

        
        if (animationPlayer.AnimationEnd)  //애니메이션 끝나면 캐릭터 상태 변경
            characterState = CharacterState.TYPE_IDLE;

        if (mentalPoint<=0)
        {
            Die();
        }
    }

    public virtual void Die() //HP가 0일 때 사망 출력
    {
        //TODO 사망 애니메이션 출력
        animationPlayer.PlayAnimation(AnimationType.TYPE_DEAD);
        //사망 애니메이션 종료후 게임 매니저에서 전투종료 화면 출력
        characterState = CharacterState.TYPE_DEAD;

    }    
}
