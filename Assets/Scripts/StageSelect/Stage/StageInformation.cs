using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformation : MonoBehaviour
{
    public enum StageType
    {
        Linear = 0,
        Scatter
    }
    public StageType stageType;

    public enum StageMode
    {
        Normal = 0,
        Exceed
    }
    public StageMode stageMode;

    public bool isDiscover;
    public bool isClear;

    public Button button;
    public Image stageLamp;
    [Range(0, 10)]
    public int stageNum;
    [Range(1, 37)]
    public int stageLevel;

    public string tips;

    public RectTransform RectTransform => this.transform as RectTransform;
    StageController stageCtrl;
    StageInformation stageInfo;

    private void Awake()
    {
        button = transform.GetChild(1).GetComponent<Button>();
        stageLamp = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        stageCtrl = GetComponentInParent<StageController>();
        stageInfo = GetComponent<StageInformation>();
        button.onClick.AddListener(StageClick);
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
            stageLamp.enabled = true;
            stageLamp.color = new Color(0.090f, 0.575f, 0.600f, 1f);
            if (isClear)
            {
                //�I�����W�F
                stageLamp.color = new Color(0.902f, 0.196f, 0.090f, 1f);
            }
        }
        else
        {
            if (stageMode == StageMode.Normal)
            {
                stageLamp.enabled = false;
                //��
            }
            else
            {
                this.gameObject.SetActive(false);
                //�{�^�����Ɣ�\��
            }
            //�{�^���������Ȃ�������
            button.interactable = false;
        }
    }

    public void CheckInteractible(bool interactible)
    {
        GetComponentInChildren<Button>().interactable = interactible;
    }

}
