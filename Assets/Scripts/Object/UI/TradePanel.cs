using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradePanel : MonoBehaviour
{
    Transform coinScrollContent;
    Image graphImage;
    InputField priceField;
    InputField amountField;
    Button maxButton;
    Button orderButton;
    Toggle buyToggle;
    Toggle sellToggle;

    float deltaTime = 0;
    float renewTime = 2.0f;

    Coin _currentSelectCoin = null;
    public Coin CurrentSelectCoin
    {
        get { return _currentSelectCoin; }
        set { _currentSelectCoin = value; }
    }

    public TradeType SellBuyType
    {
        get; set;
    }

    CoinName PrevSelectCoin
    {
        get; set;
    } 

    public bool TradeActive
    {
        get; set;
    }

    public void TradePanelInit()
    {
        coinScrollContent = transform.GetChild(0).GetChild(0).GetChild(0);
        graphImage = transform.GetChild(1).GetComponent<Image>();
        orderButton = transform.GetChild(2).GetComponent<Button>();
        priceField = transform.GetChild(3).GetComponent<InputField>();
        amountField = transform.GetChild(4).GetComponent<InputField>();
        maxButton = transform.GetChild(5).GetComponent<Button>();

        PrevSelectCoin = CoinName.NONE;
        SellBuyType = TradeType.TYPE_TRADE_BUY;

        CoinToggleSelect(null);
    }

    private void Update()
    {
        if(TradeActive)
        {
            deltaTime += Time.deltaTime;

            if (deltaTime > renewTime)
            {
                deltaTime = 0;

            }
        }
    }

    public void CoinToggleSelect(Text coinText)
    {
        TradePanel currentPanel = UIManager.Instance.TradePanelUI;
        Grapher grapher;
        Toggle coinToggle = null;
        CoinName cName = CoinName.NONE;

        if (coinText != null)
        {
            string text = coinText.text;
            cName = (CoinName)System.Enum.Parse(typeof(CoinName), text);

            Transform scorllContent = currentPanel.GetCoinScrollContent();
            coinToggle = scorllContent.Find(cName.ToString()).GetComponent<Toggle>();
        }
        else
            cName = CoinName.NEETCOIN;

        currentPanel.CurrentSelectCoin = CoinManager.Instance.GetCoinDictionary()[cName];

        if (coinToggle != null && coinToggle.isOn == false)
            return;

        if (currentPanel.PrevSelectCoin == cName)
            return;

        currentPanel.PrevSelectCoin = cName;

        UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_TRADE_GRAPH, cName);
        MarketManager.Instance.ChangeMarketInfo(cName);
        grapher = MarketManager.Instance.RenderLineGraph(cName, 2, 600);

        Vector3 graphPos = grapher.GetLastGraphPosition();
        if (graphPos != Vector3.zero)
        {
            Image graph = currentPanel.GetGraphImage();
            InputField pField = currentPanel.GetPriceField();

            Text currentPrice = graph.transform.GetChild(0).GetComponent<Text>();
            Vector2 pricePos = ((RectTransform)currentPrice.transform).anchoredPosition;
            pricePos.y = graphPos.y;
            ((RectTransform)currentPrice.transform).anchoredPosition = pricePos;
            currentPrice.text = currentPanel.CurrentSelectCoin.MarketInfo.CurrentPrice.ToString();

            Text maxPrice = graph.transform.GetChild(1).GetComponent<Text>();
            Text minPrice = graph.transform.GetChild(2).GetComponent<Text>();

            maxPrice.text = currentPanel.CurrentSelectCoin.MarketInfo.MaxFlucRange.ToString();
            minPrice.text = currentPanel.CurrentSelectCoin.MarketInfo.MinFlucRange.ToString();

            pField.text = currentPanel.CurrentSelectCoin.MarketInfo.CurrentPrice.ToString();
        }
    }

    public void ClickMaxButton()
    {
        TradePanel currentPanel = UIManager.Instance.TradePanelUI;

        if (currentPanel.CurrentSelectCoin == null)
            return;

        Player player = GameManager.Instance.PlayerCharacter;
        InputField aField = currentPanel.GetAmountField();

        switch (currentPanel.SellBuyType)
        {
            case TradeType.TYPE_TRADE_BUY:
                {
                    int maxAmount = (int)(player.CurrentProperty / currentPanel.CurrentSelectCoin.MarketInfo.CurrentPrice);
                    aField.text = maxAmount.ToString();
                }
                break;
            case TradeType.TYPE_TRADE_SELL:
                {
                    if (player.DicCoin.ContainsKey(currentPanel.CurrentSelectCoin.Name) == false)
                    {
                        Transform parentTrans = UIManager.Instance.GraphCanvasTrans;
                        UIManager.Instance.SetPopup("You don't have this coin", "OK", parentTrans);
                        return;
                    }
                    int maxAmount = (int)(player.DicCoin[currentPanel.CurrentSelectCoin.Name].CoinAmount);
                    aField.text = maxAmount.ToString();
                }
                break;
            default:
                break;
        }
    }

    public void ClickOrderButton()
    {
        TradePanel currentPanel = UIManager.Instance.TradePanelUI;
        InputField aField = currentPanel.GetAmountField();
        Player player = GameManager.Instance.PlayerCharacter;
        int amount = int.Parse(aField.text);
        float price = currentPanel.CurrentSelectCoin.MarketInfo.CurrentPrice * amount;

        switch (currentPanel.SellBuyType)
        {
            case TradeType.TYPE_TRADE_BUY:
                {
                    if (player.CurrentProperty < price || price == 0)
                        return;

                    player.CurrentProperty -= price;
                    Coin target = CoinManager.Instance.GetCoinDictionary()[currentPanel.CurrentSelectCoin.Name];
                    if(player.DicCoin.ContainsKey(currentPanel.CurrentSelectCoin.Name) == false)
                        player.DicCoin.Add(currentPanel.CurrentSelectCoin.Name, new Coin(target));

                    player.DicCoin[currentPanel.CurrentSelectCoin.Name].CoinAmount += amount;
                    aField.text = "0";
                }
                break;
            case TradeType.TYPE_TRADE_SELL:
                {
                    if (price == 0)
                        return;

                    if (player.DicCoin.ContainsKey(currentPanel.CurrentSelectCoin.Name) == false)
                    {
                        Transform parentTrans = UIManager.Instance.GraphCanvasTrans;
                        UIManager.Instance.SetPopup("You don't have this coin", "OK", parentTrans);
                        return;
                    }

                    player.CurrentProperty += price;
                    player.DicCoin[currentPanel.CurrentSelectCoin.Name].CoinAmount -= amount;
                    aField.text = "0";
                }
                break;
        }
    }

    public void ClickOKButton()
    {
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_LOBBY);
    }

    public void ChangeAmountInputField()
    {
        TradePanel currentPanel = UIManager.Instance.TradePanelUI;
        InputField aField = currentPanel.GetAmountField();
        Player player = GameManager.Instance.PlayerCharacter;
        int amount = int.Parse(aField.text);
        float price = currentPanel.CurrentSelectCoin.MarketInfo.CurrentPrice * amount;

        if (amount < 0)
        {
            aField.text = "0";
            return;
        }

        switch (SellBuyType)
        {
            case TradeType.TYPE_TRADE_BUY:
                {
                    if (price > player.CurrentProperty)
                    {
                        int maxAmount = (int)(player.CurrentProperty / _currentSelectCoin.MarketInfo.CurrentPrice);
                        aField.text = maxAmount.ToString();
                        return;
                    }
                }
                break;
            case TradeType.TYPE_TRADE_SELL:
                {
                    if (amount > player.DicCoin[currentPanel.CurrentSelectCoin.Name].CoinAmount)
                    {
                        int maxAmount = player.DicCoin[currentPanel.CurrentSelectCoin.Name].CoinAmount;
                        aField.text = maxAmount.ToString();
                        return;
                    }
                }
                break;
        }
    }

    public void ChangeSellBuyToggle(Toggle toggle)
    {
        if (toggle.isOn == false)
            return;
        if (toggle.gameObject.name.Contains("Sell"))
            SellBuyType = TradeType.TYPE_TRADE_SELL;
        else
            SellBuyType = TradeType.TYPE_TRADE_BUY;
    }

    public Transform GetCoinScrollContent()
    {
        if (coinScrollContent == null)
        {
            Debug.LogError("CoinScrollContent is not exist");
            return null;
        }
        return coinScrollContent;
    }

    public Image GetGraphImage()
    {
        if (graphImage == null)
        {
            Debug.LogError("GraphImage is not exist");
            return null;
        }
        return graphImage;
    }

    public InputField GetPriceField()
    {
        if (priceField == null)
        {
            Debug.LogError("PriceField is not exist");
            return null;
        }
        return priceField;
    }

    public InputField GetAmountField()
    {
        if (amountField == null)
        {
            Debug.LogError("AmountField is not exist");
            return null;
        }
        return amountField;
    }

    public Button GetMaxButton()
    {
        if (maxButton == null)
        {
            Debug.LogError("MaxButton is not exist");
            return null;
        }
        return maxButton;
    }

    public Button GetOrderButton()
    {
        if(orderButton == null)
        {
            Debug.LogError("OrderButton is not exist");
            return null;
        }
        return orderButton;
    }

    public Toggle GetBuyToggle()
    {
        if (buyToggle == null)
        {
            Debug.LogError("BuyToggle is not exist");
            return null;
        }
        return buyToggle;
    }

    public Toggle GetSellToggle()
    {
        if (sellToggle == null)
        {
            Debug.LogError("SellToggle is not exist");
            return null;
        }
        return sellToggle;
    }
}
