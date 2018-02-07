using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    //test
    public GameObject playerCharacter;
    public Transform testTargetTrans;
    Dictionary<CharacterType, Character> dicCharacter = new Dictionary<CharacterType, Character>(); 
    GameObject testCoin;
    CoinAnimation testAnim;
    List<Coin> testCoinList = new List<Coin>();

    private void Awake()
    {
        //test
        CoinManager.Instance.TestInit();
        MarketManager.Instance.MarketInit();
        BattleManager.Instance.BattleInit();

        testCoin = Resources.Load("Prefab/Sphere") as GameObject;
        testAnim = gameObject.GetComponent<CoinAnimation>();
        Coin testCoin1 = CoinManager.Instance.GetCoinDictionary()[CoinName.NEETCOIN];
        testAnim.CoinAnimInit(testCoin1, testCoin, this.transform, testTargetTrans, CoinAnimType.TYPE_GATE_BABYLON_ANIM);

        dicCharacter[CharacterType.TYPE_PLAYER] = playerCharacter.GetComponent<Player>();
        dicCharacter[CharacterType.TYPE_PLAYER].Init();

        UIManager.Instance.UIInit();
        //UIManager.Instance.SetSceneUI(UIType.TYPE_UI_BATTLE);
    }

    private void Update() //모든 커스텀업데이트를 이곳에 돌리는 것으로 컨트롤함
    {
        MarketManager.Instance.CustomUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            testAnim.StartCoroutine("SpawnCoin");
        }

        if (Input.GetMouseButtonDown(1))
        {
            testAnim.StartAnimation = true;
        }
    }

    public Character GetCharcter(CharacterType type)
    {
        return dicCharacter[type];
    }
}
