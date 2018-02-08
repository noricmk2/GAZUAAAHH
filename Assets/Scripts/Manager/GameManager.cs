using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    //test
    GameObject playerCharacter;
    GameObject enemyCharacter1;
    GameObject enemyCharacter2;
    GameObject enemyCharacter3;
    public Transform testTargetTrans;
    Dictionary<CharacterType, Character> dicCharacter = new Dictionary<CharacterType, Character>(); 

    private void Awake()
    {
        playerCharacter = Resources.Load(ConstValue.PlayerPrefabPath) as GameObject;
        if(playerCharacter == null)
        {
            Debug.LogError("player is not exist");
            return;
        }
        enemyCharacter1 = Resources.Load(ConstValue.Enemy1PrefabPath) as GameObject;
        if (enemyCharacter1 == null)
        {
            Debug.LogError("enemy1 is not exist");
            return;
        }
        enemyCharacter2 = Resources.Load(ConstValue.Enemy2PrefabPath) as GameObject;
        if (enemyCharacter2 == null)
        {
            Debug.LogError("enemy2 is not exist");
            return;
        }
        enemyCharacter3 = Resources.Load(ConstValue.Enemy3PrefabPath) as GameObject;
        if (enemyCharacter3 == null)
        {
            Debug.LogError("enemy3 is not exist");
            return;
        }

        dicCharacter[CharacterType.TYPE_PLAYER] = playerCharacter.GetComponent<Player>();
        dicCharacter[CharacterType.TYPE_PLAYER].Init();
        dicCharacter[CharacterType.TYPE_ENEMY1] = enemyCharacter1.GetComponent<Enemy>();
        dicCharacter[CharacterType.TYPE_ENEMY1].Init();
        dicCharacter[CharacterType.TYPE_ENEMY2] = enemyCharacter2.GetComponent<Enemy>();
        dicCharacter[CharacterType.TYPE_ENEMY2].Init();
        dicCharacter[CharacterType.TYPE_ENEMY3] = enemyCharacter3.GetComponent<Enemy>();
        dicCharacter[CharacterType.TYPE_ENEMY3].Init();

        //test
        CoinManager.Instance.CoinInit();
        MarketManager.Instance.MarketInit();
        UIManager.Instance.UIInit();
        BattleManager.Instance.BattleInit(Stage.STAGE1);
    }

    private void Update() //모든 커스텀업데이트를 이곳에 돌리는 것으로 컨트롤함
    {
        //MarketManager.Instance.CustomUpdate();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    testAnim.StartCoroutine("SpawnCoin");
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    testAnim.StartAnimation = true;
        //}
    }

    public Character GetCharcter(CharacterType type)
    {
        return dicCharacter[type];
    }
}
