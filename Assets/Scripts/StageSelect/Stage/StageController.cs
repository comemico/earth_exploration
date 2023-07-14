using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("�擾")]
    [Header("-----------------------------")]
    public List<StageInformation> stageInfoList;


    [Space(PreHeader)]
    [Header("�m�F")]
    [Header("-----------------------------")]
    [Header("���̃G���A�̓��B�l")]
    public int reachStageNumber;
    [Header("���̃G���A�ő僌�x����")]
    public int maxAreaLevel;
    [Header("�h���b�O�`�A�j���[�V���������ǂ���")]
    public bool isDragging;


    [Space(PreHeader)]
    [Header("����---�X�e�[�W�ړ�---")]
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

    [Header("---�R�[�X�p�l��----")]
    [Header("���슴�x")]
    public float dragSensitivity;

    [Header("---�C�[�W���O----")]
    [Header("���")]
    public Ease easeType;
    [Header("����")]
    public float easeDuration;

    [Header("---�I��:�傫��----")]
    public float maxValue_Scale;
    [Header("�e���͈�")]
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
        //2022.10.28 :���I�u�W�F�N�g�̔z��v�f����R�[�X�ԍ����擾���R�[�X�ԍ�����X�e�[�W�ő哞�B�l�̒l�����o�����e�R�[�X�̍ő哞�B�X�e�[�W�ֈړ����鏈��
        //2022.11.02 :Title,Loading�̃V�[����Gmanager�������
        reachStageNumber = GManager.instance.courseDate[(int)areaNumber];
        InitializedStagePosition(reachStageNumber);
        nearestStageNumber = reachStageNumber;//1/24:�ŏ��̃h���b�O���ɁAChangeTarget()���N�������Ȃ��悤�ɂ��邽�߂̏���
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
        informationMg.stageFrameMg.RectTransform.anchoredPosition = NearestStageInfo().RectTransform.anchoredPosition;//�Ǐ]
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

    public void OnDrag(PointerEventData e)// ����ʂɉ�����XY�����Ɉړ�����
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

    public void OnEndDrag(PointerEventData e)//����������߂��ʒu�Ɉ����񂹂郍�W�b�N
    {
        StageInformation nearStageInfo = NearestStageInfo();
        informationMg.UpdateStageInformation(nearStageInfo);
        informationMg.stageFrameMg.RectTransform.anchoredPosition = nearStageInfo.RectTransform.anchoredPosition;//2023.03.27:�ړIpos�܂ňړ�
        MagnetItems(nearStageInfo.RectTransform.anchoredPosition);

    }

    public StageInformation NearestStageInfo()// �}�O�l�b�g���S�ɍł��߂��v�f��I������
    {
        StageInformation stageInfo = null;
        RectTransform nearestRect = null;
        for (int i = 0; i <= reachStageNumber; i++)
        {
            if (nearestRect == null)
            {
                // ����I��
                nearestRect = stageInfoList[i].RectTransform;
                stageInfo = stageInfoList[i];
            }
            else
            {
                if (Vector2.SqrMagnitude(stageInfoList[i].RectTransform.anchoredPosition) < Vector2.SqrMagnitude(nearestRect.anchoredPosition))//��Βl�I��
                {
                    nearestRect = stageInfoList[i].RectTransform; // ��蒆�S�ɋ߂��ق���I���A�X�V
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
        informationMg.stageFrameMg.RectTransform.DOAnchorPos(-target, easeDuration).SetEase(easeType).SetRelative(true);//����pos���猴�_(0,0)�ֈړ�
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
        foreach (var item in stageInfoList)//�S�Ă�Button��Interactable��Off
        {
            item.CheckInteractible(false);

            if (maxAreaLevel < (int)item.stageLevel)//(0 < 1)
            {
                maxAreaLevel = (int)item.stageLevel;// = 1 �X�V
            }
        }
        for (int i = 0; i <= initialValue; i++)//�ő哞�B�l�܂ł�Button��Interactable��On
        {
            stageInfoList[i].CheckInteractible(true);
        }

        //�e�R�[�X�̍ő哞�B�X�e�[�W�ֈړ����鏈��
        Vector2 target = stageInfoList[initialValue].RectTransform.anchoredPosition;
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            stageInfoList[i].RectTransform.anchoredPosition -= target;
        }
    }



}