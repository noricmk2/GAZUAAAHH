using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    Image resultImage;
    Button returnButton;
    Animation UIAnimation;
    string clipName = "ResultPanelAppear";
    Sprite winSprite;
    Sprite loseSprite;

    public void ResultPanelInit()
    {
        resultImage = UIManager.Instance.ResultPanelUI.transform.GetChild(1).GetComponent<Image>();
        returnButton = UIManager.Instance.ResultPanelUI.GetComponentInChildren<Button>();
        UIAnimation = UIManager.Instance.ResultPanelUI.GetComponent<Animation>();

        winSprite = Resources.Load<Sprite>(ConstValue.WinSpritePath);
        loseSprite = Resources.Load<Sprite>(ConstValue.LoseSpritePath);
    }

    public void PlayDisappearAnimation() //UI가 사라질때의 애니메이션
    {
        UIAnimation[clipName].speed = -1;
        UIAnimation[clipName].time = UIAnimation[clipName].length;
        UIAnimation.Play(clipName);
    }

    public void PlayAppearAnimation() //UI가 나타날때의 애니메이션
    {
        UIAnimation[clipName].speed = 1;
        UIAnimation[clipName].time = 0;
        UIAnimation.Play(clipName);
    }

    public void ClickReturnButton()
    {
        StartCoroutine("ReturnToRobby");
    }

    IEnumerator ReturnToRobby()
    {
        PlayDisappearAnimation();

        yield return new WaitForSeconds(1);

        GameManager.Instance.PlayerCharacter.SetView(false);
        GameManager.Instance.PlayerCharacter.BattleEnd();
        Destroy(BattleManager.Instance.StageMap);
        Destroy(BattleManager.Instance.EnemyCharacter.gameObject);
        UIManager.Instance.SetSceneUI(UIType.TYPE_UI_LOBBY);

    }

    public void SetResultImage(ResultType type)
    {
        resultImage = UIManager.Instance.ResultPanelUI.transform.GetChild(1).GetComponent<Image>();
        switch (type)
        {
            case ResultType.TYPE_RESULT_WIN:
                resultImage.sprite = winSprite;
                break;
            case ResultType.TYPE_RESULT_DEFEAT:
                resultImage.sprite = loseSprite;
                break;
        }
    }
}
