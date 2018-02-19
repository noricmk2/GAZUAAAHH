using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoSingleton<BattleManager>
{
    GameObject StageMap;

    GameObject enemyPrefab;
    bool skip = false;

    Player player; //플레이어

    Enemy _enemy; 
    public Enemy EnemyCharacter //현재 스테이지의 에너미
    {
        get { return _enemy; }
    }

    int _currentTurn; 
    public int CurrentTurn //현재 진행턴
    {
        get { return _currentTurn; }
    }

    public int CurrentTurnCost
    {
        get; set;
    }

    private void Update()
    {
        if(UIManager.Instance.IsAntiInteractive() == true && Input.GetMouseButtonDown(0) == true )
        {
            skip = true;
        }
    }

    public void BattleInit(Stage stage) //배틀씬의 초기화
    {
        _currentTurn = 1;

        player = GameManager.Instance.PlayerCharacter;
        StageMap = Instantiate(Resources.Load("Import/Map/Low Poly Golden Gate(fix)") as GameObject); //맵 생성

        //스테이지에 따른 에너미 할당
        switch (stage)
        {
            case Stage.STAGE1:
                enemyPrefab = GameManager.Instance.GetCharacterPrefab(CharacterType.TYPE_ENEMY1);
                break;
            case Stage.STAGE2:
                enemyPrefab = GameManager.Instance.GetCharacterPrefab(CharacterType.TYPE_ENEMY2);
                break;
            case Stage.STAGE3:
                enemyPrefab = GameManager.Instance.GetCharacterPrefab(CharacterType.TYPE_ENEMY3);
                break;
        }

        //_enemy = Instantiate(enemyPrefab,new Vector3(0,0,10),new Quaternion(0,180,0,0)).GetComponent<Enemy>();
        _enemy = Instantiate(enemyPrefab).GetComponent<Enemy>(); //프리팹이 좌표값을 가지도록 수정
        _enemy.Init();
        _enemy.target = player;
        player.target = _enemy;

        //턴 갱신과 UI세팅
        TurnChange();
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE_WAIT);
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

    public IEnumerator BattleStart() //모든 입력이 끝나고 전투에 돌입했을시의 실행 메서드
    {
        //에너미의 AI실행
        _enemy.SelectCoin();

        //양측의 전투 코인 정보를 받아옴
        List<Coin> listPlayerCoin = new List<Coin>();
        List<Coin> listEnemyCoin = new List<Coin>();

        //코인목록별로 UIManager에게 그래프를 그려달라고 요청후 한번씩 시세변경을 시도한다
        foreach (KeyValuePair<CoinName, Coin> pair in player.DicCoin)
            MarketManager.Instance.ChangeMarketInfo(pair.Key);

        foreach (KeyValuePair<CoinName, Coin> pair in player.DicCoin)
        {
            listPlayerCoin.Add(pair.Value);
            UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_BATTLE_GRAPH, pair.Key, true);
            yield return new WaitForSeconds(1);
            if (skip == true)
            {
                UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_BATTLE_GRAPH, pair.Key, false);
                break;
            }
            MarketManager.Instance.RenderLineGraph(pair.Key);
            UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_BATTLE_GRAPH, pair.Key, true, true);
            yield return new WaitForSeconds(1);
            if (skip == true)
            {
                UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_BATTLE_GRAPH, pair.Key, false);
                break;
            }
            UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_BATTLE_GRAPH, pair.Key, false);
        }

        if(skip == true)
        {
            GameObject coinScroll = UIManager.Instance.BattleCanvasUI.GetCoinScrollObject();
            Transform scrollContent = coinScroll.transform.GetChild(0).GetChild(0);

            for (int i = 0; i < scrollContent.childCount; ++i)
            {
                GameObject coinUI = scrollContent.GetChild(i).gameObject;
                coinUI.SetActive(false);
            }
        }

        foreach (KeyValuePair<CoinName, Coin> pair in _enemy.DicCoin)
        {
            listEnemyCoin.Add(pair.Value);
        }

        //전투용UI로 변경
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE_ATTACK);

        yield return new WaitForSeconds(1);

        CalCulDamage(listPlayerCoin, listEnemyCoin);
    }

    public void CalCulDamage(List<Coin> playerCoinList, List<Coin> enemyCoinList) //데미지 계산후 양 캐릭에게 전달하는 메서드
    {
        int playerAttackPoint = 0;
        int playerDeffencePoint = 0;
        int enemyAttackPoint = 0;
        int enemyDeffencePoint = 0;

        //코인목록을 순회하며 타입에 따라 전투력을 누적시킨후 양측에게 알려줌
        player.CoinAnimationPlay = true; // 코인 애니메이션 실행
        _enemy.CoinAnimationPlay = true; // 코인 애니메이션 실행

        for (int i = 0; i < playerCoinList.Count; ++i)
        {
            Coin coin = playerCoinList[i];
            if (coin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
            {
                playerAttackPoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;
                if (coin.MarketInfo.DifferPrice < 0) // 코인이 하나라도 하락세일 때 코인 애니메이션 실행하지 않음
                    player.CoinAnimationPlay = false;
            }
            if (coin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
                playerDeffencePoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;

            //배틀에서 사용한 수량만큼 소유코인에서 삭감
            coin.CoinAmount -= coin.CoinAmountInBattle;
            coin.CoinAmountInBattle = 0;
        }

        for (int i = 0; i < enemyCoinList.Count; ++i)
        {
            Coin coin = enemyCoinList[i];
            if (coin.BattleType == CoinBattleType.TYPE_ATTACK_COIN)
            {
                enemyAttackPoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;
                if (coin.MarketInfo.DifferPrice < 0) // 코인이 하나라도 하락세일 때 코인 애니메이션 실행하지 않음
                    _enemy.CoinAnimationPlay = false;
            }
            if (coin.BattleType == CoinBattleType.TYPE_DEFFENCE_COIN)
                enemyDeffencePoint += (int)coin.MarketInfo.CurrentPrice * coin.CoinAmountInBattle;

            coin.CoinAmount -= coin.CoinAmountInBattle;
            coin.CoinAmountInBattle = 0;
        }

        player.SetBattlePoint(playerAttackPoint, playerDeffencePoint);
        _enemy.SetBattlePoint(enemyAttackPoint, enemyDeffencePoint);
    }

    public void TurnEnd()
    {
        skip = false;
        _currentTurn++;
        TurnChange();
        UIManager.Instance.SetAntiInteractivePanel(false);
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE_WAIT);
    }
}
