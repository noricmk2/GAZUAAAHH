using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoImage : MonoBehaviour
{
    Text titleText;
    Text continueText;
    float alphaTime = 0;
    bool alphaIncre = true;

    public void LogoInit()
    {
        titleText = UIManager.Instance.LogoImageUI.transform.GetChild(0).GetComponent<Text>();
        continueText = UIManager.Instance.LogoImageUI.transform.GetChild(1).GetComponent<Text>();
    }

    private void Update()
    {
        alphaTime += Time.deltaTime;

        if (alphaTime >= 1)
        {
            alphaTime = 0;
            alphaIncre = !alphaIncre;
        }

        if (continueText != null)
        {
            if (alphaIncre)
            {
                Color alpha = continueText.color;
                alpha.a -= Time.deltaTime;
                continueText.color = alpha;
            }
            else
            {
                Color alpha = continueText.color;
                alpha.a += Time.deltaTime;
                continueText.color = alpha;
            }
        }

        if(Input.GetMouseButtonDown(0) == true)
        {
            UIManager.Instance.SetSceneUI(UIType.TYPE_UI_LOBBY);
        }
    }
}
