using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<StageInformation> stageInfoList;

    [Header("このエリアの最大到達値")]
    public int reachStageNumber;
    // public int maxAreaLevel; [Header("このエリア最大レベル数")]

    [Header("エリア番号")]
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

    [Header("ステージパネル:機種考慮版")]
    public float dragSensitivity; //[Header("操作感度")]    
    public float maxValue_Scale; //[Header("大きさ")] 
    public float rangeOfInfluence_Scale; //[Header("影響範囲")]
    Vector2 dragLength;
    Vector2 screenFactor;
    Vector2 oldPosition, currentPosition;
    private float factorScale;

    [Header("ステージパネル:性能優先版")]
    public float dragSensitivityS;

    [Header("イージング-Magnet-")]
    public float easeDuration;
    public Ease easeType;

    [Header("ドラッグ～アニメーション中かどうか")]
    public bool isDragging;

    InformationManager informationMg;

    public RectTransform RectTransform => this.transform as RectTransform;
    private int nearestStageNumber;
    private List<Tween> tweenList = new List<Tween>();
    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Awake()
    {
        informationMg = GetComponentInParent<InformationManager>();
    }

    private void Start()
    {
        reachStageNumber = GManager.instance.courseDate[(int)areaNumber];
        InitializedStagePosition(reachStageNumber);
        nearestStageNumber = reachStageNumber;//1/24:最初のドラッグ時に、ChangeTarget()を起動させないようにするための処理
        factorScale = 1 / Mathf.Pow(rangeOfInfluence_Scale, 2);
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);

        //2022.10.28 :自オブジェクトの配列要素からコース番号を取得→コース番号からステージ最大到達値の値を取り出す→各コースの最大到達ステージへ移動する処理
        //2022.11.02 :Title,LoadingのシーンにGmanagerをいれる 
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
            informationMg.stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = eventData.position * screenFactor;
        //tweenList.KillAllAndClear();
        isDragging = true;
    }

    /*
    public void OnDrag(PointerEventData eventData)// 操作量に応じてXY方向に移動する
    {
        currentPosition = eventData.position * screenFactor;

        dragLength = (currentPosition - oldPosition) * dragSensitivity;

        foreach (var item in this.stageInfoList)
        {
            item.RectTransform.anchoredPosition += dragLength;
        }

        oldPosition = currentPosition;
    }
     */

    /*
     */
    public void OnDrag(PointerEventData e)
    {
        float delta_y = (e.delta.y * dragSensitivityS);// 操作量に応じてY方向に移動する
        float delta_x = (e.delta.x * dragSensitivityS);// 操作量に応じてY方向に移動する

        foreach (var item in this.stageInfoList)
        {
            RectTransform rect = item.RectTransform;
            var pos = rect.anchoredPosition;
            pos.y += delta_y;
            pos.x += delta_x;
            rect.anchoredPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData e)//＊離した後近い位置に引き寄せるロジック
    {
        StageInformation nearStageInfo = NearestStageInfo();
        informationMg.UpdateStageInformation(nearStageInfo);
        informationMg.stageFrameMg.RectTransform.anchoredPosition = nearStageInfo.RectTransform.anchoredPosition;//2023.03.27:目的posまで移動
        MagnetItems(nearStageInfo.RectTransform.anchoredPosition);

    }

    private void DragComplete() => isDragging = false;

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
        informationMg.stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);
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

    public void InitializedStagePosition(int initialValue) //引数:ステージ最大到達値
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            stageInfoList.Add(transform.GetChild(i).GetComponent<StageInformation>());
        }
        foreach (var item in stageInfoList)//全てのButtonのInteractableをOff
        {
            item.CheckInteractible(false);
        }
        for (int i = 0; i <= initialValue; i++)//最大到達値までのButtonのInteractableをOn
        {
            stageInfoList[i].CheckInteractible(true);
        }

        if (GManager.instance.recentCourseNum == (int)areaNumber)
        {
            //直近のコースエリアを選択=>直近のステージ位置へ移動
            Vector2 target = stageInfoList[GManager.instance.recentStageNum].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }
        else
        {
            //その他コースエリアは最大到達ステージ位置へ移動
            Vector2 target = stageInfoList[initialValue].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }
        informationMg.UpdateStageInformation(stageInfoList[GManager.instance.recentStageNum]);
    }



}