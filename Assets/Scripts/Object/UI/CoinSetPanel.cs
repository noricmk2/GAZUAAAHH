using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSetPanel : MonoBehaviour
{
    Slider coinAmountSlider; //수량을 결정하는 슬라이더
    Player playerCharacter; //플레이어
    Coin settingCoin; //수량을 설정할 대상 코인
    Text costText; //현재의 코스트를 나타내는 텍스트
    float settingPrice; //결정한 수량의 가격
    int settingAmount; //결정한 수량

    public Image coinItemSelectImage = null; //공,수중 무엇을 선택했는지 보여주는 이미지

    public ChoicePanelType PanelType //공,수 선택을 판별하는 타입변수
    {
        get; set;
    }

    public void PanelInit(string cName) //패널내의 UI를 받아온후 공수타입 설정, 재설정 및 예외처리
    {
        costText = this.transform.GetChild(1).GetComponent<Text>();
        coinAmountSlider = this.transform.GetComponentInChildren<Slider>();
        playerCharacter = GameManager.Instance.PlayerCharacter;

        CoinName parseEnum = (CoinName)System.Enum.Parse(typeof(CoinName), cName);
        settingCoin = playerCharacter.DicCoin[parseEnum];

        if (settingCoin == null)
        {
            Debug.LogError("player do not have " + cName);
        }

        //코인이 이미 한번 세팅되었다면, 다시 세팅할지 물어보고 이전 세팅 초기화
        if (settingCoin.BattleType != CoinBattleType.TYPE_NORMAL_COIN)
        {
            UIManager.Instance.SetPopup("Do you want change coin set?", "OK" , null, "Cancle", 2, 
            () => 
            {
                settingCoin.BattleType = CoinBattleType.TYPE_NORMAL_COIN;
                BattleManager.Instance.RemainCost += settingCoin.CoinAmountInBattle * (int)settingCoin.MarketInfo.CurrentPrice;
                settingCoin.CoinAmountInBattle = 0;
                PanelInit(cName);
            },
            () =>
            {
                this.gameObject.SetActive(false);
            });
        }

        //슬라이더 값 초기화
        coinAmountSlider.value = 0;
        //코인의 하나의 가격이 코스트 자체를 넘는다면 예외처리 
        if (BattleManager.Instance.RemainCost < settingCoin.MarketInfo.CurrentPrice)
        {
            coinAmountSlider.interactable = false;
            UIManager.Instance.SetPopup("Cost Over", "Ok");
            this.gameObject.SetActive(false);
            return;
        }
        //현재코스트와 세팅코인의 가격에 맞게 슬라이더의 최대값 설정
        coinAmountSlider.maxValue = Mathf.FloorToInt(BattleManager.Instance.RemainCost / settingCoin.MarketInfo.CurrentPrice);
        coinAmountSlider.minValue = 0;

        costText.text = coinAmountSlider.value.ToString() + " / " + (BattleManager.Instance.RemainCost).ToString();
    }

    public void TempConfirm() //결정 버튼을 눌렀을 때의 임시 설정 처리
    {
        if(coinItemSelectImage == null)
        {
            Debug.LogError("coin UI is not exist");
            return;
        }

        //설정가격이 0라면 취소처리 
        if (settingPrice == 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        //현재코스트에서 설정한 수량만큼의 가격을 빼준다
        BattleManager.Instance.RemainCost -= (int)settingPrice;
        settingCoin.CoinAmountInBattle = settingAmount;

        //공수타입에 따라 이미지 세팅
        switch (PanelType)
        {
            case ChoicePanelType.TYPE_ATTACK_PANEL:
                settingCoin.BattleType = CoinBattleType.TYPE_ATTACK_COIN;
                coinItemSelectImage.color = Color.red;
                break;
            case ChoicePanelType.TYPE_DEFFENCE_PANEL:
                settingCoin.BattleType = CoinBattleType.TYPE_DEFFENCE_COIN;
                coinItemSelectImage.color = Color.blue;
                break;
        }
        this.gameObject.SetActive(false);
    }

    public void FinalConfirm(Button confirmBtn) //최종 결정, 턴종료시의 실행 메서드
    {
        UIManager.Instance.SetAntiInteractivePanel(true);
        confirmBtn.interactable = false;
        //프리팹으로 불러온 현재 오브젝트에서는 코루틴을 사용할수 없기때문에, 코루틴용 임시객체를 만든후 배틀매니저의 BattleStart를 호출
        GameObject obj = new GameObject("TempCoroutineObject");
        CoinSetPanel comp = obj.AddComponent<CoinSetPanel>();
        if (comp != null)
            comp.StartCoroutine(BattleManager.Instance.BattleStart());
    }

    public void ChangeSliderValue() //슬라이더를 통한 수량 변경시의 메서드
    {
        if (coinAmountSlider.value >= settingCoin.CoinAmount)
            coinAmountSlider.value = settingCoin.CoinAmount;

        settingAmount = Mathf.FloorToInt(coinAmountSlider.value);
        settingPrice = coinAmountSlider.value * settingCoin.MarketInfo.CurrentPrice;
        costText.text = settingPrice.ToString() + " / " + (BattleManager.Instance.RemainCost).ToString();
    }
}
