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

    [Header("���̃G���A�̍ő哞�B�l")]
    public int reachStageNumber;

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
    [Header("�G���A�ԍ�")]
    public AreaNumber areaNumber;

    [Header("�X�e�[�W�p�l��:�@��l����")]
    public float dragSensitivity; //[Header("���슴�x")]    
    public float maxValue_Scale; //[Header("�傫��")] 
    public float rangeOfInfluence_Scale; //[Header("�e���͈�")]
    Vector2 dragLength;
    Vector2 screenFactor;
    Vector2 oldPosition, currentPosition;
    private float factorScale;

    [Header("�X�e�[�W�p�l��:���\�D���")]
    public float dragSensitivityS;

    [Header("�C�[�W���O-Magnet-")]
    public float easeDuration;
    public Ease easeType;

    [Header("�h���b�O�`�A�j���[�V���������ǂ���")]
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
        reachStageNumber = informationMg.data.courseDate[(int)areaNumber];
        nearestStageNumber = reachStageNumber;//1/24:�ŏ��̃h���b�O���ɁAChangeTarget()���N�������Ȃ��悤�ɂ��邽�߂̏���
        InitializedStagePosition(informationMg.data.recentStageNum);

        factorScale = 1 / Mathf.Pow(rangeOfInfluence_Scale, 2);
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);

        //2022.10.28 :���I�u�W�F�N�g�̔z��v�f����R�[�X�ԍ����擾���R�[�X�ԍ�����X�e�[�W�ő哞�B�l�̒l�����o�����e�R�[�X�̍ő哞�B�X�e�[�W�ֈړ����鏈��
        //2022.11.02 :Title,Loading�̃V�[����Gmanager������� 
    }

    private void Update()
    {
        Debug.Log(NearestStageInfo());
        //UpdateItemsScale();
        if (!isDragging)
        {
            return;
        }

        if (nearestStageNumber != NearestStageInfo().stageNum)
        {
            StageInformation stageInfo = NearestStageInfo();
            informationMg.UpdateStageInformation(stageInfo);
            informationMg.stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);

            nearestStageNumber = stageInfo.stageNum;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = eventData.position * screenFactor;
        //tweenList.KillAllAndClear();
        isDragging = true;
    }

    /*
    public void OnDrag(PointerEventData eventData)// ����ʂɉ�����XY�����Ɉړ�����
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
        float delta_y = (e.delta.y * dragSensitivityS);// ����ʂɉ�����Y�����Ɉړ�����
        float delta_x = (e.delta.x * dragSensitivityS);// ����ʂɉ�����Y�����Ɉړ�����

        foreach (var item in this.stageInfoList)
        {
            RectTransform rect = item.RectTransform;
            var pos = rect.anchoredPosition;
            pos.y += delta_y;
            pos.x += delta_x;
            rect.anchoredPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData e)//����������߂��ʒu�Ɉ����񂹂郍�W�b�N
    {
        StageInformation nearStageInfo = NearestStageInfo();
        informationMg.UpdateStageInformation(nearStageInfo);
        informationMg.stageFrameMg.RectTransform.anchoredPosition = nearStageInfo.RectTransform.anchoredPosition;//2023.03.27:�ړIpos�܂ňړ�
        MagnetItems(nearStageInfo.RectTransform.anchoredPosition);

    }

    private void DragComplete() => isDragging = false;

    public StageInformation NearestStageInfo()// �}�O�l�b�g���S�ɍł��߂��v�f��I������
    {
        StageInformation stageInfo = null;
        RectTransform nearestRect = null;
        for (int i = 0; i < stageInfoList.Count; i++)
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
        nearestStageNumber = stageInfo.stageNum;
        informationMg.UpdateStageInformation(stageInfo);
        informationMg.stageFrameMg.ChangeTarget(stageInfo.stageLevel, stageInfo.tips);
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

    public void InitializedStagePosition(int initialValue) //����:�X�e�[�W�ő哞�B�l
    {
        /*
        stageInfoList.AddRange(linearList);
        stageInfoList.AddRange(scatterList);
         */

        if (informationMg.data.recentCourseNum == (int)areaNumber)
        {
            //���߂̃R�[�X�G���A��I��=>���߂̃X�e�[�W�ʒu�ֈړ�
            Vector2 target = stageInfoList[informationMg.data.recentStageNum].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }
        else
        {
            //���̑��R�[�X�G���A�͍ő哞�B�X�e�[�W�ʒu�ֈړ�
            Vector2 target = stageInfoList[initialValue].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }

        informationMg.UpdateStageInformation(stageInfoList[informationMg.data.recentStageNum]);
    }

}