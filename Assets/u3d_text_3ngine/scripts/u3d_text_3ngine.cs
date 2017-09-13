using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class u3d_text_3ngine : MonoBehaviour
{
    public static int totalChars = 0;

    private List<GameObject> guis = new List<GameObject>();

    string texta;
    string textb;
    bool yeah = false;
    GameObject template;
    bool calibrate = true;
    int screenWidthChars;

    // Use this for initialization
    void Start()
    {
        template = transform.GetChild(0).gameObject;
        template.GetComponents<MonoBehaviour>().ToList().ForEach(cur => cur.enabled = false);
    }

    Color32[] colors_d = new Color32[] {
        new Color32(  0,   0, 255, 255),
        new Color32(  0, 255,   0, 255),
        new Color32(  0, 255, 255, 255),
        new Color32(255,   0,   0, 255),
        new Color32(255,   0, 255, 255),
        new Color32(255, 255,   0, 255),
        new Color32(255, 255, 255, 255),
    };

    // Update is called once per frame
    void Update()
    {
        if (calibrate)
        {
            calibrate = false;

            RectTransform canvas = gameObject.GetComponent<RectTransform>();
            template.GetComponents<MonoBehaviour>().ToList().ForEach(cur => cur.enabled = true);

            TextMeshProUGUI templateGui = template.GetComponent<TextMeshProUGUI>();
            templateGui.ForceMeshUpdate();
            float charWidth = templateGui.textInfo.characterInfo[1].topLeft.x - templateGui.textInfo.characterInfo[0].topLeft.x;
            float charHeight = templateGui.textInfo.characterInfo[0].topLeft.y - templateGui.textInfo.characterInfo[0].bottomLeft.y;

            int maxWidth = 681; // 1010101010 - 1 ... not sure wtf this number is but any more causes exceptions....
            screenWidthChars = (int)Mathf.Floor(canvas.rect.width / charWidth) - 2;
            int screenHeightChars = (int)Mathf.Floor(canvas.rect.height / charHeight) - 2;

            totalChars = screenWidthChars * screenHeightChars;

            if (screenWidthChars > maxWidth)
            {
                throw new Exception("Big Problem!!!");
            }

            for (int i = 0; i < screenHeightChars; ++i)
            {
                float newY = charHeight * i;

                GameObject newObj = Instantiate(template, transform);
                RectTransform rectTrans = newObj.GetComponent<RectTransform>();
                rectTrans.sizeDelta = new Vector2(canvas.rect.width, charHeight);
                rectTrans.localPosition = new Vector3(charWidth, canvas.rect.height - (newY + canvas.rect.height/2 + charHeight/2) - charHeight);

                guis.Add(newObj);
            }

            template.GetComponents<MonoBehaviour>().ToList().ForEach(cur => cur.enabled = false);
        }

        Random r = new Random();

        foreach (GameObject guiObj in guis)
        {
            bool old_color = false;

            TextMeshProUGUI gui = guiObj.GetComponent<TextMeshProUGUI>();

            //int x = 47;
            //int y = 18;
            //int tot = x * y;

            StringBuilder sb = new StringBuilder();

            string color;

            if (old_color)
            {
                color = r.Next(16777215).ToString("X6");
                sb.AppendFormat("<color=#{0}>", color);
            }

            for (int i = 0; i < screenWidthChars; ++i)
            {
                if (old_color)
                {
                    if (((i + 1) % 1) == 0)
                    {
                        sb.Append("</color>");
                        color = r.Next(16777215).ToString("X6");
                        sb.AppendFormat("<color=#{0}>", color);
                    }
                }

                sb.AppendFormat("{0}", (char)(r.Next(26) + 'a'));
            }

            if (old_color)
            {
                sb.Append("</color>");
            }

            string newVal = sb.ToString();

            gui.SetText(newVal);
            gui.ForceMeshUpdate();

            if (!old_color)
            {
                CanvasRenderer renderer = guiObj.GetComponent<CanvasRenderer>();
                Mesh mesh = gui.textInfo.meshInfo[0].mesh;

                // IMPORTANT!!! mesh.vertices is O(n), NOT O(1)!!!!!
                int length = mesh.vertices.Length;
                Color32[] colors = new Color32[length];

                int curCol = r.Next(colors_d.Length);

                for (int i = 0; i < length / 4; ++i)
                {
                    ++curCol;
                    if (curCol == colors_d.Length)
                    {
                        curCol = 0;
                    }

                    int j = i * 4;
                    Color32 curColor = new Color32((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255), 255);
                    colors[j + 0] = curColor;
                    colors[j + 1] = curColor;
                    colors[j + 2] = curColor;
                    colors[j + 3] = curColor;
                }

                mesh.colors32 = colors;
                renderer.SetMesh(mesh);
            }
        }
    }
}
