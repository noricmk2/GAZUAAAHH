using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    Text turnText; //턴텍스트
    Slider enemyHUD; //에너미 hp바
    Slider playerHUD; //플레이어 hp바
    GameObject choicePanel; //공수 선택 패널
    GameObject coinScrollObj; //코인 목록 스크롤뷰
    CoinSetPanel coinSetPanel; //공수 선택시 수량 설정 패널
    Animation[] UIAnimations = new Animation[5]; //각 UI의 애니메이션

    private void Awake() //배틀씬의 모든 UI와 애니메이션을 받아옴
    {
        turnText = this.transform.GetChild(0).GetComponent<Text>();
        enemyHUD = this.transform.GetChild(1).GetComponent<Slider>();
        playerHUD = this.transform.GetChild(2).GetComponent<Slider>();
        choicePanel = this.transform.GetChild(3).gameObject;
        coinScrollObj = this.transform.GetChild(4).gameObject;
        coinSetPanel = this.transform.GetChild(5).GetComponent<CoinSetPanel>();

        UIAnimations[0] = turnText.transform.GetComponent<Animation>();
        UIAnimations[1] = enemyHUD.transform.GetComponent<Animation>();
        UIAnimations[2] = playerHUD.transform.GetComponent<Animation>();
        UIAnimations[3] = choicePanel.transform.GetComponent<Animation>();
        UIAnimations[4] = coinScrollObj.transform.GetComponent<Animation>();
    }

    public void PlayDisappearAnimation() //UI가 사라질때의 애니메이션
    {
        //각 UI에 설정된 애니메이션 클립의 스피드를 -1로 하고 시작을 끝으로 하여 역재생
        for (int i = 0; i < UIAnimations.Length; ++i)
        {
            Animation anim = UIAnimations[i];
            if (anim.gameObject.name.Contains("HUD"))
                continue;

            string clipName = anim.gameObject.name + "Appear";
            anim[clipName].speed = -1;
            anim[clipName].time = anim[clipName].length;
            anim.Play(clipName);
        }
    }

    public void PlayAppearAnimation() //UI가 나타날때의 애니메이션
    {
        for (int i = 0; i < UIAnimations.Length; ++i)
        {
            Animation anim = UIAnimations[i];
            string clipName = anim.gameObject.name + "Appear";
            anim[clipName].speed = 1;
            anim[clipName].time = 0;
            anim.Play(clipName);
        }
    }

    public void ShakeUI(BattleSceneUI ui, float Intensity, float decay)
    {

    }

    public void PlayerHudAnimation(float value, bool isPlayerHUD) //hp바의 애니메이션
    {
        Slider animatedSlider;

        if (isPlayerHUD)
            animatedSlider = playerHUD;
        else
            animatedSlider = enemyHUD;

        animatedSlider.value -= value;

        ShakeUI(BattleSceneUI.UI_PLAYER_HUD, 0.1f, 0.002f);
    }

    //이하 UI의 Getter 메서드
    public Text GetTurnText()
    {
        if (turnText == null)
        {
            Debug.LogError("TurnText is not exist");
            return null;
        }
        return turnText;
    }

    public Slider GetEnemyHUD()
    {
        if (enemyHUD == null)
        {
            Debug.LogError("EnemyHUD is not exist");
            return null;
        }
        return enemyHUD;
    }

    public Slider GetPlayerHUD()
    {
        if (playerHUD == null)
        {
            Debug.LogError("PlayerHUD is not exist");
            return null;
        }
        return playerHUD;
    }

    public GameObject GetChoicePanel()
    {
        if (choicePanel == null)
        {
            Debug.LogError("ChoicePanel is not exist");
            return null;
        }
        return choicePanel;
    }

    public CoinSetPanel GetCoinSetPanel()
    {
        if (coinSetPanel == null)
        {
            Debug.LogError("CoinSetPanel is not exist");
            return null;
        }
        return coinSetPanel;
    }

    public GameObject GetCoinScrollObject()
    {
        if (coinScrollObj == null)
        {
            Debug.LogError("CoinScrollObject is not exist");
            return null;
        }
        return coinScrollObj;
    }
}
