
public enum ObjectState
{
    OBJECT_STATE_NORMAL,
    OBJECT_STATE_DEAD
}

public enum CoinName
{
    NEETCOIN,
    ATHURIUM,
    SUFFLE,
    AOS,
}

public enum MicroCoinTrend
{
    TREND_UP_MICRO,
    TREND_DOWN_MICRO,
    TREND_EQUAL_MICRO,
}

public enum MacroCoinTrend
{
    TREND_UP_MACRO,
    TREND_DOWN_MACRO,
    TREND_EQUAL_MACRO,
}

public enum GraphTarget
{
    GRAPH_COIN,
}

public enum AnimationType // 1/30일 오전 추가 
{
    TYPE_IDLE,
    TYPE_ATTACK,
    TYPE_DAMAGE,
    TYPE_DEAD
}   

public enum CharacterState
{
    TYPE_IDLE,
    TYPE_BATTLE,
    TYPE_DEAD,
}

public enum CoinBattleType
{
    TYPE_ATTACK_COIN,
    TYPE_DEFFENCE_COIN,
}

public enum CoinAnimType
{
    TYPE_BASE_ATTACK_ANIM,
    TYPE_BASE_DEFFENCE_ANIM,
    TYPE_GATE_BABYLON_ANIM,
}

public enum UIType
{
    TYPE_UI_TITLE,
    TYPE_UI_BATTLE,
    TYPE_UI_TRADE,
    TYPE_UI_ROBBY,
}

public enum CharacterType //02/06 추가
{
    TYPE_PLAYER,
    TYPE_ENEMY1,
    TYPE_ENEMY2,
    TYPE_ENEMY3
}

public enum ChoicePanelType
{
    TYPE_ATTACK_PANEL,
    TYPE_DEFFENCE_PANEL,
}