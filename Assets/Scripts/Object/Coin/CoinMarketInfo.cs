using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMarketInfo
{
    List<float> listPrice = new List<float>();

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

    public float PrevPrice //변동전 가격
    {
        get; set;
    }

    public float DifferPrice //변동전 가격과의 시세차
    {
        get; set;
    }

    public float MinFlucRange //시세의 최저 변동폭
    {
        get; set;
    }

    public float MaxFlucRange //시세의 최고 변동폭
    {
        get; set;
    }

    public MicroCoinTrend CurrentMicroTrend //현재 미시적 가격변동의 추세
    {
        get; set;
    }

    public MacroCoinTrend CurrentMacroTrend //현재 거시적 가격변동의 추세
    {
        get; set;
    }

    public CoinMarketInfo(float price) //가격을 받아와서 세팅하는 생성자
    {
        _currentPrice = price;
        PrevPrice = price;
        listPrice.Add(price);
        SetFluctuationRange();
        CurrentMacroTrend = MacroCoinTrend.TREND_EQUAL_MACRO;
        CurrentMicroTrend = MicroCoinTrend.TREND_EQUAL_MICRO;
    }

    void SetFluctuationRange() //코인의 시세에 따라 변동폭을 조정하는 메서드
    {
        int digit = Util.CalculDigit((int)_currentPrice);

        switch (digit)
        {
            case 1:
            case 2:
            case 3:
                MinFlucRange = 0;
                MaxFlucRange = _currentPrice + 200;
                break;
            case 4:
                MinFlucRange = _currentPrice - 1000;
                MaxFlucRange = _currentPrice + 1000;
                break;
            case 5:
                MinFlucRange = _currentPrice - 10000;
                MaxFlucRange = _currentPrice + 10000;
                break;
            case 6:
                MinFlucRange = _currentPrice - 100000;
                MaxFlucRange = _currentPrice + 100000;
                break;
            case 7:
                MinFlucRange = _currentPrice - 500000;
                MaxFlucRange = _currentPrice + 500000;
                break;
        }

        if (MinFlucRange < 0)
            MinFlucRange = 0;
    }

    public void AddPriceList(float value)
    {
        listPrice.Add(value);
    }

    public List<float> GetPriceList()
    {
        return listPrice;
    }
}
