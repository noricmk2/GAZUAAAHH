using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketButton : BaseObject {  

    public void MarketView()
    {
        LobbyCanvas lobby = GetComponentInParent<LobbyCanvas>();
        //lobby.LobbyOut();
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_TRADE);
    }

}
