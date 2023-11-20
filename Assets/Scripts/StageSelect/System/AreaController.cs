using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AreaController : MonoBehaviour
{
    [Header("取得")]
    public GameObject area;
    public List<StageController> stageCtrlList;

    [Header("イージング-エリア移動-")]
    public float moveDuration;
    public Ease moveType;

    InformationManager informationMg;
    StageFrameManager stageFrameMg;

    private List<Tween> tweenList = new List<Tween>();
    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        GetComponent();
        InitializedAreaPosition(informationMg.data.recentCourseAdress);
    }

    void GetComponent()
    {
        informationMg = GetComponentInParent<InformationManager>();
        stageFrameMg = GetComponentInChildren<StageFrameManager>();
    }

    public void InitializedAreaPosition(int initialValue)
    {
        for (int i = 0; i < area.transform.childCount; i++)
        {
            stageCtrlList.Add(area.transform.GetChild(i).GetComponent<StageController>());
        }
        //コースエリア位置へ移動
        Vector2 targetY = stageCtrlList[initialValue].RectTransform.anchoredPosition;
        for (int i = 0; i < stageCtrlList.Count; i++)
        {
            stageCtrlList[i].RectTransform.anchoredPosition -= targetY;
        }
    }

    public void MoveArea(int courseNum, bool isComplete = false)
    {
        if (courseNum >= stageCtrlList.Count)
        {
            Debug.Log("エリアが追加されていません");
            return;
        }
        StageInformation stageInfo = stageCtrlList[courseNum].NearestStageInfo();

        stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);

        tweenList.KillAllAndClear();
        informationMg.UpdateStageInformation(stageInfo, stageCtrlList[courseNum].stageInfoList.IndexOf(stageInfo));
        informationMg.UpdateCourseNumber(courseNum);

        float target = stageCtrlList[courseNum].RectTransform.anchoredPosition.y;
        for (int i = 0; i < stageCtrlList.Count; i++)
        {
            Tween t = stageCtrlList[i].RectTransform.DOAnchorPosY(-target, moveDuration).SetRelative(true).SetEase(moveType);
            if (i <= stageCtrlList.Count)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(t);
                if (isComplete)
                {
                    seq.Complete();
                }
                tweenList.Add(seq);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }



}
