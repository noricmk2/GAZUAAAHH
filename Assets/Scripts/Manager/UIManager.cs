using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoSingleton<UIManager>
{
    GameObject battleCanvasPrefab; //전투씬UI의 프리팹
    GameObject UICoinPrefab; //전투씬에서 동적으로 생성할 코인목록의 아이템 
    GameObject popupPrefab; //팝업창의 프리팹

    public GameObject CurrentUIScreen //현재의 UI씬
    {
        get; set;
    }

    public void UIInit() //각 UI프리팹의 초기화
    {
        battleCanvasPrefab = Resources.Load(ConstValue.BattleUIPath) as GameObject;
        UICoinPrefab = Resources.Load(ConstValue.CoinUIPath) as GameObject;
        popupPrefab = Resources.Load(ConstValue.PopupUIPath) as GameObject;
    }

    public void SetSceneUI(UIType uiType) //요청한 UI타입대로 UI화면을 세팅하는 메서드
    {
        switch (uiType)
        {
            case UIType.TYPE_UI_TITLE:
                break;
            case UIType.TYPE_UI_BATTLE_WAIT: //배틀씬중 입력대기상태 UI
                {
                    // 현재의 UI화면이 없거나 배틀UI가 아닐경우
                    if (CurrentUIScreen == null || CurrentUIScreen.name.Contains("BattleCanvas") != true)
                    {
                        //현재의 UI화면을 배틀UI로 세팅
                        CurrentUIScreen = Instantiate(battleCanvasPrefab);

                        //코인목록을 생성해야되는 스크롤뷰의 Content와 코인목록의 정보를 가진 플레이어 정보를 가지고 옴
                        Transform contentTrans = CurrentUIScreen.transform.Find("CoinScroll").GetChild(0).GetChild(0);
                        Player player = GameManager.Instance.GetCharcter(CharacterType.TYPE_PLAYER) as Player;
                        Dictionary<CoinName, Coin> playerCoin = player.DicCoin;

                        //배틀화면에서 쓰이는 플레이어의 코인 목록을 동적으로 스크롤뷰에 생성
                        foreach (KeyValuePair<CoinName, Coin> pair in playerCoin)
                        {
                            GameObject UICoin = Instantiate(UICoinPrefab);
                            Text coinText = UICoin.transform.GetComponentInChildren<Text>();
                            coinText.text = pair.Key.ToString();
                            UICoin.transform.SetParent(contentTrans, false);
                        }
                    }

                    //BattleCanvas를 받아와서 UI애니메이션 실행
                    BattleCanvas battleCanvas = CurrentUIScreen.GetComponent<BattleCanvas>();
                    battleCanvas.PlayAnimation();
                    //현재턴을 TurnText에 입력
                    Text turnText = battleCanvas.GetTurnText();
                    turnText.text = BattleManager.Instance.CurrentTurn.ToString();
                }
                break;
            case UIType.TYPE_UI_BATTLE_ATTACK: //배틀씬중 전투상태
                {
                    if (CurrentUIScreen.name.Contains("BattleCanvas") != true)
                    {
                        Debug.LogError("not in battle scene");
                        return;
                    }

                    //전투상태에 맞게 UI애니메이션을 역재생하여 빈 화면으로 만든다
                    BattleCanvas battleCanvas = CurrentUIScreen.GetComponent<BattleCanvas>();
                    battleCanvas.PlayReverseAnimation();
                }
                break;
            case UIType.TYPE_UI_TRADE:
                break;
            case UIType.TYPE_UI_ROBBY:
                break;
        }
    }

    //내용과 원버튼, 투버튼, OnCilck메서드를 세팅할수 있는 팝업창을 만드는 메서드
    public void SetPopup(string content, string btnText, string btnText2 = null, int btnCount = 1, 
        UnityAction function1 = null, UnityAction function2 = null)
    {
        if (CurrentUIScreen == null)
        {
            Debug.LogError("Scene has no UI");
            return;
        }

        GameObject popup = Instantiate(popupPrefab, CurrentUIScreen.transform);
        Text popupText = popup.transform.GetChild(0).GetComponent<Text>();
        Button confirmBtn = popup.transform.GetChild(1).GetComponent<Button>();
        Button cancleBtn = popup.transform.GetChild(2).GetComponent<Button>();
        Text confirmBtnText = popup.transform.GetChild(1).GetComponentInChildren<Text>();
        Text cancleBtnText = popup.transform.GetChild(2).GetComponentInChildren<Text>();

        //입력받은 버튼의 수만큼 버튼 생성
        switch (btnCount)
        {
            case 1:
                //버튼이 하나일때는 내용과 클릭용 메서드가 있을경우 넣어주고 생성
                popupText.text = content;
                confirmBtnText.text = btnText;
                if (function1 != null)
                    confirmBtn.onClick.AddListener(function1);
                break;
            case 2:
                {
                    //버튼이 두개일 경우 두개의 버튼을 세팅후 상동한 처리후 생성
                    cancleBtn.gameObject.SetActive(true);

                    RectTransform confirmBtnTrans = popup.transform.GetChild(1).GetComponent<RectTransform>();
                    RectTransform cancleBtnTrans = popup.transform.GetChild(2).GetComponent<RectTransform>();

                    Vector3 btnPos = confirmBtnTrans.localPosition;
                    btnPos.x = -60;
                    confirmBtnTrans.localPosition = btnPos;
                    btnPos.x = 60;
                    cancleBtnTrans.localPosition = btnPos;
                    cancleBtn.onClick.AddListener(function2);

                    popupText.text = content;
                    confirmBtnText.text = btnText;
                    cancleBtnText.text = btnText2;

                    if (function1 != null)
                        confirmBtn.onClick.AddListener(function1);
                    if (function2 != null)
                        cancleBtn.onClick.AddListener(function2);
                }
                break;
        }
    }
}
