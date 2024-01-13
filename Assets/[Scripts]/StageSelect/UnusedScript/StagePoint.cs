using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePoint : MonoBehaviour
{
    [Header("ステージ番号")] public int stagePointNum; //場所によって番号を変えていく
    [Header("接触判定（スクリプト）")] public PlayerTriggerCheck PlayerTrigger;

    private bool on = false;//二重防止

    void Update()
    {
        if (PlayerTrigger.isOn && !on)
        {
            on = true;
        }
    }
}
