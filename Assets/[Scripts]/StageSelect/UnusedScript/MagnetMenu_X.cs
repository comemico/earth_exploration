using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
/*
public class MagnetMenu_X : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{


    [Header("�X�e�[�W���X�g")] [SerializeField] List<RectTransform> stageList;
    // �����\�������Ƃ��ɒ����ɕ\������l
    //[Header("�����\�������Ƃ��ɒ����ɕ\������l")] [SerializeField] int centerElemIndex;
    
    // �z�����ʒu�̒��S���W��Menu�I�u�W�F�N�g�̃A���J�[�ʒu��ς��邱�ƂŎ���
    //[SerializeField] private Vector2 magnetPosition;
    // ���Ԋu�ɕ��ׂ�v�f�Ɨv�f�̊Ԋu
    //[SerializeField] private float itemDistance;
    // ����Ώۂ̎q�v�f
    [Header("InformationManager")] public InformationManager informationMg;

    
    // �A�j���[�V���������ǂ����̃t���O
    // true : ���s�� / false : ����ȊO
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
        // ����ʂɉ�����X�����Ɉړ�����
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

    //����������߂��ʒu�Ɉ����񂹂郍�W�b�N�� (�擾�\�ϐ� : �߂��ʒu���A�ʒu��񂪎��z��ԍ�)
    public void OnEndDrag(PointerEventData e)
    {
        // �ړ��ڕW�ʂ��v�Z
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

    // �}�O�l�b�g���S�ɍł��߂��v�f��I������
    private RectTransform pickupNearestRect()
    {
        RectTransform nearestRect = null;
        foreach (var rect in this.stageList)
        {
            if (nearestRect == null)
            {
                nearestRect = rect; // ����I��
            }
            else
            {
                if (Mathf.Abs(rect.GetAnchoredPosX()) < Mathf.Abs(nearestRect.GetAnchoredPosX()))
                {
                    nearestRect = rect; // ��蒆�S�ɋ߂��ق���I��
                }
            }
        }
        return nearestRect;
    }



    public void MoveStageNumber(int num)
    {
        informationMg.UpdateStageNumber(num);

        // �ړ��ڕW�ʂ��v�Z
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
        // �ړ��ڕW�ʂ��v�Z
        int num = stageList.IndexOf(this.pickupNearestRect());

        if (stageList.Count - 1 > num)
        {
            RectTransform rect = stageList[num + 1];
            informationMg.DisplayStageNumber(num + 1);


            //Debug.Log(num + "����" + items[num + 1] + "�ֈړ�");
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
        // �ړ��ڕW�ʂ��v�Z
        int num = stageList.IndexOf(this.pickupNearestRect());

        if (num >= 1)
        {
            RectTransform rect = stageList[num - 1];
            informationMg.DisplayStageNumber(num - 1);


            //Debug.Log(num + "����" + items[num - 1] + "�ֈړ�");
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