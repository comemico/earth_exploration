using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AreaController : MonoBehaviour
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("取得")]
    [Header("-----------------------------")]
    public GameObject area;
    public List<StageController> stageCtrlList;


    [Space(PreHeader)]
    [Header("操作---エリア移動---")]
    [Header("-----------------------------")]
    [Header("イージング")]
    public Ease easeType;
    public float easeDuration;


    InformationManager informationMg;
    private List<Tween> tweenList = new List<Tween>();

    private void Start()
    {
        informationMg = GetComponentInParent<InformationManager>();
        InitializedAreaPosition(GManager.instance.recentCourseNum);
        MoveArea(GManager.instance.recentCourseNum, true);
    }

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    public void MoveArea(int courseNum, bool isComplete = false)
    {

        if (courseNum >= stageCtrlList.Count)
        {
            Debug.Log("エリアが追加されていません");
            return;
        }

        informationMg.UpdateStageInformation(stageCtrlList[courseNum].NearestStageInfo());
        informationMg.UpdateCourseNumber(courseNum, stageCtrlList[courseNum].maxAreaLevel);

        tweenList.KillAllAndClear();

        float target = stageCtrlList[courseNum].RectTransform.anchoredPosition.y;

        for (int i = 0; i < stageCtrlList.Count; i++)
        {
            Tween t = stageCtrlList[i].RectTransform.DOAnchorPosY(-target, easeDuration).SetRelative(true).SetEase(easeType);

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

    public void InitializedAreaPosition(int initialValue)
    {
        for (int i = 0; i < area.transform.childCount; i++)
        {
            stageCtrlList.Add(area.transform.GetChild(i).GetComponent<StageController>());
        }

        //開始時に選択したコース番号へ移動する処理
        Vector2 targetY = stageCtrlList[initialValue].RectTransform.anchoredPosition;
        for (int i = 0; i < stageCtrlList.Count; i++)
        {
            stageCtrlList[i].RectTransform.anchoredPosition -= targetY;
        }

    }


}
