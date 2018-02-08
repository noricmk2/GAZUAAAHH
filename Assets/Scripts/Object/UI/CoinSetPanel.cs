using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSetPanel : MonoBehaviour
{
    Slider coinAmountSlider;
    Player playerCharacter;
    Coin settingCoin;
    Text costText;
    float settingPrice;
    int settingAmount;

    public ChoicePanelType PanelType
    {
        get; set;
    }

    public void PanelInit(string cName)
    {
        costText = this.transform.GetChild(1).GetComponent<Text>();
        coinAmountSlider = this.transform.GetComponentInChildren<Slider>();
        playerCharacter = GameManager.Instance.GetCharcter(CharacterType.TYPE_PLAYER) as Player;

        CoinName parseEnum = (CoinName)System.Enum.Parse(typeof(CoinName), cName);
        settingCoin = playerCharacter.DicCoin[parseEnum];

        if (settingCoin == null)
        {
            Debug.LogError("player do not have " + cName);
        }
        if (settingCoin.BattleType != CoinBattleType.TYPE_NORMAL_COIN)
        {
            UIManager.Instance.SetPopup("Do you want change coin set?", "OK" , "Cancle", 2, 
            () => 
            {
                settingCoin.BattleType = CoinBattleType.TYPE_NORMAL_COIN;
                BattleManager.Instance.CurrentTurnCost += settingCoin.CoinAmountInBattle * (int)settingCoin.MarketInfo.CurrentPrice;
                settingCoin.CoinAmountInBattle = 0;
                PanelInit(cName);
            },
            () =>
            {
                this.gameObject.SetActive(false);
            });
        }

        coinAmountSlider.value = 0;
        if (BattleManager.Instance.CurrentTurnCost < settingCoin.MarketInfo.CurrentPrice)
        {
            coinAmountSlider.interactable = false;
            UIManager.Instance.SetPopup("Cost Over", "Ok");
            this.gameObject.SetActive(false);
            return;
        }
        coinAmountSlider.maxValue = BattleManager.Instance.CurrentTurnCost / settingCoin.MarketInfo.CurrentPrice;
        coinAmountSlider.minValue = 0;

        costText.text = coinAmountSlider.value.ToString() + " / " + ((int)BattleManager.Instance.CurrentTurnCost).ToString();
    }

    public void TempConfirm()
    {
        if (settingPrice == 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        BattleManager.Instance.CurrentTurnCost -= (int)settingPrice;
        settingCoin.CoinAmountInBattle = settingAmount;

        switch (PanelType)
        {
            case ChoicePanelType.TYPE_ATTACK_PANEL:
                settingCoin.BattleType = CoinBattleType.TYPE_ATTACK_COIN;
                break;
            case ChoicePanelType.TYPE_DEFFENCE_PANEL:
                settingCoin.BattleType = CoinBattleType.TYPE_DEFFENCE_COIN;
                break;
        }
        this.gameObject.SetActive(false);
    }

    public void FinalConfirm()
    {
        BattleManager.Instance.BattleStart();
    }

    public void ChangeSliderValue()
    {
        if (coinAmountSlider.value > settingCoin.CoinAmount)
            coinAmountSlider.value = settingCoin.CoinAmount;

        settingAmount = (int)coinAmountSlider.value;
        settingPrice = coinAmountSlider.value * settingCoin.MarketInfo.CurrentPrice;
        costText.text = settingPrice.ToString() + " / " + ((int)BattleManager.Instance.CurrentTurnCost).ToString();
    }
}
