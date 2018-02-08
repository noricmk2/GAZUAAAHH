using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseObject
{
    protected AnimationPlayer animationPlayer;
    public Character target;
    public Dictionary<CoinName, Coin> DicCoin{ get; set;}
    public float mentalPoint; // 멘탈치 HP개념
    public int cost{ get; set;} //코스트

    float attackPoint; // 배틀 매니저에서 받아온 공격포인트
    float deffencePoint; // 배틀 매니저에서 받아온 방어포인트
    bool isAttack; // 배틀에서 공격을 실행했는지 판단하는 편수

    public CharacterType characterType{ get; set; }
    public List<Coin> listSelectCoins{ get; set; }//공격할 코인을 저장하는 변수    
    public CharacterState characterState; //캐릭터 상태(배틀매니저가 사용)
    
    public override void Init()
    {
        animationPlayer = GetComponent<AnimationPlayer>();
        animationPlayer.Init(gameObject);
        //TODO 코인 매니저에서 코인정보 획득

        mentalPoint = 100; //임시 값
        cost = 3; //임시 값
        characterState = CharacterState.TYPE_IDLE;
    }

    public virtual void SelectCoin() //드로우한 코인중 사용할 코인 선택
    {      

    }

    public void SetBattlePoint(float AttackPoint, float DeffencePoint) //배틀 매니저로 부터 공격,방어 포인트를 받아옴
    {
        attackPoint = AttackPoint;
        deffencePoint = DeffencePoint;
        isAttack = false;
    }

    public void Battle()
    {
        if (isAttack == true && target.isAttack == true) //플레이어와 에너미 둘다 공격을 실행했을 때
        {
            if(animationPlayer.IsAnimationPlay == false) //애니메이션 실행 중이 아닐때
                characterState = CharacterState.TYPE_IDLE; //캐릭터 상태를 IDLE로 변경   
            return;
        }

        if (characterType == CharacterType.TYPE_PLAYER) //플레이어일 때
        {
            if (isAttack == false)//아직 공격을 실행하지 않았으면   
            {
                Attack(); //공격 실행 
                isAttack = true;
            }

        }
        else  //Enemy일 때
        {
            if (target.isAttack == true && animationPlayer && animationPlayer.IsAnimationPlay == false) //플레이어가 공격을 실행했고 실행중인 애니메이션이 없을때           
            {
                Attack();  //공격실행    
                isAttack = true;
            }
        }

    }

    public virtual void Attack() 
    {      
        animationPlayer.PlayAnimation(AnimationType.TYPE_ATTACK); //공격 애니메이션 출력
        target.Deffence(attackPoint); // 상대방 Dffence 실행
    }

    public virtual void Deffence(float damage) 
    {       
        if(damage > deffencePoint) // 방어력보다 공격력이 높을 때
        {
            animationPlayer.PlayAnimation(AnimationType.TYPE_DAMAGE); //피격 애니메이션 실행
            mentalPoint -= (damage-deffencePoint); //데미지 적용
        }
        else
        {
            animationPlayer.PlayAnimation(AnimationType.TYPE_DAMAGE); //임시
            //방어 애니메이션 출력
        }

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
