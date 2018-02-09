using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    Text turnText;
    Slider enemyHUD;
    Slider playerHUD;
    GameObject choicePanel;
    GameObject coinScrollObj;
    CoinSetPanel coinSetPanel;
    Animation[] UIAnimations = new Animation[5];

    float _uiTime;
    public float UIAnimTime
    {
        get { return _uiTime; }
    }
    bool disappearAnim = false;

    private void Awake()
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

    private void Update()
    {
        if (disappearAnim && UIAnimTime < 1.0f)
        {
            _uiTime += Time.deltaTime;
        }
    }

    public void PlayDisappearAnimation()
    {
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

    public void PlayAppearAnimation()
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

    public void PlayerHudAnimation(float value, bool isPlayerHUD)
    {
        Slider animatedSlider;

        if (isPlayerHUD)
            animatedSlider = playerHUD;
        else
            animatedSlider = enemyHUD;

        animatedSlider.value -= value;

        ShakeUI(BattleSceneUI.UI_PLAYER_HUD, 0.1f, 0.002f);
    }

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
