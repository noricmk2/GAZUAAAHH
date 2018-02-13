using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleEnterUI : BaseObject {

    Stage stage;
 
    public override void Init()
    {      
        gameObject.SetActive(false);
        //image = GetComponent<Image>();
    }

    public void OnBattleEnter(Stage _stage)
    {
        stage = _stage;
        gameObject.SetActive(true);
    }

    public void BattleStart()
    {
        LobbyCanvas lobby = GetComponentInParent<LobbyCanvas>();
        gameObject.SetActive(false);
        lobby.LobbyOut();

        GameManager.Instance.SceneChange(SceneType.TYPE_SCENE_BATTLE);
    }
	
    public void Cancle()
    {
        gameObject.SetActive(false);       
    }
}
