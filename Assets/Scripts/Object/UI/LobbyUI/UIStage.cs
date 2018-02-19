using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStage : BaseObject
{
    Image image;
    public bool isClear { get; set; }
    public BattleEnterUI battleEnter;
    public Stage stage;
    Color orgColor;
    //bool isSelect;

    public override void Init()
    {
        image = gameObject.GetComponent<Image>();
        image.color = new Color(0.7f, 0.7f, 0.7f);
        isClear = false;
        //isSelect = false;
    }

    public void Update()
    {
       if (isClear)
            image.CrossFadeAlpha(0.3f, 1f, true);

       
    }

    public void BattleEnter()
    {
        if (isClear == false)
        {
            battleEnter.OnBattleEnter(stage); //호출할 스테이지의 정보전달 필요    
        }
    }

    public void HighlightOn()
    {
        orgColor = image.color;
        image.color = new Color(1,1,1);
    }

    public void HighlightOff()
    {
        image.color = orgColor;
    }

}
