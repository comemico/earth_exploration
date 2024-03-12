using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<StageInformation> linearList;
    public List<StageInformation> scatterList;
    public List<StageInformation> stageInfoList;

    InformationManager informationMg;

    [Header("エリア番号")]
    [Range(0, 9)] public int areaNum;
    public int stageAdress; //選択されたステージがstageInfoListの何番目にあるか
    //public int reachStageNumber; //このエリアの最大到達値
    StageInformation nearStageInfo;

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


    public RectTransform RectTransform => this.transform as RectTransform;
    private List<Tween> tweenList = new List<Tween>();
    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Awake()
    {
        informationMg = GetComponentInParent<InformationManager>();
    }

    private void Start()
    {
        //reachStageNumber = informationMg.data.linearData[areaNum];
        InitializedStagePosition(informationMg.data.recentStageAdress);
        nearStageInfo = stageInfoList[informationMg.data.recentStageAdress];//1/24:最初のドラッグ時に、ChangeTarget()を起動させないようにするための処理

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

        if (nearStageInfo != NearestStageInfo())
        {
            StageInformation stageInfo = NearestStageInfo();
            stageAdress = stageInfoList.IndexOf(stageInfo);
            informationMg.UpdateStageInformation(stageInfo, stageAdress);
            informationMg.stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);

            nearStageInfo = stageInfo;
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
        stageAdress = stageInfoList.IndexOf(nearStageInfo);
        informationMg.UpdateStageInformation(nearStageInfo, stageAdress);
        informationMg.stageFrameMg.RectTransform.anchoredPosition = nearStageInfo.RectTransform.anchoredPosition;//2023.03.27:目的posまで移動
        MagnetItems(nearStageInfo.RectTransform.anchoredPosition);

    }

    public StageInformation NearestStageInfo()// マグネット中心に最も近い要素を選択する
    {
        StageInformation stageInfo = null;
        RectTransform nearestRect = null;
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            if (nearestRect == null)
            {
                // 初回選択
                nearestRect = stageInfoList[i].RectTransform;
                stageInfo = stageInfoList[i];
            }
            else
            {
                if (Vector2.SqrMagnitude(stageInfoList[i].RectTransform.anchoredPosition) < Vector2.SqrMagnitude(nearestRect.anchoredPosition) && stageInfoList[i].isDiscover == true)//絶対値的な
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
        nearStageInfo = stageInfo;
        stageAdress = stageInfoList.IndexOf(stageInfo);
        informationMg.UpdateStageInformation(stageInfo, stageAdress);

        informationMg.stageFrameMg.RectTransform.DOComplete();
        informationMg.stageFrameMg.RectTransform.anchoredPosition = stageInfo.RectTransform.anchoredPosition;
        informationMg.stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);
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
    private void DragComplete() => isDragging = false;

    public void InitializedStagePosition(int stageAdress) //引数:ステージ最大到達値
    {
        stageInfoList.AddRange(linearList);
        stageInfoList.AddRange(scatterList);

        for (int i = 0; i <= informationMg.data.linearData[areaNum]; i++)
        {
            if (i < informationMg.data.linearData[areaNum])
            {
                linearList[i].isClear = true;
            }
            if (i > linearList.Count - 1) break;
            linearList[i].isDiscover = true;
        }

        foreach (StageInformation stageInfo in stageInfoList)
        {
            stageInfo.JudgeLamp();
        }

        if (informationMg.data.recentCourseAdress == areaNum)
        {
            //直近のコースエリアを選択=>直近のステージ位置へ移動=>recent
            Vector2 target = stageInfoList[informationMg.data.recentStageAdress].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }
        else
        {
            //その他コースエリアは最大到達ステージ位置へ移動=>course[areaNum] 
            Vector2 target = stageInfoList[informationMg.data.linearData[areaNum]].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }

        informationMg.UpdateStageInformation(stageInfoList[informationMg.data.recentStageAdress], stageAdress);
    }

}