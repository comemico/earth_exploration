using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class TimeManager : MonoBehaviour
{
    public Text timeText;
    public Text clearTimeText;
    public Text recordTimeText;
    public Text recordUpdateText;

    public float recordTime;
    private string stringRecordTime;
    public float currentTime;
    private bool isTimeStop = true;
    public float time = 0.0f;
    private int min = 0;


    void Update()
    {
        if (isTimeStop)
        {
            //ゴールされた後は以降の処理を実行しないためにreturn
            return;
        }

        time += Time.deltaTime;
        if (time >= 60f)
        {
            min++;
            time = time - 60;
        }
        timeText.text = min.ToString("00") + ":" + time.ToString("00.00");
    }

    public void StartTimer()
    {
        isTimeStop = false;
    }
    public void StopTime()
    {
        isTimeStop = true;
    }

    public void StopTimer()
    {
        recordTime = ConversionRecordTime(min, time);

        if (PlayerPrefs.GetFloat(GManager.instance.stageRecord[GManager.instance.stageNum - 1], 999.99f) > recordTime)
        {
            PlayerPrefs.SetFloat(GManager.instance.stageRecord[GManager.instance.stageNum - 1], recordTime);
            recordUpdateText.text = "New Record!";
        }
        clearTimeText.text = "ClearTime : " + min.ToString("00") + ":" + time.ToString("00.00");
        recordTimeText.text = "RecordTime : " + ConversionTextTime(PlayerPrefs.GetFloat(GManager.instance.stageRecord[GManager.instance.stageNum - 1]));
        isTimeStop = true;
    }

    float ConversionRecordTime(int min, float time)
    {
        recordTime = (float)(min * 60f) + time;
        return recordTime;
    }

    public string ConversionTextTime(float recordTime)
    {
        if (recordTime >= 60f)
        {
            float value;
            value = (recordTime - Mathf.Floor(recordTime)) * 100;
            int min = (int)(Mathf.Floor(recordTime) / 60);
            int sec = (int)recordTime - min * 60;
            stringRecordTime = min.ToString("00") + ":" + sec.ToString("00") + "." + Mathf.Floor(value).ToString();
        }
        else
        {
            stringRecordTime = recordTime.ToString("F2");
        }
        return stringRecordTime;

    }

}
 */
