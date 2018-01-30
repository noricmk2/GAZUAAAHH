using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMarketInfo
{
    float _currentPrice;

    public float CurrentPrice//현재 코인의 가격
    {
        get { return _currentPrice; }
        set
        {
            if (value < 0)
            {
                Debug.LogError("price can not set under zero"); //코인의 현재 가격이 0밑으로 내려가지 못하게 예외처리
            }
            else
                _currentPrice = value;
        }
    }

    public float PrevPrice
    {
        get; set;
    }

    public float DifferPrice //변동전 가격과의 시세차
    {
        get; set;
    }

    public CoinTrend CurrentTrend //현재 가격변동의 추세
    {
        get; set;
    }

    public CoinMarketInfo(float price) //가격을 받아와서 세팅하는 생성자
    {
        _currentPrice = price;
        PrevPrice = price;
    }
}
