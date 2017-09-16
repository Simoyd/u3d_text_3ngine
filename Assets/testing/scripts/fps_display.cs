// This scripts is stolen and modified from here: http://wiki.unity3d.com/index.php?title=FramesPerSecond

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Script used to display an FPS counter and other debug info on the screen
/// </summary>
public class fps_display : MonoBehaviour
{
    /// <summary>
    /// Number of frames to average for FPS
    /// </summary>
    const int frameAverage = 120;

    /// <summary>
    /// Rect used to display the fps counter
    /// </summary>
    Rect rect = new Rect(0, 0, 0, 0);

    /// <summary>
    /// Style used for the FPS counter GUI
    /// </summary>
    GUIStyle style = new GUIStyle();

    /// <summary>
    /// Queue to hold multiple frame times for averaging
    /// </summary>
    Queue<float> frameTimes = new Queue<float>();

    /// <summary>
    /// Use this for initialization, called once per instantiation
    /// </summary>
    void Start()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    }

    /// <summary>
    /// OnGUI is potentially called multiple times per frame
    /// </summary>
    void OnGUI()
    {
        // Adjust for the new screen size
        int w = Screen.width;
        int h = Screen.height;

        rect.width = w;
        rect.height = h * 2 / 25;
        style.fontSize = h * 2 / 25;

        // Construct the string to display
        frameTimes.Enqueue(Time.deltaTime);
        while (frameTimes.Count > frameAverage)
        {
            frameTimes.Dequeue();
        }

        float fps = 1.0f / frameTimes.Average();
        string text = string.Format("{0:0.} fps\nChars: {1}\nHeight: {2}\nLines: {3} / {4}", 
                                    fps,
                                    u3d_text_3ngine.TotalChars.Sum(cur => cur.Value),
                                    scroll_test.height, scroll_test.linesPerSecond,
                                    scroll_test.height * 60);

        // Send it to Unity to display
        GUI.Label(rect, text, style);
    }
}