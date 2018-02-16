using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoSingleton<UIManager>
{
    GameObject antiInteractivePanelPrefab; //터치방지용 패널의 프리팹
    GameObject popupPrefab; //팝업창의 프리팹

    GameObject lobbyCanvasPrefab; //로비씬UI의 프리팹

    GameObject tradePanelPrefab; //거래창의 프리팹
    GameObject coinTogglePrefab; //거래창 코인목록에 쓰일 코인토글의 프리팹

    GameObject battleCanvasPrefab; //전투씬UI의 프리팹
    GameObject coinItemPrefab; //전투씬에서 동적으로 생성할 코인목록의 아이템 

    GameObject graphCameraPrefab; //그래프용 카메라의 프리팹
    Transform graphCanvasTrans; //그래프용 캔버스의 트랜스폼

    GameObject currentBattlegraph; //배틀씬중, 현재 그려지는 그래프
    GameObject antiInteractivePanel; //터치방지용 패널

    public BattleCanvas BattleCanvasUI
    {
        get; set;
    }

    public LobbyCanvas LobbyCanvasUI
    {
        get; set;
    }

    public TradePanel TradePanelUI
    {
        get; set;
    }

    public GameObject CurrentUIScreen //현재의 UI씬
    {
        get; set;
    }

    public void UIInit() //각 UI프리팹의 초기화
    {
        antiInteractivePanelPrefab = Resources.Load(ConstValue.AntiInteractivePanelPath) as GameObject;
        lobbyCanvasPrefab = Resources.Load(ConstValue.LobbyUIPath) as GameObject;
        tradePanelPrefab = Resources.Load(ConstValue.TradePanelPath) as GameObject;
        coinTogglePrefab = Resources.Load(ConstValue.CoinTogglePath) as GameObject;
        battleCanvasPrefab = Resources.Load(ConstValue.BattleUIPath) as GameObject;
        coinItemPrefab = Resources.Load(ConstValue.CoinUIPath) as GameObject;
        popupPrefab = Resources.Load(ConstValue.PopupUIPath) as GameObject;
        graphCameraPrefab = Resources.Load(ConstValue.GraphUICameraPath) as GameObject;
    }

    public void SetSceneUI(UIType uiType) //요청한 UI타입대로 UI화면을 세팅하는 메서드
    {
        switch (uiType)
        {
            case UIType.TYPE_UI_TITLE:
                break;
            case UIType.TYPE_UI_BATTLE_WAIT: //배틀씬중 입력대기상태 UI
                {
                    BattleCanvas battleCanvas = null;
                    Player player = GameManager.Instance.PlayerCharacter;

                    // 현재의 UI화면이 없거나 배틀UI가 아닐경우
                    if (CurrentUIScreen == null || CurrentUIScreen.name.Contains("BattleCanvas") != true)
                    {
                        //현재의 UI화면을 배틀UI로 세팅
                        CurrentUIScreen = Instantiate(battleCanvasPrefab);

                        //각 유닛의 HUD초기화
                        battleCanvas = CurrentUIScreen.GetComponent<BattleCanvas>();
                        Slider playerHUD = battleCanvas.GetPlayerHUD();
                        Slider enemyHUD = battleCanvas.GetEnemyHUD();

                        playerHUD.value = playerHUD.maxValue = player.mentalPoint;
                        enemyHUD.value = enemyHUD.maxValue = BattleManager.Instance.EnemyCharacter.mentalPoint;

                        if (graphCanvasTrans == null)
                            graphCanvasTrans = Instantiate(graphCameraPrefab).GetComponentInChildren<Canvas>().transform;
                    }

                    if(battleCanvas == null)
                        battleCanvas = CurrentUIScreen.GetComponent<BattleCanvas>();

                    //코인목록을 생성해야되는 스크롤뷰의 Content와 코인목록의 정보를 가진 플레이어 정보를 가지고 옴
                    Transform contentTrans = CurrentUIScreen.transform.Find("CoinScroll").GetChild(0).GetChild(0);
                    Dictionary<CoinName, Coin> playerCoin = player.DicCoin;

                    //스크롤뷰에 전에 쓰던 코인목록이 있다면 삭제
                    for (int i = 0; i < contentTrans.childCount; ++i)
                    {
                        Destroy(contentTrans.GetChild(i).gameObject);
                    }

                    //배틀화면에서 쓰이는 플레이어의 코인 목록을 동적으로 스크롤뷰에 생성
                    foreach (KeyValuePair<CoinName, Coin> pair in playerCoin)
                    {
                        GameObject UICoin = Instantiate(coinItemPrefab);
                        Text coinText = UICoin.transform.GetComponentInChildren<Text>();
                        coinText.text = pair.Key.ToString();
                        UICoin.transform.SetParent(contentTrans, false);
                    }

                    //BattleCanvas의 UI애니메이션 실행
                    battleCanvas.PlayAppearAnimation();

                    Button confirmBtn = battleCanvas.transform.GetChild(3).GetComponentInChildren<Button>();
                    confirmBtn.interactable = true;

                    //현재턴을 TurnText에 입력
                    Text turnText = battleCanvas.GetTurnText();
                    turnText.text = BattleManager.Instance.CurrentTurn.ToString();

                    //캔버스의 카메라를 메인으로 설정
                    //Canvas CurrentCanvas = CurrentUIScreen.transform.GetComponent<Canvas>();
                    //CurrentCanvas.worldCamera = Camera.main;
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
                    battleCanvas.PlayDisappearAnimation();
                }
                break;
            case UIType.TYPE_UI_TRADE:
                {
                    if(graphCanvasTrans == null)
                        graphCanvasTrans = Instantiate(graphCameraPrefab).GetComponentInChildren<Canvas>().transform;

                    if ( CurrentUIScreen.name.Contains("LobbyCanvas") == true)
                    {
                        if(TradePanelUI != null)
                        {
                            TradePanelUI.gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject panel = Instantiate(tradePanelPrefab);
                            panel.transform.SetParent(graphCanvasTrans, false);

                            TradePanelUI = panel.GetComponent<TradePanel>();
                            TradePanelUI.TradePanelInit();

                            Transform scrollTrans = TradePanelUI.GetCoinScrollContent();

                            Dictionary<CoinName, Coin> dicCoin = CoinManager.Instance.GetCoinDictionary();

                            foreach (KeyValuePair<CoinName, Coin> pair in dicCoin)
                            {
                                GameObject coinToggleObj = Instantiate(coinTogglePrefab);
                                Text coinText = coinToggleObj.transform.GetComponentInChildren<Text>();
                                coinText.text = pair.Key.ToString();
                                coinToggleObj.transform.SetParent(scrollTrans, false);
                            }
                        }
                    }
                }
                break;
            case UIType.TYPE_UI_LOBBY:
                {
                    if (LobbyCanvasUI == null)
                    {
                        CurrentUIScreen = Instantiate(lobbyCanvasPrefab);
                        Canvas CurrentCanvas = CurrentUIScreen.transform.GetComponent<Canvas>();
                        CurrentCanvas.worldCamera = Camera.main;
                        LobbyCanvasUI = CurrentUIScreen.GetComponent<LobbyCanvas>();
                    }
                    else
                    {
                        if (TradePanelUI != null)
                        {
                            TradePanelUI.gameObject.SetActive(false);
                        }
                    }
                }
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

    public void SetGraphUI(GraphType type, CoinName coin, bool view = true, bool change = false) //요청받은 그래프를 그리는 메서드
    {
        switch (type)
        {
            case GraphType.TYPE_IN_BATTLE_GRAPH: //배틀씬중에 그래프를 요청 받았을 경우
                {
                    //그래프를 보여달라는 요청일 경우
                    if (view)
                    {
                        //단순히 그래프 시세값 변경 요청일 경우 값만 변경후 리턴
                        if (change == true && currentBattlegraph != null)
                        {
                            Text price = currentBattlegraph.transform.GetChild(2).GetComponent<Text>();
                            price.text = CoinManager.Instance.GetCoinDictionary()[coin].MarketInfo.CurrentPrice.ToString();
                            return;
                        }

                        //배틀씬의 UI정보를 받아옴
                        BattleCanvas battleCanvas = CurrentUIScreen.GetComponent<BattleCanvas>();
                        GameObject coinScroll = battleCanvas.GetCoinScrollObject();
                        Transform scrollContent = coinScroll.transform.GetChild(0).GetChild(0);

                        //스크롤뷰 안의 코인목록을 순회하며 요청한 그래프의 코인일 경우 코인목록UI를 복사후 애니메이션 실행, 그래프 렌더
                        for (int i = 0; i < scrollContent.childCount; ++i)
                        {
                            GameObject coinUI = scrollContent.GetChild(i).gameObject;
                            string UIText = coinUI.GetComponentInChildren<Text>().text;

                            if (UIText == coin.ToString())
                            {
                                currentBattlegraph = Instantiate(coinUI);
                                coinUI.SetActive(false);
                                currentBattlegraph.transform.SetParent(graphCanvasTrans, false);
                                ((RectTransform)currentBattlegraph.transform).anchorMax = new Vector2(0, 1);
                                ((RectTransform)currentBattlegraph.transform).anchorMin = new Vector2(0, 1);
                                ((RectTransform)currentBattlegraph.transform).anchoredPosition = Vector2.zero;

                                Text price = currentBattlegraph.transform.GetChild(2).GetComponent<Text>();
                                price.text = CoinManager.Instance.GetCoinDictionary()[coin].MarketInfo.CurrentPrice.ToString();

                                currentBattlegraph.GetComponent<UICoin>().PlayAnimation();
                                
                                Image graphImage = currentBattlegraph.transform.GetChild(1).GetComponent<Image>();
                                graphImage.color = Color.white;
                                MarketManager.Instance.RegistLineGraph(coin, graphImage.transform);
                            }
                        }
                    }
                    else
                    {
                        //삭제 요청일 경우
                        Destroy(currentBattlegraph);
                    }
                }
                break;
            case GraphType.TYPE_IN_TRADE_GRAPH:
                {
                    TradePanel tradePanel = graphCanvasTrans.GetComponentInChildren<TradePanel>();
                    Image graphImage = tradePanel.GetGraphImage();
                    MarketManager.Instance.RegistLineGraph(coin, graphImage.transform);
                }
                break;
        }
    }

    public void SetAntiInteractivePanel(bool setting)
    {
        if (antiInteractivePanel == null)
        {
            antiInteractivePanel = Instantiate(antiInteractivePanelPrefab);
            antiInteractivePanel.SetActive(false);
        }

        antiInteractivePanel.transform.SetParent(CurrentUIScreen.transform, false);

        if (setting)
        {
            antiInteractivePanel.SetActive(true);
        }
        else
        {
            antiInteractivePanel.SetActive(false);
        }
    }
}
