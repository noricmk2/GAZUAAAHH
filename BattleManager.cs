using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoSingleton<BattleManager>
{
   public Enemy enemyCharacter;
   public Player playerCharacter;

    List<Coin> listPlayerCoins;
    List<Coin> listEnemyCoins;

    int turnNumber; //현재 진행중인 턴이 몇번째인지 나타내는 변수

    bool battleEnd;  
    bool isPause = false; //임시
    bool isDrawCoin = false;//적과 플레이어가 코인을 드로우 했나
    bool isCoinSetting = false; //코인 세팅이 끝나서 전투할 준비가 됐나
   

    //임시 ///////////////////////////////////////////
    public void Start()
    {
        Init();
        turnNumber = 1;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
            isPause = true;


        if (isPause == true)
        {
            CustomUpdate();
            isPause = false;
        }
    }
    ///////////////////////////////////////////////
    

    public override void Init()
    {
        enemyCharacter.Init();
        playerCharacter.Init();
        battleEnd = false;
        turnNumber = 0;

    }


    public override void CustomUpdate()
    {
        playerCharacter.CustomUpdate();
        enemyCharacter.CustomUpdate();

        CoinSelect();
        if(isCoinSetting)
        {
            CoinBattle();
            isCoinSetting = false;
            isDrawCoin = false;
        }
        turnNumber++;
        if (battleEnd)
        {
            BattleResult();
        }
    }

    public void CoinSelect()
    {
        if(isDrawCoin == false)
        {
            playerCharacter.DrawCoin();
            enemyCharacter.DrawCoin();
            isDrawCoin = true;
        }

        playerCharacter.SelectCoin();
        
        if(playerCharacter.isSelected)
        {
            enemyCharacter.SelectCoin();
            listPlayerCoins = playerCharacter.ThrowCoinList(); 
            listEnemyCoins = enemyCharacter.ThrowCoinList();
            isCoinSetting = true;
        }

    }

    public void CoinBattle()
    {
        float playerPoint = 0 ; // 플레이어 점수
        float enemyPoint = 0; // 에너미 점수

        //TODO 받아온 코인 리스트에서 스킬과 공격력 등을 계산해 결과값 생성후 Point변수에 할당
        foreach(Coin enemycoin in listEnemyCoins)
        {
            //if(enemycoin.CoinSkill != null)
            //{
            //    //스킬 사용
            //}

            enemyPoint += enemycoin.MarketInfo.CurrentPrice;

        }

        foreach (Coin playercoin in listPlayerCoins)
        {
            //if (playercoin.CoinSkill != null)
            //{
            //    //스킬 사용
            //}

            playerPoint += playercoin.MarketInfo.CurrentPrice;

        }

        if (playerPoint>enemyPoint)
        {
            playerCharacter.Attack(playerPoint - enemyPoint);           
        }
        else if(playerPoint < enemyPoint)
        {
            enemyCharacter.Attack(enemyPoint- playerPoint);         
        }
        else
        {
            
            //TODO 무승부 화면 출력후 다음 턴으로
        }

        playerCharacter.isSelected = false;
        Debug.Log("에너미 HP : "+enemyCharacter.mentalPoint+" 플레이어 HP : "+playerCharacter.mentalPoint+"에너미 점수 : "+enemyPoint+"플레이어 점수 : "+playerPoint );
        Debug.Log("턴 : " + turnNumber);

        //TODO 전투 종료 시 battleEnd 변수를 true로 바꿔 전투 종료를 알림
        if(enemyCharacter.mentalPoint <=0 || playerCharacter.mentalPoint <=0)
        {
            battleEnd = true;
            
        }
    }

    public void BattleResult()
    {

        //TODO 전투 결과 UI출력
        Debug.Log("배틀종료");
        
    }

}
