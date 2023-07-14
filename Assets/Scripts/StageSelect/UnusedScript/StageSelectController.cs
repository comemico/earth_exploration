using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class StageSelectController : MonoBehaviour
{
    [Header("ステージボタンの位置情報")] public RectTransform[] StageAddress;
    [Header("プレイヤー情報")] public GameObject gryps;
    [Header("プレイヤー情報")] public RectTransform grypsIcon;
    [Header("ステージ番号の表示")] public StageInformationManager stageInformation;

    private void Start()
    {
        MoveGryps(GManager.instance.stageNum, GManager.instance.maxLifeMemoryGage);
    }

    public void MoveGryps(int num, int life)
    {
        grypsIcon.anchoredPosition = StageAddress[(num - 1)].anchoredPosition;
        //gryps.transform.position = StageAddress[(num - 1)].transform.position; //移動
        GManager.instance.stageNum = num; //ステージ番号更新
        stageInformation.DisplayStageNum(num, life);//ステージ番号の表示
    }
}
 */
