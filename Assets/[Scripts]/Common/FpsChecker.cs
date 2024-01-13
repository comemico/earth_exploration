using UnityEngine;
using UnityEngine.UI;

public class FpsChecker : MonoBehaviour
{
    /*
    public Text fpsText;

    // Update()が呼ばれた回数をカウントします。
    int frameCount;

    // 前回フレームレートを表示してからの経過時間です。
    float elapsedTime;

    void Update()
    {
        // 呼ばれた回数を加算します。
        frameCount++;

        // 前のフレームからの経過時間を加算します。
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1.0f)
        {
            // 経過時間が1秒を超えていたら、フレームレートを計算します。
            float fps = 1.0f * frameCount / elapsedTime;

            // 計算したフレームレートを画面に表示します。(小数点以下2ケタまで)
            string fpsRate = $"FPS: {fps.ToString("F2")}";
            fpsText.text = fpsRate;

            // フレームのカウントと経過時間を初期化します。
            frameCount = 0;
            elapsedTime = 0f;
        }
    }
    */

    /// <summary>
    /// Reflect measurement results every 'EveryCalcurationTime' seconds.
    /// EveryCalcurationTime 秒ごとに計測結果を反映する
    /// </summary>
    [SerializeField, Range(0.1f, 1.0f)]
    float EveryCalcurationTime = 0.5f;
    Text fpsText;

    /// <summary>
    /// FPS value
    /// </summary>
    public float Fps
    {
        get; private set;
    }

    int frameCount;
    float prevTime;

    void Start()
    {
        fpsText = GetComponent<Text>();
        frameCount = 0;
        prevTime = 0.0f;
        Fps = 0.0f;
    }
    void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        // n秒ごとに計測
        if (time >= EveryCalcurationTime)
        {
            Fps = frameCount / time;
            string fpsRate = $"FPS: {Fps.ToString("F2")}";
            fpsText.text = fpsRate;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }



}
