using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
/// <remarks>The AddComponentMenu attribute here hides this script
/// from context menus because it's created programatically.</remarks>
[AddComponentMenu("")]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class bg_graphic : Graphic
{
    private static System.Random r = new System.Random();

    protected override void Start()
    {
        raycastTarget = false;

        base.Start();

        RectTransform rect = GetComponent<RectTransform>();

        // Get the parent rect
        RectTransform parentRect = null;
        Transform parentTransform = transform.parent;

        while (parentRect == null)
        {
            parentRect = parentTransform.gameObject.GetComponent<RectTransform>();
            parentTransform = parentTransform.parent;
        }

        // Size the rect to the size of the parent
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
    }

    public float charWidth { get; set; }

    public float charHeight { get; set; }

    /// <summary>
    /// Gets the width of the object in chars
    /// </summary>
    public int WidthChars { get; set; }

    /// <summary>
    /// Gets the height of the object in chars
    /// </summary>
    public int HeightChars { get; set; }

    public void Recalibrate()
    {
        Rebuild(CanvasUpdate.Layout);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        int curIndex = 0;

        // [CW]TODO: add the width instead of multiplying each time...
        for (int curY = 0; curY < HeightChars; ++curY)
        {
            for (int curX = 0; curX < WidthChars; ++curX)
            {
                float top = rectTransform.rect.height - (charHeight * curY + rectTransform.rect.height / 2);
                float left = (curX * charWidth) - (rectTransform.rect.width / 2);
                Color32 charColor = new Color32((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255), 255);

                Vector2 corner1 = new Vector2(left, top);
                Vector2 corner2 = new Vector2(left + charWidth, top - charHeight);

                UIVertex vert = UIVertex.simpleVert;

                int v0 = curIndex;
                vert.position = new Vector2(corner1.x, corner1.y);
                vert.color = charColor;
                vh.AddVert(vert);

                int v1 = ++curIndex;
                vert.position = new Vector2(corner1.x, corner2.y);
                vert.color = charColor;
                vh.AddVert(vert);

                int v2 = ++curIndex;
                vert.position = new Vector2(corner2.x, corner2.y);
                vert.color = charColor;
                vh.AddVert(vert);

                int v3 = ++curIndex;
                vert.position = new Vector2(corner2.x, corner1.y);
                vert.color = charColor;
                vh.AddVert(vert);

                ++curIndex;

                vh.AddTriangle(v0, v1, v2);
                vh.AddTriangle(v2, v3, v0);
            }
        }
    }
}
