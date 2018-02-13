using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : BaseObject {

    public UIStage stage1;
    public UIStage stage2;
    public UIStage stage3;
    public BattleEnterUI battleEnter;
    public UIText text;

    public void Awake()
    {
        Init();
    }

    public override void Init()
    {
        stage1.Init();
        stage2.Init();
        stage3.Init();
        battleEnter.Init();
        text.Init();

    }

    public void Update()
    {
        text.CustomUpdate();
    }

    public void LobbyOut()
    {
        gameObject.SetActive(false);
    }

}
