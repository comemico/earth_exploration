using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
/*
public class MagnetMenu_X : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{


    [Header("ステージリスト")] [SerializeField] List<RectTransform> stageList;
    // 初期表示したときに中央に表示する値
    //[Header("初期表示したときに中央に表示する値")] [SerializeField] int centerElemIndex;
    
    // 吸いつく位置の中心座標→Menuオブジェクトのアンカー位置を変えることで実現
    //[SerializeField] private Vector2 magnetPosition;
    // 等間隔に並べる要素と要素の間隔
    //[SerializeField] private float itemDistance;
    // 制御対象の子要素
    [Header("InformationManager")] public InformationManager informationMg;

    
    // アニメーション中かどうかのフラグ
    // true : 実行中 / false : それ以外
    private bool isDragging;

    private void Awake()
    {
        informationMg = this.transform.root.gameObject.GetComponentInChildren<InformationManager>();
    }

    private void Start()
    {
        updateItemsScale();
        //MoveStageNumber(centerElemIndex);
    }

    private void OnDestroy()
    {
        this.tweenList.KillAllAndClear();
    }

    private void Update()
    {
        if (!this.isDragging)
        {
            return;
        }
        this.updateItemsScale();
    }

    private void updateItemsScale()
    {
        foreach (var item in this.stageList)
        {
            float distance = Mathf.Abs(item.GetAnchoredPosX());
            float scale = Mathf.Clamp(1.5f - (distance / (170.0f * 4.0f)), 1.0f, 1.5f);
            item.SetLocalScaleXY(scale);
        }
    }

    public void OnDrag(PointerEventData e)
    {
        // 操作量に応じてX方向に移動する
        float delta_x = e.delta.x;
        foreach (var item in this.stageList)
        {
            RectTransform rect = item;
            var pos = rect.anchoredPosition;
            pos.x += delta_x;
            rect.anchoredPosition = pos;
        }
    }

    public void OnBeginDrag(PointerEventData e)
    {
        this.isDragging = true;
        this.tweenList.KillAllAndClear();
    }

    //＊離した後近い位置に引き寄せるロジック＊ (取得可能変数 : 近い位置情報、位置情報が持つ配列番号)
    public void OnEndDrag(PointerEventData e)
    {
        // 移動目標量を計算
        RectTransform rect = this.pickupNearestRect();
        float targetX = -rect.GetAnchoredPosX();

        //Debug.Log(targetX);
        informationMg.UpdateStageNumber(stageList.IndexOf(pickupNearestRect()));

        for (int i = 0; i < this.stageList.Count; i++)
        {
            RectTransform item = this.stageList[i];

            Tween t =
                item.DOAnchorPosX(item.GetAnchoredPosX()
                    + targetX, 0.075f).SetEase(Ease.OutSine);
            if (i <= this.stageList.Count)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(t);
                seq.AppendCallback(this.onCompleted);
                this.tweenList.Add(seq);
            }
            else
            {
                this.tweenList.Add(t);
            }
        }

    }

    private void onCompleted() => this.isDragging = false;


    private List<Tween> tweenList = new List<Tween>();

    // マグネット中心に最も近い要素を選択する
    private RectTransform pickupNearestRect()
    {
        RectTransform nearestRect = null;
        foreach (var rect in this.stageList)
        {
            if (nearestRect == null)
            {
                nearestRect = rect; // 初回選択
            }
            else
            {
                if (Mathf.Abs(rect.GetAnchoredPosX()) < Mathf.Abs(nearestRect.GetAnchoredPosX()))
                {
                    nearestRect = rect; // より中心に近いほうを選択
                }
            }
        }
        return nearestRect;
    }



    public void MoveStageNumber(int num)
    {
        informationMg.UpdateStageNumber(num);

        // 移動目標量を計算
        //RectTransform rect = this.pickupNearestRect();
        RectTransform rect = stageList[num];
        float targetX = -rect.GetAnchoredPosX();

        for (int i = 0; i < this.stageList.Count; i++)
        {
            RectTransform item = this.stageList[i];

            Tween t =
                item.DOAnchorPosX(item.GetAnchoredPosX()
                    + targetX, 0.075f).SetEase(Ease.OutSine);
            if (i <= this.stageList.Count)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(t);
                seq.AppendCallback(updateItemsScale);
                seq.AppendCallback(this.onCompleted);
                this.tweenList.Add(seq);
            }
            else
            {
                this.tweenList.Add(t);
            }
        }
    }



    
    public void MoveUpStage()
    {
        // 移動目標量を計算
        int num = stageList.IndexOf(this.pickupNearestRect());

        if (stageList.Count - 1 > num)
        {
            RectTransform rect = stageList[num + 1];
            informationMg.DisplayStageNumber(num + 1);


            //Debug.Log(num + "から" + items[num + 1] + "へ移動");
            float targetX = -rect.GetAnchoredPosX();

            for (int i = 0; i < this.stageList.Count; i++)
            {
                RectTransform item = this.stageList[i];

                Tween t =
                    item.DOAnchorPosX(item.GetAnchoredPosX()
                        + targetX, 0.075f).SetEase(Ease.OutSine);
                if (i <= this.stageList.Count)
                {
                    Sequence seq = DOTween.Sequence();
                    seq.Append(t);
                    seq.AppendCallback(updateItemsScale);
                    seq.AppendCallback(this.onCompleted);
                    this.tweenList.Add(seq);
                }
                else
                {
                    this.tweenList.Add(t);
                }
            }

        }
    }
    public void MoveDownStage()
    {
        // 移動目標量を計算
        int num = stageList.IndexOf(this.pickupNearestRect());

        if (num >= 1)
        {
            RectTransform rect = stageList[num - 1];
            informationMg.DisplayStageNumber(num - 1);


            //Debug.Log(num + "から" + items[num - 1] + "へ移動");
            float targetX = -rect.GetAnchoredPosX();

            for (int i = 0; i < this.stageList.Count; i++)
            {
                RectTransform item = this.stageList[i];

                Tween t =
                    item.DOAnchorPosX(item.GetAnchoredPosX()
                        + targetX, 0.075f).SetEase(Ease.OutSine);
                if (i <= this.stageList.Count)
                {
                    Sequence seq = DOTween.Sequence();
                    seq.Append(t);
                    seq.AppendCallback(updateItemsScale);
                    seq.AppendCallback(this.onCompleted);
                    this.tweenList.Add(seq);
                }
                else
                {
                    this.tweenList.Add(t);
                }
            }

        }
    }
    

}
 */