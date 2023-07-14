using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class StageSelectManager : MonoBehaviour
{
[Header("ステージ")] public Transform[] stagePoint;
[Header("StageNumber")] public Text stageNumberText;
[Header("スクリプト")] public SceneTransitionManager sceneTransition;
[Header("スクリプト")] public AgentScript agentScript;
//[Header("RecordTime")] public Text recordTime;

private int stagePointNum = 0;
private int stageNum = 0;
private string sceneName;
private string stringRecordTime;
private bool right;
private bool left;

private void Start()
{
agentScript.isOn = true;
agentScript.transform.position = stagePoint[GManager.instance.stageNum].transform.position;
//MoveNextStage();
}

private void Update()
{
if (right)
{
    //右に動かすためのメソッドを呼び出す
    MoveNextStage();
}
else if (left)
{
    //左に動かすためのメソッドを呼び出す
    MovePreviousStage();
}
}

public void rPushDown()
{
//右ボタンを押している間
right = true;
}

public void rPushUp()
{
//右ボタンを押すのをやめた時
right = false;
}

public void lPushDown()
{
//左ボタンを押している間
left = true;
}

public void lPushUp()
{
//左ボタンを押すのをやめた時
left = false;
}
public void MoveNextStage()
{
Debug.Log("起動");
if (agentScript.isOn)
{
    if (GManager.instance.stageNum < GManager.instance.maxStageNum)
    {
        //++stagePointNum;
        //++stageNum;
        //GManager.instance.stageNum = stageNum;
        ++GManager.instance.stageNum;
        agentScript.IncreaceStagePoint(stagePoint[GManager.instance.stageNum].transform.position);
        stageNumberText.text = "STAGE :" + GManager.instance.stageNum.ToString();
        agentScript.isOn = false;
        //recordTime.text = ConversionTextTime(PlayerPrefs.GetFloat(GManager.instance.stageRecord[GManager.instance.stageNum - 1]));
    }
    else
    {
        Debug.Log("まだ進めないよ！");
    }

}
else
{
    Debug.Log("コースを通過していないよ");
}


}
public void MovePreviousStage()
{
if (agentScript.isOn)
{
    if (GManager.instance.stageNum <= 1)
    {
        Debug.Log("最初のステージだよ");
        return;
    }
    else
    {
        //--stagePointNum;
        //--stageNum;
        //GManager.instance.stageNum = stageNum;
        --GManager.instance.stageNum;
        agentScript.DecreaceStagePoint(stagePoint[GManager.instance.stageNum].transform.position);
        stageNumberText.text = "STAGE :" + GManager.instance.stageNum.ToString();
        agentScript.isOn = false;
        //recordTime.text = ConversionTextTime(PlayerPrefs.GetFloat(GManager.instance.stageRecord[GManager.instance.stageNum - 1]));
    }
}
else
{
    Debug.Log("コースを通過していないよ");
}
}

public void PlayGame()
{
sceneTransition.SceneToAnim("Stage" + GManager.instance.stageNum);
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
    stringRecordTime = "00:" + recordTime.ToString("00.00");
}
return stringRecordTime;

}

}
*/
