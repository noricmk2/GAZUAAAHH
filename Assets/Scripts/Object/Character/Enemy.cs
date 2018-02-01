using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public override void SelectCoin() //드로우한 코인중 사용할 코인 선택
    {
        int[] arr = new int[3];
        int cnt = 0;

        for (int i = 0; i < 3; i++)
        {
            arr[i] = Random.Range(0, listDrawCoins.Count);
        }
 
       foreach (Coin coin in listDrawCoins)
       {         
            //랜덤으로 3개 선택(임시) 
            for (int i = 0; i < 3; i++)
            {
                if(cnt == arr[i])
                {
                    listSelectCoins.Add(coin);
                }
            }         
            cnt++;                             
       }
       

       
    }   

}
