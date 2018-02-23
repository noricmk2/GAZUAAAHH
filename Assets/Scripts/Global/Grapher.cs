using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grapher : MonoBehaviour
{
    static Material lineMaterial = null; //그래프를 그릴 선의 마테리얼
    List<Vector3> listPoint = null; //그래프의 정점 리스트

    float UIMin = 0; //그래프를 그릴 UI높이의 최소값
    float UIMax = 0; //그래프를 그릴 UI높이의 최대값
    int pointCount = 40; //그래프를 구성할 정점의 총합

    float interval = 4;
    public float Interval //그래프 각 점의 간격
    {
        get { return interval; }
        set { interval = value; }
    }

    float _lineWidth = 600.0f; 
    public float LineWidth //선의 굵기
    {
        get { return _lineWidth; }
        set { _lineWidth = value; }
    }

    public bool SetDraw //그래프를 그릴지의 여부
    {
        get; set;
    }

    static void CreateLineMaterial() //마테리얼 생성
    {
        if (lineMaterial == null)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void OnRenderObject() //GL을 그려주는 콜백메서드
    {
        if (SetDraw)
        {
            //정점정보가 없거나 정점수가 선을 그리기 부족한 2 미만이라면 리턴
            if (listPoint == null || listPoint.Count < 2)
                return;

            //GL의 렌더링 코드
            CreateLineMaterial();

            lineMaterial.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            //스크린의 크기에 비례하여 굵기를 조정한다
            float thisWidth = 1.0f / Screen.width * _lineWidth * 0.5f;

            GL.Begin(GL.QUADS);
            GL.Color(Color.red);

            //정점리스트를 순회하며 그래프를 그린다
            for (int i = 0; i < listPoint.Count; ++i)
            {
                if (i == listPoint.Count - 1)
                    break;
                DrawLine2D(listPoint[i], listPoint[i + 1], thisWidth);
            }

            GL.End();
            GL.PopMatrix();
        }
    }

    void DrawLine2D(Vector3 v0, Vector3 v1, float lineWidth) //실제로 선을 그리는 메서드
    {
        Vector3 n = (new Vector3(v1.y, v0.x, 0) - new Vector3(v0.y, v1.x, 0)).normalized * lineWidth;
        GL.Vertex3(v0.x - n.x, v0.y - n.y, 0.0f); 
        GL.Vertex3(v0.x + n.x, v0.y + n.y, 0.0f);
        GL.Vertex3(v1.x + n.x, v1.y + n.y, 0.0f);
        GL.Vertex3(v1.x - n.x, v1.y - n.y, 0.0f);
    }

    public void RenewValue(object value, GraphTarget targetType) //정점리스트를 갱신하는 메서드
    {
        switch (targetType)
        {
            case GraphTarget.GRAPH_COIN:
                {
                    if (listPoint == null)
                    {
                        Debug.LogError("need set point list");
                        return;
                    }

                    //리스트에 있는 정점의 수가 지정한 정점의 총합보다 커지면 선두에 있는 정점을 제거하고 각 정점의 x위치를 재조정
                    if (listPoint.Count > pointCount)
                    {
                        int removeCount = listPoint.Count - pointCount;
                        Vector3[] tempVector = new Vector3[pointCount];

                        for (int i = removeCount; i < listPoint.Count; ++i)
                        {
                            int arrIndex = i - removeCount;
                            tempVector[arrIndex] = listPoint[i - 1];
                            tempVector[arrIndex].x = listPoint[arrIndex].x;
                        }
                        listPoint.Clear();

                        for (int i = 0; i < tempVector.Length; ++i)
                        {
                            listPoint.Add(tempVector[i]);
                        }
                    }

                    CoinMarketInfo cmInfo = value as CoinMarketInfo;

                    //매개변수로 받아온 갱신값을 UI의 크기에 맞는 값으로 조정
                    Vector3 tempPos = listPoint[listPoint.Count - 1];
                    float per = Mathf.InverseLerp(cmInfo.MinFlucRange, cmInfo.MaxFlucRange, cmInfo.CurrentPrice);
                    tempPos.y = Mathf.Lerp(UIMin, UIMax, per);
                    tempPos.x += interval;

                    listPoint.Add(tempPos);
                }
                break;
        }
    }

    public void SetGraphTarget(GraphTarget targetType, object info, Transform renderTarget) //무엇에 대한 그래프를 그릴지 세팅하는 메서드
    {
        switch (targetType)
        {
            case GraphTarget.GRAPH_COIN:
                {
                    Coin coin = info as Coin;
                    //그래프를 그릴 대상에 그려질 UI정보를 세팅
                    coin.GraphUI = renderTarget.GetComponent<Image>();

                    //UI에 대한 정보들을 세팅
                    CoinMarketInfo cmInfo = coin.MarketInfo;
                    RectTransform rectTrans = renderTarget as RectTransform;
                    UIMin = rectTrans.rect.height * -0.5f;
                    UIMax = rectTrans.rect.height * 0.5f;
                    pointCount = (int)(rectTrans.rect.width / interval);

                    listPoint = new List<Vector3>();
                    List<float> priceList = cmInfo.GetPriceList();

                    //정점리스트를 초기화
                    Vector3 tempPos = rectTrans.position;
                    tempPos.x = rectTrans.rect.width * -0.5f;

                    for (int i =0; i < priceList.Count; ++i)
                    {
                        float per = Mathf.InverseLerp(cmInfo.MinFlucRange, cmInfo.MaxFlucRange, priceList[i]);
                        tempPos.y = Mathf.Lerp(UIMin, UIMax, per);
                        listPoint.Add(tempPos);
                        tempPos.x += interval;
                    }
                }
                break;
        }
    }

    public Vector3 GetLastGraphPosition()
    {
        if (listPoint.Count > 0)
            return listPoint[listPoint.Count - 1];
        else
        {
            Debug.LogError("list is empty");
            return Vector3.zero;
        }
    }
}
