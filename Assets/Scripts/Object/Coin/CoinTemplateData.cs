using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class CoinTemplateData {

    string _strKey = string.Empty;
    public string strKey { get { return _strKey; } }

    Dictionary<CoinInfo, float> _dicCoinInfo = new Dictionary<CoinInfo, float>();
    public Dictionary<CoinInfo, float> dicCoinInfo { get { return _dicCoinInfo; } } 

    public CoinTemplateData(string strKey, JSONNode nodeData)
    {
        _strKey = strKey;

        for(int i = 0;i<(int)CoinInfo.MAX;i++)
        {          
            CoinInfo coinInfo = (CoinInfo)i;
            float valueData = nodeData[coinInfo.ToString("F")].AsFloat;
            _dicCoinInfo[coinInfo] = valueData;
            //Amount 안들어가는거 확인 후 수정

        }
    } 
}
