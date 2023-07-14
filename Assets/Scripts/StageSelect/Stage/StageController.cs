using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("取得")]
    [Header("-----------------------------")]
    public List<StageInformation> stageInfoList;


    [Space(PreHeader)]
    [Header("確認")]
    [Header("-----------------------------")]
    [Header("このエリアの到達値")]
    public int reachStageNumber;
    [Header("このエリア最大レベル数")]
    public int maxAreaLevel;
    [Header("ドラッグ～アニメーション中かどうか")]
    public bool isDragging;


    [Space(PreHeader)]
    [Header("操作---ステージ移動---")]
    [Header("-----------------------------")]
    public AreaNumber areaNumber;
    public enum AreaNumber
    {
        Area_01 = 0,
        Area_02,
        Area_03,
        Area_04,
        Area_05,
        Area_06,
        Area_07,
        Area_08,
        Area_09,
        Area_10
    }

    [Header("---コースパネル----")]
    [Header("操作感度")]
    public float dragSensitivity;

    [Header("---イージング----")]
    [Header("種類")]
    public Ease easeType;
    [Header("時間")]
    public float easeDuration;

    [Header("---選択:大きさ----")]
    public float maxValue_Scale;
    [Header("影響範囲")]
    public float rangeOfInfluence_Scale;

    private float factorScale;


    public RectTransform RectTransform => this.transform as RectTransform;
    private List<Tween> tweenList = new List<Tween>();

    InformationManager informationMg;
    private int nearestStageNumber;

    private void Awake()
    {
        informationMg = GetComponentInParent<InformationManager>();
    }

    private void Start()
    {
        //2022.10.28 :自オブジェクトの配列要素からコース番号を取得→コース番号からステージ最大到達値の値を取り出す→各コースの最大到達ステージへ移動する処理
        //2022.11.02 :Title,LoadingのシーンにGmanagerをいれる
        reachStageNumber = GManager.instance.courseDate[(int)areaNumber];
        InitializedStagePosition(reachStageNumber);
        nearestStageNumber = reachStageNumber;//1/24:最初のドラッグ時に、ChangeTarget()を起動させないようにするための処理
        factorScale = 1 / Mathf.Pow(rangeOfInfluence_Scale, 2);
    }

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Update()
    {
        UpdateItemsScale();
        if (!isDragging)
        {
            return;
        }

        if (nearestStageNumber != (int)NearestStageInfo().stageNumber)
        {
            StageInformation stageInfo = NearestStageInfo();

            informationMg.UpdateStageInformation(stageInfo);
            informationMg.stageFrameMg.ChangeTarget((int)stageInfo.stageLevel);
            nearestStageNumber = (int)stageInfo.stageNumber;
            return;
        }
        informationMg.stageFrameMg.RectTransform.anchoredPosition = NearestStageInfo().RectTransform.anchoredPosition;//追従
    }

    private void UpdateItemsScale()
    {
        foreach (var item in stageInfoList)
        {
            float distance = Vector2.SqrMagnitude(item.RectTransform.anchoredPosition);
            float value_Scale = Mathf.Clamp(maxValue_Scale - (distance * factorScale * 1), 1f, maxValue_Scale);
            Vector2 scale = item.RectTransform.anchoredPosition;
            scale.x = value_Scale;
            scale.y = value_Scale;
            item.RectTransform.localScale = scale;
        }

    }

    public void OnDrag(PointerEventData e)// 操作量に応じてXY方向に移動する
    {
        float delta_y = (e.delta.y * dragSensitivity);
        float delta_x = (e.delta.x * dragSensitivity);
        foreach (var item in this.stageInfoList)
        {
            Vector2 pos = item.RectTransform.anchoredPosition;
            pos.x += delta_x;
            pos.y += delta_y;
            item.RectTransform.anchoredPosition = pos;
        }
    }

    // private void DragComplete() => isDragging = false;
    private void DragComplete()
    {
        isDragging = false;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        tweenList.KillAllAndClear();
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData e)//＊離した後近い位置に引き寄せるロジック
    {
        StageInformation nearStageInfo = NearestStageInfo();
        informationMg.UpdateStageInformation(nearStageInfo);
        informationMg.stageFrameMg.RectTransform.anchoredPosition = nearStageInfo.RectTransform.anchoredPosition;//2023.03.27:目的posまで移動
        MagnetItems(nearStageInfo.RectTransform.anchoredPosition);

    }

    public StageInformation NearestStageInfo()// マグネット中心に最も近い要素を選択する
    {
        StageInformation stageInfo = null;
        RectTransform nearestRect = null;
        for (int i = 0; i <= reachStageNumber; i++)
        {
            if (nearestRect == null)
            {
                // 初回選択
                nearestRect = stageInfoList[i].RectTransform;
                stageInfo = stageInfoList[i];
            }
            else
            {
                if (Vector2.SqrMagnitude(stageInfoList[i].RectTransform.anchoredPosition) < Vector2.SqrMagnitude(nearestRect.anchoredPosition))//絶対値的な
                {
                    nearestRect = stageInfoList[i].RectTransform; // より中心に近いほうを選択、更新
                    stageInfo = stageInfoList[i];
                }
            }
        }
        return stageInfo;
    }

    public void MoveStage(StageInformation stageInfo)
    {
        nearestStageNumber = (int)stageInfo.stageNumber;
        informationMg.UpdateStageInformation(stageInfo);
        informationMg.stageFrameMg.ChangeTarget((int)stageInfo.stageLevel);
        informationMg.stageFrameMg.RectTransform.DOComplete();
        informationMg.stageFrameMg.RectTransform.anchoredPosition = stageInfo.RectTransform.anchoredPosition;
        MagnetItems(stageInfo.RectTransform.anchoredPosition);
    }

    void MagnetItems(Vector2 target)
    {
        informationMg.stageFrameMg.RectTransform.DOAnchorPos(-target, easeDuration).SetEase(easeType).SetRelative(true);//次のposから原点(0,0)へ移動
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            Tween t = stageInfoList[i].RectTransform.DOAnchorPos(-target, easeDuration).SetEase(easeType).SetRelative(true);

            if (i <= stageInfoList.Count)
            {
                Sequence move = DOTween.Sequence();
                move.Append(t);
                move.OnComplete(DragComplete);
                tweenList.Add(move);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }

    public void InitializedStagePosition(int initialValue)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            stageInfoList.Add(transform.GetChild(i).GetComponent<StageInformation>());
        }
        foreach (var item in stageInfoList)//全てのButtonのInteractableをOff
        {
            item.CheckInteractible(false);

            if (maxAreaLevel < (int)item.stageLevel)//(0 < 1)
            {
                maxAreaLevel = (int)item.stageLevel;// = 1 更新
            }
        }
        for (int i = 0; i <= initialValue; i++)//最大到達値までのButtonのInteractableをOn
        {
            stageInfoList[i].CheckInteractible(true);
        }

        //各コースの最大到達ステージへ移動する処理
        Vector2 target = stageInfoList[initialValue].RectTransform.anchoredPosition;
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            stageInfoList[i].RectTransform.anchoredPosition -= target;
        }
    }



}