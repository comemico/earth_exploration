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

    [Header("�G���A�ԍ�")]
    [Range(0, 9)] public int areaNum;
    public int stageAdress; //�I�����ꂽ�X�e�[�W��stageInfoList�̉��Ԗڂɂ��邩
    //public int reachStageNumber; //���̃G���A�̍ő哞�B�l
    StageInformation nearStageInfo;

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
        nearStageInfo = stageInfoList[informationMg.data.recentStageAdress];//1/24:�ŏ��̃h���b�O���ɁAChangeTarget()���N�������Ȃ��悤�ɂ��邽�߂̏���

        factorScale = 1 / Mathf.Pow(rangeOfInfluence_Scale, 2);
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);

        //2022.10.28 :���I�u�W�F�N�g�̔z��v�f����R�[�X�ԍ����擾���R�[�X�ԍ�����X�e�[�W�ő哞�B�l�̒l�����o�����e�R�[�X�̍ő哞�B�X�e�[�W�ֈړ����鏈��
        //2022.11.02 :Title,Loading�̃V�[����Gmanager������� 
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
        stageAdress = stageInfoList.IndexOf(nearStageInfo);
        informationMg.UpdateStageInformation(nearStageInfo, stageAdress);
        informationMg.stageFrameMg.RectTransform.anchoredPosition = nearStageInfo.RectTransform.anchoredPosition;//2023.03.27:�ړIpos�܂ňړ�
        MagnetItems(nearStageInfo.RectTransform.anchoredPosition);

    }

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
                if (Vector2.SqrMagnitude(stageInfoList[i].RectTransform.anchoredPosition) < Vector2.SqrMagnitude(nearestRect.anchoredPosition) && stageInfoList[i].isDiscover == true)//��Βl�I��
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
    private void DragComplete() => isDragging = false;

    public void InitializedStagePosition(int stageAdress) //����:�X�e�[�W�ő哞�B�l
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
            //���߂̃R�[�X�G���A��I��=>���߂̃X�e�[�W�ʒu�ֈړ�=>recent
            Vector2 target = stageInfoList[informationMg.data.recentStageAdress].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }
        else
        {
            //���̑��R�[�X�G���A�͍ő哞�B�X�e�[�W�ʒu�ֈړ�=>course[areaNum] 
            Vector2 target = stageInfoList[informationMg.data.linearData[areaNum]].RectTransform.anchoredPosition;
            for (int i = 0; i < stageInfoList.Count; i++)
            {
                stageInfoList[i].RectTransform.anchoredPosition -= target;
            }
        }

        informationMg.UpdateStageInformation(stageInfoList[informationMg.data.recentStageAdress], stageAdress);
    }

}