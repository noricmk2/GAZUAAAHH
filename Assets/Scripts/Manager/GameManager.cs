using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    GameObject playerPrefab;
    GameObject enemyPrefab1;
    GameObject enemyPrefab2;
    GameObject enemyPrefab3;

    Player _player;
    public Player PlayerCharacter
    {
        get { return _player; }
    }

    SceneType _currentScene;
    public SceneType CurrentScene
    {
        get { return _currentScene; }
    }

    public Stage CurrentStage
    {
        get; set;
    }

    Dictionary<CharacterType, Character> dicEnemy = new Dictionary<CharacterType, Character>();
    Dictionary<CharacterType, GameObject> dicCharacterPrefabs = new Dictionary<CharacterType, GameObject>();

    private void Awake()
    {
        ResourceLoad();

        CoinManager.Instance.CoinInit();
        MarketManager.Instance.MarketInit();
        UIManager.Instance.UIInit();

        _player = Instantiate(dicCharacterPrefabs[CharacterType.TYPE_PLAYER]).GetComponent<Player>();
        _player.SetView(false); // 2/19
        _player.Init();

        CurrentStage = Stage.STAGE1;
    }

    private void Start()
    {
        //SceneChange(SceneType.TYPE_SCENE_LOGO);
        SceneChange(SceneType.TYPE_SCENE_ROBBY);
        //SceneChange(SceneType.TYPE_SCENE_BATTLE);
    }

    private void Update() //모든 커스텀업데이트를 이곳에 돌리는 것으로 컨트롤함
    {
    }

    public void SceneChange(SceneType sceneType)
    {
        _currentScene = sceneType;

        switch (_currentScene)
        {
            case SceneType.TYPE_SCENE_LOGO:
                break;
            case SceneType.TYPE_SCENE_ROBBY:
                _player.SetView(false); // 2/19
                UIManager.Instance.SetSceneUI(UIType.TYPE_UI_LOBBY); //02/13추가
                break;
            case SceneType.TYPE_SCENE_BATTLE:
                _player.SetView(true); // 2/19
                BattleManager.Instance.BattleInit(CurrentStage);
                break;
        }
    }

    public Character GetCharcter(CharacterType type)
    {
        return dicEnemy[type];
    }

    public void SetCharacter(CharacterType type, Character character)
    {
        dicEnemy[type] = character;
    }

    public GameObject GetCharacterPrefab(CharacterType type)
    {
        return dicCharacterPrefabs[type];
    }

    void ResourceLoad()
    {
        playerPrefab = Resources.Load(ConstValue.PlayerPrefabPath) as GameObject;
        if (playerPrefab == null)
        {
            Debug.LogError("player is not exist");
            return;
        }
        enemyPrefab1 = Resources.Load(ConstValue.Enemy1PrefabPath) as GameObject;
        if (enemyPrefab1 == null)
        {
            Debug.LogError("enemy1 is not exist");
            return;
        }
        enemyPrefab2 = Resources.Load(ConstValue.Enemy2PrefabPath) as GameObject;
        if (enemyPrefab2 == null)
        {
            Debug.LogError("enemy2 is not exist");
            return;
        }
        enemyPrefab3 = Resources.Load(ConstValue.Enemy3PrefabPath) as GameObject;
        if (enemyPrefab3 == null)
        {
            Debug.LogError("enemy3 is not exist");
            return;
        }

        dicCharacterPrefabs[CharacterType.TYPE_PLAYER] = playerPrefab;
        dicCharacterPrefabs[CharacterType.TYPE_ENEMY1] = enemyPrefab1;
        dicCharacterPrefabs[CharacterType.TYPE_ENEMY2] = enemyPrefab2;
        dicCharacterPrefabs[CharacterType.TYPE_ENEMY3] = enemyPrefab3;
    }
}
