using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    float _currentProperty; 
    public float CurrentProperty //현재 플레이어의 자산
    {
        get { return _currentProperty; }
        set { _currentProperty = value; }
    }

    public override void Init()
    {
        base.Init();
        characterType = CharacterType.TYPE_PLAYER;
        DicCoin = new Dictionary<CoinName, Coin>(); // 임시
        _currentProperty = 5000000f;//테스트용 자산 초기화
    }  
    public void SetView(bool isView)
    {
        SkinnedMeshRenderer skin = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();        
        skin.enabled = isView;       
    }

    public void BattleEnd()
    {
        if (mentalPoint > 0)
        {
            _currentProperty += 10000000f;
        }
        mentalPoint = maxMP;
        animationPlayer.PlayAnimation(AnimationType.TYPE_IDLE);
        transform.position = defaultPosition;       
    }
}
