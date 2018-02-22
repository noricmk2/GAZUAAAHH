using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Enemy : Character
{
    public override void Init()
    {
        listSelectCoins = new List<Coin>();
        DicCoin = new Dictionary<CoinName, Coin>(); // 임시
        characterType = CharacterType.TYPE_ENEMY1;
        base.Init();
        CoinSetting();
    }

    public override void SelectCoin() //사용할 코인 선택
    {
        cost = BattleManager.Instance.CurrentTurnCost;
        float attackRate = Random.Range(0.2f,0.8f); //공격 비중을 랜덤으로 지정
        float deffenceRate = (1 - attackRate); // 공격 비중에 따라 방어 비중 지정
        float attackCost = cost* attackRate; 
        float deffenceCost = cost * deffenceRate;


        foreach (KeyValuePair<CoinName, Coin> pair in DicCoin)
        {
            Coin coin = pair.Value;
            int aa = Random.Range(0, 5); //코인을 방어에 쓸지 공격에 쓸지 랜덤으로 선택
            float price = coin.MarketInfo.CurrentPrice;
            int maxAmount;
            int selectAmount;

            if (aa >2)
            {              
                if (price > attackCost)
                    continue;
                else
                {
                    coin.BattleType = CoinBattleType.TYPE_ATTACK_COIN;
                    maxAmount = (int)attackCost / (int)price;
                    selectAmount = Random.Range(maxAmount/3, maxAmount);
                    coin.CoinAmountInBattle = selectAmount;
                    attackCost -= coin.CoinAmountInBattle*price;
                }
            }
            else
            {
                if (price > deffenceCost)
                    continue;
                else
                {
                    coin.BattleType = CoinBattleType.TYPE_DEFFENCE_COIN;
                    maxAmount = (int)attackCost / (int)price;
                    selectAmount = Random.Range(maxAmount / 3, maxAmount);
                    coin.CoinAmountInBattle = selectAmount;
                    deffenceCost -= coin.CoinAmountInBattle * price;
                }
            }

        }

    }
    
    public void CoinSetting()
    {
        TextAsset coinText = Resources.Load(ConstValue.EnemyCoinPath) as TextAsset;
        if (coinText != null)
        {
            JSONNode rootNode = JSON.Parse(coinText.text);
            string nodePath = ""; 
            switch (characterType)
            {              
                case CharacterType.TYPE_ENEMY1:
                    nodePath ="ENEMY1_COIN_TEMPLATE";
                    break;
                case CharacterType.TYPE_ENEMY2:
                    nodePath = "ENEMY2_COIN_TEMPLATE";
                    break;
                case CharacterType.TYPE_ENEMY3:
                    nodePath = "ENEMY3_COIN_TEMPLATE";
                    break;
                default:
                    break;
            }
            JSONNode coinTemPlateNode = rootNode[nodePath];
            foreach (KeyValuePair<string, JSONNode> templateNode in coinTemPlateNode)
            {
                CoinName name = (CoinName)System.Enum.Parse(typeof(CoinName), templateNode.Key);
                Coin coin = new Coin(CoinManager.Instance.GetCoinDictionary()[name]);
                int valueData = (int)templateNode.Value["Amount"];
                coin.CoinAmount = valueData;
                DicCoin.Add(name,coin);
            }

        }

    }

}   


