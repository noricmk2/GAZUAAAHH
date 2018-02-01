using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : MonoBehaviour
{
    static Material lineMaterial = null;

    List<Vector3> listPoint = null;

    float _lineWidth = 20.0f;
    public float LineWidth
    {
        get { return _lineWidth; }
        set { _lineWidth = value; }
    }

    public bool SetDraw
    {
        get; set;
    }

    static void CreateLineMaterial()
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

    public void OnRenderObject()
    {
        if (SetDraw)
        {
            if (listPoint == null || listPoint.Count < 2)
                return;

            CreateLineMaterial();

            lineMaterial.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            float thisWidth = 1.0f / Screen.width * _lineWidth * 0.5f;

            GL.Begin(GL.QUADS);
            GL.Color(Color.red);

            for(int i=0; i<listPoint.Count; ++i)
            {
                DrawLine2D(listPoint[i], listPoint[i + 1], thisWidth);
            }

            //DrawLine2D(new Vector3(0, 1, 0), new Vector3(0, 2, 0), thisWidth);
            //DrawLine2D(new Vector3(0, 2, 0), new Vector3(1, 3, 0), thisWidth);
            //DrawLine2D(new Vector3(1, 3, 0), new Vector3(2, 4, 0), thisWidth);
            //DrawLine2D(new Vector3(2, 4, 0), new Vector3(3, 6, 0), thisWidth);
            GL.End();
            GL.PopMatrix();
        }
    }

    void DrawLine2D(Vector3 v0, Vector3 v1, float lineWidth)
    {
        Vector3 n = (new Vector3(v1.y, v0.x, 0) - new Vector3(v0.y, v1.x, 0)).normalized * lineWidth;
        GL.Vertex3(v0.x - n.x, v0.y - n.y, 0.0f);
        GL.Vertex3(v0.x + n.x, v0.y + n.y, 0.0f);
        GL.Vertex3(v1.x + n.x, v1.y + n.y, 0.0f);
        GL.Vertex3(v1.x - n.x, v1.y - n.y, 0.0f);
    }

    public void SetPosition(Vector3 pos)
    {
        if(listPoint == null)
        {
            Debug.LogError("need set point list");
            return;
        }

        if (listPoint.Count > 15)
        {
            listPoint.RemoveAt(0);
        }

        listPoint.Add(pos);
    }

    public void SetGraphTarget()
    {
    }
}
