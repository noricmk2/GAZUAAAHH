using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler , IDragHandler, IEndDragHandler
{
    GameObject dragObject = null; //원본과는 별개로 드래그 애니메이션을 보여줄 원본의 복제
    Image[] orgImage = null; //원본의 ImageUI
    Text orgText = null; //원본의 TextUI
    Transform CanvasTrans; //드래그될 복제가 움직일 화면

    private void Awake()
    {
        CanvasTrans = GameObject.FindGameObjectWithTag("Canvas").transform; //현재 화면상의 캔버스를 찾아온다
        orgImage = this.GetComponentsInChildren<Image>();
        orgText = this.transform.Find("ViewText").GetComponent<Text>();
    }

    public void OnBeginDrag(PointerEventData eventData) //드래그가 시작되면 복제본을 프리팹에서 불러오고 원본의 이미지와 텍스트를 비활성화
    {
        //원본을 복제하여 자신의 부모에게 붙여 초기위치를 정한뒤, 캔버스내의 자유로운 이동을 위해 캔버스로 부모를 바꿔줌
        dragObject = Instantiate(this.gameObject, this.transform.parent);
        dragObject.transform.SetParent(CanvasTrans);

        //드래그시 레이캐스트가 드래그 오브젝트에게 막히는걸 방지
        dragObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

        //드래그 원본의 이미지와 텍스트 비활성화
        if (orgImage != null)
        {
            for(int i=0; i<orgImage.Length; ++i)
                orgImage[i].enabled = false;
        }
        if(orgText != null)
            orgText.enabled = false;
    }

    public void OnDrag(PointerEventData eventData) //포인터의 포지션대로 드래그 오브젝트의 포지션을 변경
    {
        dragObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) //드래그가 끝날시 복제된 오브젝트를 삭제하고 원본의 이미지와 텍스트를 재활성화
    {
        dragObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        //원본의 이미지와 텍스트 재활성화
        Destroy(dragObject);
        if (orgImage != null)
        {
            for (int i = 0; i < orgImage.Length; ++i)
                orgImage[i].enabled = true;
        }
        if (orgText != null)
            orgText.enabled = true;
    }
}
