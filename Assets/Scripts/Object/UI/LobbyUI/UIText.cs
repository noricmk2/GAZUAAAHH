using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : BaseObject {
    Text text;

    public override void Init()
    {
        text = gameObject.GetComponent<Text>();
    }

    public override void CustomUpdate()
    {
        text.text = "자산\n" + GameManager.Instance.PlayerCharacter.CurrentProperty;
    }
}
