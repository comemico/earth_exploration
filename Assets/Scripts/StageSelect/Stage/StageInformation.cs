using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformation : MonoBehaviour
{
    public enum StageType
    {
        Normal = 0,
        Exceed,
        Special,
    }

    public StageType stageType;
    public bool isDiscover;
    public bool isClear;
    public Image stageLamp;
    [Range(0, 10)]
    public int stageNum;
    [Range(1, 37)]
    public int stageLevel;

    public string tips;

    public RectTransform RectTransform => this.transform as RectTransform;
    StageController stageCtrl;
    StageInformation stageInfo;

    void Start()
    {
        stageLamp = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        stageCtrl = GetComponentInParent<StageController>();
        stageInfo = GetComponent<StageInformation>();
        GetComponentInChildren<Button>().onClick.AddListener(StageClick);
    }

    void StageClick()
    {
        stageCtrl.MoveStage(stageInfo);
    }

    public void JudgeLamp() //�Ăԃ^�C�~���O�́Adata����estageInfo�ɑ�����ꂽ��ɍs�Ȃ�
    {
        if (isDiscover)
        {
            //�F
            if (isClear)
            {
                //�I�����W�F
            }
        }
        else
        {
            if (stageType == StageType.Normal)
            {
                stageLamp.enabled = false;
                //��
            }
            else
            {
                this.gameObject.SetActive(false);
                //�{�^�����Ɣ�\��
            }
        }
    }

    public void CheckInteractible(bool interactible)
    {
        GetComponentInChildren<Button>().interactable = interactible;
    }

}
