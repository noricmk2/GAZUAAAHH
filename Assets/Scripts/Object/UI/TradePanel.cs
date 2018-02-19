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

    float deltaTime = 0;
    float renewTime = 2.0f;

    Coin _currentSelectCoin = null;
    public Coin CurrentSelectCoin
    {
        get { return _currentSelectCoin; }
        set { _currentSelectCoin = value; }
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
        CoinName cName = CoinName.NONE;

        if (coinText != null)
        {
            string text = coinText.text;
            cName = (CoinName)System.Enum.Parse(typeof(CoinName), text);
        }
        else
            cName = CoinName.NEETCOIN;

        currentPanel.CurrentSelectCoin = CoinManager.Instance.GetCoinDictionary()[cName];

        if (currentPanel.PrevSelectCoin == cName)
            return;

        currentPanel.PrevSelectCoin = cName;

        UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_TRADE_GRAPH, cName);
        MarketManager.Instance.ChangeMarketInfo(cName);
        grapher = MarketManager.Instance.RenderLineGraph(cName, 4, 600);

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

        int maxAmount = (int)(player.CurrentProperty / _currentSelectCoin.MarketInfo.CurrentPrice);
        aField.text = maxAmount.ToString();
    }

    public void ClickOrderButton()
    {
        TradePanel currentPanel = UIManager.Instance.TradePanelUI;
        InputField aField = currentPanel.GetAmountField();
        Player player = GameManager.Instance.PlayerCharacter;
        int amount = int.Parse(aField.text);
        float price = currentPanel.CurrentSelectCoin.MarketInfo.CurrentPrice * amount;

        if (player.CurrentProperty < price || price == 0)
            return;

        player.CurrentProperty -= price;
        Coin target = CoinManager.Instance.GetCoinDictionary()[currentPanel.CurrentSelectCoin.Name];
        player.DicCoin.Add(currentPanel.CurrentSelectCoin.Name, new Coin(target));
        player.DicCoin[currentPanel.CurrentSelectCoin.Name].CoinAmount += amount;
        aField.text = "0";
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

        if(amount < 0)
        {
            aField.text = "0";
            return;
        }

        if (price > player.CurrentProperty)
        {
            int maxAmount = (int)(player.CurrentProperty / _currentSelectCoin.MarketInfo.CurrentPrice);
            aField.text = maxAmount.ToString();
            return;
        }
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
}
