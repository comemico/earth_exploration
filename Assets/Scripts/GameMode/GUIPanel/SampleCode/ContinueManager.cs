using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

/*
public class ContinueManager : MonoBehaviour
{
//[SerializeField] PlayableDirector playableDirector;
[Header("コンティニュー番号")] public byte respawnNum; //場所によって番号を変えていく
//[Header("確認用:timeline番号")] [SerializeField] int timelineNum;
[Header("接触判定（スクリプト）")] public PlayerTriggerCheck respawnTrigger;
[Header("タイムライン制御スクリプト")] public TimelineManager timelineManager;
[Header("メモリ管理スクリプト")] public MemoryGageManager memoryGageManager;
[Header("メモリ残量テキスト")] public Text text;
[Header("ステート確認スクリプト")] public StageCtrl stageCtrl;
[Header("ジェット起動用")] public GrypsManager gryps;



private bool on = false;//二重防止


void Update()
{
    if (respawnTrigger.isOn && !on && stageCtrl.isPlay)
    {
        PassRespown();
        on = true;
    }

}
public void PassRespown()
{
    //GManager.instance.respawnNum = respawnNum;
    timelineManager.TapeChangeObject((respawnNum * 2) - 1);
    timelineManager.PlayTimeline(2);
    memoryGageManager.RecPassedMemoryGage();
    text.text = memoryGageManager.RecPassedMemoryGage().ToString();
    gryps.RecIsJet();
}
}
 */
