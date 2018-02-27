using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour, IDropHandler
{
    GameObject coinSetPanel; //드롭 이벤트시 띄워야하는 UI의 오브젝트
    public ChoicePanelType panelType; //선택패널의 타입을 나타내는 변수

    private void Awake()
    {
        //띄워야할 대상의 오브젝트를 찾아온다
        coinSetPanel = transform.parent.parent.Find("CoinSetPanel").gameObject;
        if(coinSetPanel == null)
        {
            Debug.LogError("panel not found");
        }
    }

    public void OnDrop(PointerEventData eventData) //드롭이벤트가 발생하면 타입에 맞게 패널을 띄워줌
    {
        Text titleText = coinSetPanel.transform.Find("PanelTitle").GetComponent<Text>();
        GameObject droppedObj = eventData.pointerDrag;
        Text coinTextInfo = droppedObj.GetComponentInChildren<Text>();
        Image coinSelectImage = droppedObj.transform.GetChild(0).GetComponent<Image>();
        coinSetPanel.GetComponent<CoinSetPanel>().coinItemSelectImage = coinSelectImage;

        //자신의 패널타입에 따라 띄우는 패널의 텍스트와 어느쪽에서 불렀는지 정보를 넣어준다
        switch (panelType)
        {
            case ChoicePanelType.TYPE_ATTACK_PANEL:
                coinSetPanel.GetComponent<CoinSetPanel>().PanelType = ChoicePanelType.TYPE_ATTACK_PANEL;
                titleText.text = "Attack Set";
                break;
            case ChoicePanelType.TYPE_DEFFENCE_PANEL:
                coinSetPanel.GetComponent<CoinSetPanel>().PanelType = ChoicePanelType.TYPE_DEFFENCE_PANEL;
                titleText.text = "Deffence Set";
                break;
        }

        //띄우는 패널을 초기화후 활성화
        coinSetPanel.GetComponent<CoinSetPanel>().PanelInit(coinTextInfo.text);
        coinSetPanel.SetActive(true);
    }
}
