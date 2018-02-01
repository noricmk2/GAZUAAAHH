using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseObject
{
    protected AnimationPlayer animationPlayer;
    public Character target;
    protected List<Coin> listCoin; 
    public float mentalPoint; // 멘탈치 HP개념
    protected int cost;
    protected List<Coin> listDrawCoins; //매 턴마다 드로우하는 코인들을 받을 변수
    protected List<Coin> listSelectCoins; //선택한 코인을 저장하는 변수
    public bool isSelected; //플레이어가 코인을 선택했는지 여부를 나타냄

    //임시 UI


    //임시///////////////////////////////////////////////////////////////////
    private void Start()
    {
        
        listCoin = new List<Coin>();
        for (int i = 0; i < 15; i++)
        {
            Coin coin = new Coin(15, CoinName.AOS, null);
            listCoin.Add(coin);
            
        }

        for (int i = 0; i < 10; i++)
        {
            Coin coin = new Coin(5, CoinName.ATHURIUM, null);
            listCoin.Add(coin);
        }
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    public override void Init()
    {
        animationPlayer = new AnimationPlayer();
        animationPlayer.Init(gameObject);
        //TODO 코인 매니저에서 코인정보 획득

        //DicCoin = CoinManager.Instance.GetCoinDictionary(); // 수정필요

        mentalPoint = 100; //임시 값
        cost = 3; //임시 값
        isSelected = false;
    }
    

    public void Update()//임시 
    {
        animationPlayer.CustomUpdate();
    }

    public void DrawCoin() //덱에서 코인을 랜덤으로 5장 드로우
    {
        //랜덤으로 5장 드로우 
        int[] tmp = new int[5];
        int cnt = 0;

        for(int i = 0; i<4; i++)
        {
            tmp[i] = Random.Range(0, listCoin.Count);
        }

        listDrawCoins = new List<Coin>();
        listSelectCoins = new List<Coin>(); //선택한 코인을 저장할 리스트

        foreach (Coin Deck in listCoin)
        {
            for(int i = 0; i < 5; i++)
            {
               if(tmp[i] == cnt)
               {
                    listDrawCoins.Add(Deck);
               }
            }            
            cnt++;
        }       
    }

    public virtual void SelectCoin() //드로우한 코인중 사용할 코인 선택
    {      

    }
        

    public virtual void Attack(float damage) //매니저에서 전투 계산 후 자신의 값이 높을 경우 공격 실행
    {
        //TODO 공격 애니메이션 실행
        animationPlayer.PlayAnimation(AnimationType.TYPE_ATTACK);
        //TODO 정확한 피격 시점에 실행하도록 변경
        target.GetDamage(damage);
        

    }

    public virtual void GetDamage(float damage) //매니저에서 전투 계산 후 자신의 값이 낮을 경우 피격 실행
    {
        //TODO 피격 애니메이션 실행
        animationPlayer.PlayAnimation(AnimationType.TYPE_DAMAGE);
        //이펙트 출력
        mentalPoint -= damage;

        if(mentalPoint<=0)
        {
            Die();
        }
    }

    public virtual void Die() //HP가 0일 때 사망 출력
    {
        //TODO 사망 애니메이션 출력
        animationPlayer.PlayAnimation(AnimationType.TYPE_DEAD);
        //사망 애니메이션 종료후 게임 매니저에서 전투종료 화면 출력

       
    }

    public List<Coin> ThrowCoinList()
    {
        return listSelectCoins;
    }

}
