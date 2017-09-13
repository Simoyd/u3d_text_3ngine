// This scripts is stolen and modified from here: http://wiki.unity3d.com/index.php?title=FramesPerSecond

using UnityEngine;
using System.Collections;

public class fps_display : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 25);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 25;
        style.normal.textColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps) - Total chars: {2}", msec, fps, u3d_text_3ngine.totalChars);
        GUI.Label(rect, text, style);
    }
}