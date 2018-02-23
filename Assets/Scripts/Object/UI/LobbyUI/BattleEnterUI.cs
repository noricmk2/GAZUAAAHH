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
        if (GameManager.Instance.PlayerCharacter.DicCoin.Count < 1)
        {
            UIManager.Instance.SetPopup("보유한 코인이 없습니다. Market에서 코인을 구매해 주세요", "OK");
            gameObject.SetActive(false);
        }
        else
        {
            LobbyCanvas lobby = GetComponentInParent<LobbyCanvas>();
            gameObject.SetActive(false);
            lobby.LobbyOut();

            GameManager.Instance.CurrentStage = stage;
            GameManager.Instance.SceneChange(SceneType.TYPE_SCENE_BATTLE);
        }
    }
	
    public void Cancle()
    {
        gameObject.SetActive(false);       
    }
}
