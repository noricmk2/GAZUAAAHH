using System.Collections.Generic;
using UnityEngine;

public class Character : BaseObject
{
    protected AnimationPlayer animationPlayer;
    public Character target;
    public Dictionary<CoinName, Coin> DicCoin { get; set; }
    public float mentalPoint; // 멘탈치 HP개념
    public int cost { get; set; } //코스트

    float attackPoint; // 배틀 매니저에서 받아온 공격포인트
    float deffencePoint; // 배틀 매니저에서 받아온 방어포인트
    bool isAttack; // 배틀에서 공격을 실행했는지 판단하는 편수
    bool isBattle;

    public CharacterType characterType { get; set; }
    public List<Coin> listSelectCoins { get; set; }//공격할 코인을 저장하는 변수    

    public override void Init()
    {
        isBattle = false;
        animationPlayer = GetComponent<AnimationPlayer>();
        animationPlayer.Init();
        animationPlayer.PlayAnimation(AnimationType.TYPE_IDLE);
        //TODO 코인 매니저에서 코인정보 획득

    }

    public virtual void SelectCoin() //드로우한 코인중 사용할 코인 선택
    {

    }

    public void SetBattlePoint(float AttackPoint, float DeffencePoint) //배틀 매니저로 부터 공격,방어 포인트를 받아옴
    {
        attackPoint = AttackPoint;
        deffencePoint = DeffencePoint;
        isAttack = false;
        if(characterType == CharacterType.TYPE_PLAYER)
        {
            Battle();
        }
    }

    public void Battle()
    {
        if (isAttack == false)
        {
            animationPlayer.PlayAnimation(AnimationType.TYPE_ATTACK);//공격 실행 
        }
        if (isAttack == true && target.isAttack == true)
        {
            TurnEnd();
        }
    }

    public virtual void Attack()
    {
        animationPlayer.PlayAnimation(AnimationType.TYPE_IDLE);
        if (isAttack == false)
        {
            target.Deffence(attackPoint); // 상대방 Dffence 실행
            isAttack = true;
        }
        
    }

    public virtual void Deffence(float damage)
    {

        bool _characteType;
        if (characterType == CharacterType.TYPE_PLAYER)
        {
            _characteType = true;
        }
        else
        {
            _characteType = false;
        }
        UIManager.Instance.CurrentUIScreen.GetComponent<BattleCanvas>().PlayerHudAnimation((damage), _characteType);

        if (damage > deffencePoint) // 방어력보다 공격력이 높을 때
        {
            mentalPoint -= (damage - deffencePoint); //데미지 적용
            //bool _characteType;
            //if(characterType == CharacterType.TYPE_PLAYER)
            //{
            //    _characteType = true;
            //}
            //else
            //{
            //    _characteType = false;
            //}
            //UIManager.Instance.CurrentUIScreen.GetComponent<BattleCanvas>().PlayerHudAnimation((damage - deffencePoint), _characteType);

            if (mentalPoint <= 0)
            {
                animationPlayer.PlayAnimation(AnimationType.TYPE_DEAD);
                return;
            }
            else
            {
                animationPlayer.PlayAnimation(AnimationType.TYPE_DAMAGE); //피격 애니메이션 실행                    
            }
        }
        else
        {
            animationPlayer.PlayAnimation(AnimationType.TYPE_DAMAGE);//임시
            //animationPlayer.PlayAnimation(AnimationType.TYPE_DEFFENCE); 

        }
    }

    public void TurnEnd()
    {
        if (isAttack == true && target.isAttack == true)
        {
            if (characterType == CharacterType.TYPE_PLAYER)
            BattleManager.Instance.TurnEnd();

            animationPlayer.PlayAnimation(AnimationType.TYPE_IDLE);
            target.animationPlayer.PlayAnimation(AnimationType.TYPE_IDLE);
        }
    }
}
