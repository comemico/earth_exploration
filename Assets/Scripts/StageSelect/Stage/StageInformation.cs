using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformation : MonoBehaviour
{
    public enum StageNumber
    {
        Stage_01 = 0,
        Stage_02,
        Stage_03,
        Stage_04,
        Stage_05,
        Stage_06,
        Stage_07
    }

    public enum StageLevel
    {
        Level_01 = 1,
        Level_02,
        Level_03,
        Level_04,
        Level_05,
        Level_06,
        Level_07,
        Level_08,
        Level_09,
        Level_10
    }

    public StageNumber stageNumber;
    [Range(1, 37)] public int stageLevel;
    public string tips;
    //[Range(1, 40)] public int stageLifeNumber;
    public RectTransform RectTransform => this.transform as RectTransform;
    StageController stageCtrl;
    StageInformation stageInfo;

    void Start()
    {
        stageCtrl = GetComponentInParent<StageController>();
        stageInfo = GetComponent<StageInformation>();
        GetComponentInChildren<Button>().onClick.AddListener(StageClick);
    }
    void StageClick()
    {
        stageCtrl.MoveStage(stageInfo);
    }
    public void CheckInteractible(bool interactible)
    {
        GetComponentInChildren<Button>().interactable = interactible;
    }
}
