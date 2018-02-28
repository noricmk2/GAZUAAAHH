using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class CoinManager : MonoSingleton<CoinManager>
{
    Dictionary<CoinName, Coin> DicCoin = new Dictionary<CoinName, Coin>();
    Dictionary<string, CoinTemplateData> dicTemplateData = new Dictionary<string, CoinTemplateData>(); //코인정보를 외부에서 받아올 dictionary

    public void CoinInit() //코인 정보 초기화
    {
        TextAsset coinText = Resources.Load(ConstValue.coinpath) as TextAsset;
        if (coinText != null)
        {
            JSONNode rootNode = JSON.Parse(coinText.text);
            if (rootNode != null)
            {
                JSONNode coinTemPlateNode = rootNode["COIN_TEMPLATE"];

                foreach (KeyValuePair<string, JSONNode> templateNode in coinTemPlateNode)
                {
                    dicTemplateData.Add(templateNode.Key, new CoinTemplateData(templateNode.Key, templateNode.Value));
                }
            }
        }
        foreach (KeyValuePair<string, CoinTemplateData> pair in dicTemplateData)
        {
            Coin coin = CreateCoin(pair.Key, pair.Value);
            DicCoin.Add(coin.Name, coin);
        }
      
    }

    public Coin CreateCoin(string name, CoinTemplateData coinData) //이름을 받아오면 외부에서 읽어온 데이터를 토대로 코인 생성
    {
        Coin coin = null;
        CoinName coinName = (CoinName)System.Enum.Parse(typeof(CoinName), name);

        coin = new Coin(coinData.dicCoinInfo[CoinInfo.Price], coinName, SkillManager.Instance.SkillInit((SkillType)coinData.dicCoinInfo[CoinInfo.Skill]),coinData.shortName);
        coin.CoinSkill.Init(coin);
        coin.CoinAmount = (int)coinData.dicCoinInfo[CoinInfo.Amount];

        return coin;
    }

    public Dictionary<CoinName, Coin> GetCoinDictionary() //코인딕셔너리의 getter
    {
        return DicCoin;
    }
}
