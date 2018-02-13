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
    Toggle autoToggle;
    Button maxButton;
    Button orderButton;

    float deltaTime = 0;
    float renewTime = 2.0f;

    CoinName prevSelectCoin = CoinName.NONE;

    public bool TradeActive
    {
        get; set;
    }

    private void Awake()
    {
        coinScrollContent = this.transform.GetChild(0).GetChild(0).GetChild(0);
        graphImage = this.transform.GetChild(1).GetComponent<Image>();
        orderButton = this.transform.GetChild(2).GetComponent<Button>();
        priceField = this.transform.GetChild(3).GetComponent<InputField>();
        amountField = this.transform.GetChild(4).GetComponent<InputField>();
        autoToggle = this.transform.GetChild(5).GetComponent<Toggle>();
        maxButton = this.transform.GetChild(6).GetComponent<Button>();
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

    public void TradePanelInit()
    {
        CoinToggleSelect(null);
    }

    public void CoinToggleSelect(Text coinText)
    {
        Grapher grapher = null;
        CoinName cName = CoinName.NONE;

        if (coinText != null)
        {
            string text = coinText.text;
            cName = (CoinName)System.Enum.Parse(typeof(CoinName), text);
        }
        else
            cName = CoinName.NEETCOIN;

        if (prevSelectCoin == cName)
            return;

        prevSelectCoin = cName;

        UIManager.Instance.SetGraphUI(GraphType.TYPE_IN_TRADE_GRAPH, cName);
        MarketManager.Instance.ChangeMarketInfo(cName);
        grapher = MarketManager.Instance.RenderLineGraph(cName, 4, 600);

        Vector3 graphPos = grapher.GetLastGraphPosition();
        if (graphPos != Vector3.zero)
        {
            Image graph = UIManager.Instance.TradePanelUI.GetGraphImage();

            Text currentPrice = graph.transform.GetChild(0).GetComponent<Text>();
            Vector2 pricePos = ((RectTransform)currentPrice.transform).anchoredPosition;
            pricePos.y = graphPos.y;
            ((RectTransform)currentPrice.transform).anchoredPosition = pricePos;
            currentPrice.text = CoinManager.Instance.GetCoinDictionary()[cName].MarketInfo.CurrentPrice.ToString();

            Text maxPrice = graph.transform.GetChild(1).GetComponent<Text>();
            Text minPrice = graph.transform.GetChild(2).GetComponent<Text>();

            maxPrice.text = CoinManager.Instance.GetCoinDictionary()[cName].MarketInfo.MaxFlucRange.ToString();
            minPrice.text = CoinManager.Instance.GetCoinDictionary()[cName].MarketInfo.MinFlucRange.ToString();
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

    public Toggle GetAutoToggle()
    {
        if (autoToggle == null)
        {
            Debug.LogError("AutoToggle is not exist");
            return null;
        }
        return autoToggle;
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
