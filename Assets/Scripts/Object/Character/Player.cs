using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    //임시/////////////////////////////////////////////////////////////////////////////////////////////////////
    public override void SelectCoin() //드로우한 코인중 사용할 코인 선택
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            int cnt2 = 0;
            int cnt = 0;
            foreach (Coin coin in listDrawCoins)
            {
                if(cnt == cnt2)
                {
                    listSelectCoins.Add(coin);
                }
                cnt++;
            }
        }
        
        if(listSelectCoins.Count>=3)
        {
            isSelected = true;
        }
       
    }

    
    ////////////////////////////////////////////////////////////////////////
}
