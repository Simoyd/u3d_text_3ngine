// <license file="scroll_test.cs" repository="https://github.com/DrizzlyBear/u3d_text_3ngine">
//     MIT License https://github.com/DrizzlyBear/u3d_text_3ngine/blob/master/LICENSE
// </license>

using System.Linq;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

public class scroll_test : MonoBehaviour
{
    enum TestMode
    {
        Start,
        HoldTop,
        DownSwing,
        HoldBottom,
        UpSwing,
    }

    private const double percentFull = 1.0;

    private static System.Random r = new System.Random();
    public static double linesPerSecond = 1;

    private u3d_text_3ngine engine;
    private Stopwatch stopwatch;
    private double accumulator = 0;
    private TestMode mode = TestMode.Start;

    private long msSinceChange = 0;
    private int lastCorrCount = 0;

    // Use this for initialization
    void Start()
    {
        engine = GetComponent<u3d_text_3ngine>();
        stopwatch = new Stopwatch();
        stopwatch.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        long elapsed = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();
        stopwatch.Start();

        accumulator += elapsed;

        TestMode orgMode = mode;

        switch (mode)
        {
            case TestMode.Start:
                {
                    ++linesPerSecond;

                    if (engine.CorruptedLines > 0)
                    {
                        mode = TestMode.HoldTop;
                    }
                }
                break;
            case TestMode.HoldTop:
                {
                    if (engine.CorruptedLines == 0)
                    {
                        mode = TestMode.UpSwing;
                    }

                    if (msSinceChange >= 2000)
                    {
                        mode = TestMode.DownSwing;
                    }
                }
                break;
            case TestMode.DownSwing:
                {
                    --linesPerSecond;

                    if ((engine.CorruptedLines == 0) || (engine.CorruptedLines < lastCorrCount))
                    {
                        mode = TestMode.HoldBottom;
                    }
                }
                break;
            case TestMode.HoldBottom:
                {
                    if ((engine.CorruptedLines > 0) && (engine.CorruptedLines >= lastCorrCount))
                    {
                        mode = TestMode.DownSwing;
                    }

                    if (msSinceChange >= 5000)
                    {
                        mode = TestMode.UpSwing;
                    }
                }
                break;
            case TestMode.UpSwing:
                {
                    ++linesPerSecond;

                    if (engine.CorruptedLines > 0)
                    {
                        mode = TestMode.HoldTop;
                    }
                }
                break;
        }

        if (linesPerSecond < 1)
        {
            linesPerSecond = 1;
        }

        lastCorrCount = engine.CorruptedLines;

        if (orgMode != mode)
        {
            msSinceChange = 0;
        }
        else
        {
            msSinceChange += elapsed;
        }

        double msPerLine = 1000.0 / linesPerSecond;

        while (accumulator > msPerLine)
        {
            // TODO: put color in this test string, once color parsing is done in engine
            string newLine = string.Join("", Enumerable.Range(0, engine.WidthChars).Select(cur => r.NextDouble() <= percentFull ? string.Format("`{0}{1}`", u3d_text_3ngine.HackmudColors.ElementAt(r.Next(u3d_text_3ngine.HackmudColors.Count)).Key, (char)(r.Next(26) + 'a')) : " ").ToArray());

            engine.DisplayText = engine.DisplayText.Skip(1).Concat(new string[] { newLine }).ToArray();

            accumulator -= msPerLine;
        }
    }
}
