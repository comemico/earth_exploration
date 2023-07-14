using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CourseController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("取得&確認")]
    [Header("-----------------------------")]
    public GameObject Course;
    public List<CourseInformation> courseInfoList;
    [Header("ドラッグ～アニメーション中かどうか")]
    public bool isDragging;


    [Space(PreHeader)]
    [Header("操作")]
    [Header("---コースパネル----")]
    [Header("操作感度")]
    public float dragSensitivity;

    [Header("---イージング----")]
    [Header("種類")]
    public Ease easeType;
    [Header("時間")]
    public float easeDuration;
    [Header("---イージング:Fade----")]
    [Header("種類")]
    public Ease easeTypeFade;
    [Header("時間")]
    public float easeDurationFade;
    [Header("移動量")]
    public float distanceFade;

    [Header("---選択:距離----")]
    [Header("最大値")]
    public float maxValue_Distance;
    [Header("影響範囲")]
    public float rangeOfInfluence_Distance;

    [Header("---選択:大きさ----")]
    public float maxValue_Scale;
    [Header("影響範囲")]
    public float rangeOfInfluence_Scale;

    AreaController areaCtrl;
    private List<Tween> tweenList = new List<Tween>();
    public CourseFrameManager courseFrameMg;
    private int nearestNumber;//最も近い要素が変わったら更新する為の変数
    private float factorDistance;
    private float factorScale;

    private void Start()
    {
        areaCtrl = transform.parent.GetComponentInChildren<AreaController>();
        courseFrameMg = GetComponentInChildren<CourseFrameManager>();

        InitializedStagePosition(GManager.instance.recentCourseNum);
        factorDistance = 1 / rangeOfInfluence_Distance;
        factorScale = 1 / rangeOfInfluence_Scale;
    }

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Update()
    {
        UpdateSelection();
        if (!isDragging)
        {
            return;
        }
        if (nearestNumber != (int)NearestCourseInfo().courseNumber)
        {
            CourseInformation courseInfo = NearestCourseInfo();

            areaCtrl.MoveArea((int)courseInfo.courseNumber);
            SelectionEnable((int)courseInfo.courseNumber);
            courseFrameMg.SelectCourse();

            nearestNumber = (int)courseInfo.courseNumber;
            return;
        }
        courseFrameMg.RectTransform.anchoredPosition = NearestCourseInfo().RectTransform.anchoredPosition;//追従
    }

    private void UpdateSelection()
    {
        foreach (var item in courseInfoList)
        {

            float distance = Mathf.Abs(item.RectTransform.anchoredPosition.y);
            float value_Distance = Mathf.Clamp(maxValue_Distance - (distance * factorDistance), 0, maxValue_Distance);
            Vector2 pos = item.RectTransform.anchoredPosition;
            pos.x = value_Distance;
            item.RectTransform.anchoredPosition = pos;

            float value_Scale = Mathf.Clamp(maxValue_Scale - (distance * factorScale), 0.95f, maxValue_Scale);
            Vector2 scale = item.RectTransform.anchoredPosition;
            scale.x = value_Scale;
            scale.y = value_Scale;
            item.RectTransform.localScale = scale;

        }
    }

    public void OnDrag(PointerEventData e)
    {
        float delta_y = (e.delta.y * dragSensitivity);// 操作量に応じてY方向に移動する

        foreach (var item in courseInfoList)
        {
            RectTransform rect = item.RectTransform;
            var pos = rect.anchoredPosition;
            pos.y += delta_y;
            rect.anchoredPosition = pos;
        }
    }

    public void OnBeginDrag(PointerEventData e)
    {
        tweenList.KillAllAndClear();
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData e)
    {
        MagnetItems(NearestCourseInfo().RectTransform.anchoredPosition.y);
    }

    private void DragCompleted() => isDragging = false;

    private CourseInformation NearestCourseInfo()// マグネット中心に最も近い要素を選択する
    {
        CourseInformation courseInfo = null;
        RectTransform nearestRect = null;
        foreach (var item in courseInfoList)
        {
            if (nearestRect == null)
            {
                nearestRect = item.RectTransform; // 初回選択
                courseInfo = item;
            }
            else
            {
                if (Mathf.Abs(item.RectTransform.anchoredPosition.y) < Mathf.Abs(nearestRect.anchoredPosition.y))
                {
                    nearestRect = item.RectTransform; // より中心(0)に近いほうを選択
                    courseInfo = item;
                }
            }
        }
        return courseInfo;
    }

    public void MoveUpArea()
    {
        int nearestNum = (int)NearestCourseInfo().courseNumber;
        if (courseInfoList.Count - 1 > nearestNum)
        {
            nearestNumber = nearestNum + 1;
            areaCtrl.MoveArea(nearestNumber);
            SelectionEnable(nearestNumber);
            MagnetItems(courseInfoList[nearestNumber].RectTransform.anchoredPosition.y);
        }
    }
    public void MoveDownArea()
    {
        int nearestNum = (int)NearestCourseInfo().courseNumber;
        if (nearestNum >= 1)
        {
            nearestNumber = nearestNum - 1;
            areaCtrl.MoveArea(nearestNumber);
            SelectionEnable(nearestNumber);
            MagnetItems(courseInfoList[nearestNumber].RectTransform.anchoredPosition.y);
        }
    }

    public void MoveCourse(int courseNum)
    {
        nearestNumber = courseNum;
        areaCtrl.MoveArea(courseNum);
        SelectionEnable(courseNum);
        MagnetItems(courseInfoList[courseNum].RectTransform.anchoredPosition.y);
    }

    void MagnetItems(float targetY)
    {
        courseFrameMg.RectTransform.DOComplete();
        courseFrameMg.RectTransform.anchoredPosition = new Vector2(maxValue_Distance, targetY);
        courseFrameMg.RectTransform.DOAnchorPosY(-targetY, easeDuration).SetRelative(true).SetEase(easeType);
        courseFrameMg.SelectCourse();

        for (int i = 0; i < courseInfoList.Count; i++)
        {
            Tween t = courseInfoList[i].RectTransform.DOAnchorPosY(-targetY, easeDuration).SetRelative(true).SetEase(easeType);

            if (i <= courseInfoList.Count)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(t);
                sequence.AppendCallback(DragCompleted);
                tweenList.Add(sequence);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }

    public void FadeOutItems(bool isComplete)
    {
        courseFrameMg.RectTransform.DOAnchorPosY(distanceFade, easeDurationFade).SetRelative(true).SetEase(easeTypeFade);
        for (int i = 0; i < courseInfoList.Count; i++)
        {
            Tween t = courseInfoList[i].RectTransform.DOAnchorPosY(distanceFade, easeDurationFade).SetRelative(true).SetEase(easeTypeFade);

            if (i <= courseInfoList.Count)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(t);
                sequence.AppendCallback(DragCompleted);
                if (isComplete) sequence.Complete();
                tweenList.Add(sequence);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }
    public void FadeInItems()
    {
        courseFrameMg.RectTransform.DOAnchorPosY(distanceFade, easeDurationFade).SetRelative(true).SetEase(easeTypeFade);
        for (int i = 0; i < courseInfoList.Count; i++)
        {
            Tween t = courseInfoList[i].RectTransform.DOAnchorPosY(distanceFade, easeDurationFade).SetRelative(true).SetEase(easeTypeFade);

            if (i <= courseInfoList.Count)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(t);
                sequence.AppendCallback(DragCompleted);
                tweenList.Add(sequence);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }

    public void InitializedStagePosition(int initialValue)
    {
        for (int i = 0; i < Course.transform.childCount; i++)
        {
            courseInfoList.Add(Course.transform.GetChild(i).GetComponent<CourseInformation>());
        }

        //開始時に選択したコース番号へ移動する処理
        Vector2 target = courseInfoList[initialValue].RectTransform.anchoredPosition;
        for (int i = 0; i < courseInfoList.Count; i++)
        {
            courseInfoList[i].RectTransform.anchoredPosition -= target;
        }
        courseFrameMg.RectTransform.anchoredPosition = new Vector2(maxValue_Distance, courseFrameMg.RectTransform.anchoredPosition.y);
        SelectionEnable(initialValue);
    }

    public void SelectionEnable(int courseNum)
    {
        foreach (var item in courseInfoList)//全てのImageのenableをOff
        {
            item.CheckEnable(false);
        }
        courseInfoList[courseNum].CheckEnable(true);

    }


}