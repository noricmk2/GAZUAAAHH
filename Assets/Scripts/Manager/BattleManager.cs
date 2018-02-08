using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoSingleton<BattleManager>
{
    Enemy enemy; //현재 스테이지의 에너미
    Player player; //플레이어
    int _currentTurn; //현재 진행턴

    public int CurrentTurn
    {
        get { return _currentTurn; }
    }

    public int CurrentTurnCost
    {
        get; set;
    }

    public void BattleInit(Stage stage) //배틀씬의 초기화
    {
        _currentTurn = 1;

        player = GameManager.Instance.GetCharcter(CharacterType.TYPE_PLAYER) as Player;

        //스테이지에 따른 에너미 할당
        switch (stage)
        {
            case Stage.STAGE1:
                enemy = GameManager.Instance.GetCharcter(CharacterType.TYPE_ENEMY1) as Enemy;
                break;
            case Stage.STAGE2:
                enemy = GameManager.Instance.GetCharcter(CharacterType.TYPE_ENEMY2) as Enemy;
                break;
            case Stage.STAGE3:
                enemy = GameManager.Instance.GetCharcter(CharacterType.TYPE_ENEMY3) as Enemy;
                break;
        }

        //턴 갱신과 UI세팅
        TurnChange();
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE_WAIT);
    }

    public override void CustomUpdate()
    {
         switch(player.characterState)
         {
            case CharacterState.TYPE_IDLE:
                break;
            case CharacterState.TYPE_BATTLE:
                break;
            case CharacterState.TYPE_DEAD:
                break;
         }
    }

    void TurnChange() //턴 갱신용 메서드
    {
        //턴이 진행될때마다 코스트를 바꿔준다
        if (CurrentTurn == 1)
            CurrentTurnCost = ConstValue.FirstTurnCost;
        else if (CurrentTurn == 2)
            CurrentTurnCost = ConstValue.SecondTurnCost;
        else if (CurrentTurn == 3)
            CurrentTurnCost = ConstValue.ThirdTurnCost;
        else if (CurrentTurn == 4)
            CurrentTurnCost = ConstValue.ForthTurnCost;
        else if (CurrentTurn == 5)
            CurrentTurnCost = ConstValue.FifthTurnCost;
        else if (CurrentTurn == 6)
            CurrentTurnCost = ConstValue.SixthTurnCost;
        else if (CurrentTurn == 7)
            CurrentTurnCost = ConstValue.SeventhTurnCost;
        else if (CurrentTurn == 8)
            CurrentTurnCost = ConstValue.EighthTurnCost;
        else if (CurrentTurn == 9)
            CurrentTurnCost = ConstValue.NinthTurnCost;
        else
            CurrentTurnCost = ConstValue.TenthTurnCost;
    }

    public void BattleStart() //모든 입력이 끝나고 전투에 돌입했을시의 실행 메서드
    {
        //전투용UI로 변경
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE_ATTACK);

        //에너미의 AI실행
        enemy.SelectCoin();

        //양측의 전투 코인 정보를 받아옴
        List<Coin> listPlayerCoin = new List<Coin>();
        List<Coin> listEnemyCoin = new List<Coin>();

        foreach (KeyValuePair<CoinName, Coin> pair in player.DicCoin)
        {
            listPlayerCoin.Add(pair.Value);
        }

        foreach (KeyValuePair<CoinName, Coin> pair in enemy.DicCoin)
        {
            listEnemyCoin.Add(pair.Value);
        }

        CalCulDamage(listPlayerCoin, listEnemyCoin);
    }

    public void CalCulDamage(List<Coin> playerCoinList, List<Coin> enemyCoinList) //데미지 계산후 양 캐릭에게 전달하는 메서드
    {
        int playerAttackPoint = 0;
        int playerDeffencePoint = 0;
        int enemyAttackPoint = 0;
        int enemyDeffencePoint = 0;

        //코인목록을 순회하며 타입에 따라 전투력을 누적시킨후 양측에게 알려줌
        for (int i = 0; i < playerCoinList.Count; ++i)
        {
            Coin coin = playerCoinList[i];
            if (coin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
                playerAttackPoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;
            if (coin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
                playerDeffencePoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;
        }

        for (int i = 0; i < enemyCoinList.Count; ++i)
        {
            Coin coin = enemyCoinList[i];
            if (coin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
                enemyAttackPoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;
            if (coin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
                enemyDeffencePoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;
        }
    }

    public void TurnEnd()
    {
        _currentTurn++;
        TurnChange();
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE_WAIT);
    }
}
