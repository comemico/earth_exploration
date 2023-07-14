using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MemoryGageManager_Unused : MonoBehaviour
{
    [Space(20)]
    [Header("�������ɂ���")]
    [Header("����������z��")]
    public GameObject maxLifeObject;
    public Image[] maxLifeBox;
    [Header("���C�t�������Ǐ]�z��")]
    public GameObject lifeMemoryFollowerObject;
    public Image[] lifeMemoryFollowerBox;
    [Header("���C�t�������z��")]
    public GameObject lifeMemoryObject;
    public Image[] lifeMemoryBox;

    [Space(20)]
    [Header("�p�l���ɂ���")]
    [Header("�p�l��")] public RectTransform lifePanelUp;
    [Header("�p�l��")] public RectTransform lifePanelDown;
    [Header("�����l")] public float initialValue;
    [Header("�������̊Ԋu")] public float memorySpace;

    [Space(20)]
    [Header("���̑�")]
    [Header("�J�ڎ���_Blue")] public float blueTime = 3.0f;
    [Header("�J�ڎ���_Red")] public float redTime = 3.0f;
    [Header("SomerSaultCount")] public GetMemoryManager getMemoryMg;
    [Header("MemoryCountText")] public Text memoryCountText;
    [Header("SomerSaultCount")] public MemoryCountManager memoryCountMg;

    public int memoryGage = 0;
    private int oldMemory = 0;
    //private int oldReciveGageMemory = 0;

    private Tween decreaseTween = null;
    private Vector3 initScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 defaultScale = new Vector3(0.35f, 0.35f, 0.35f);


    private void Awake()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 500, sequencesCapacity: 50);
    }

    private void Start()
    {
        InitializeMemoryGage();
    }

    private void Update()
    {
        if (oldMemory != memoryGage)
        {
            DotweenRedGage(oldMemory, memoryGage, 0.3f);
            oldMemory = memoryGage;
        }
    }

    public void InitializeMemoryGage()
    {
        //�����擾
        maxLifeBox = new Image[(maxLifeObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        lifeMemoryFollowerBox = new Image[(lifeMemoryFollowerObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        lifeMemoryBox = new Image[(lifeMemoryObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];

        for (int i = 0; i < maxLifeBox.Length; i++)
        {
            //����Image�R���|�[�l���g������
            if (i < maxLifeObject.transform.GetChild(0).childCount)
            {
                maxLifeBox[i] = maxLifeObject.transform.GetChild(0).GetChild(i).GetComponent<Image>();
                lifeMemoryFollowerBox[i] = lifeMemoryFollowerObject.transform.GetChild(0).GetChild(i).GetComponent<Image>();
                lifeMemoryBox[i] = lifeMemoryObject.transform.GetChild(0).GetChild(i).GetComponent<Image>();
            }
            else
            {
                maxLifeBox[i] = maxLifeObject.transform.GetChild(1).GetChild(i - maxLifeObject.transform.GetChild(0).childCount).GetComponent<Image>();
                lifeMemoryFollowerBox[i] = lifeMemoryFollowerObject.transform.GetChild(1).GetChild(i - maxLifeObject.transform.GetChild(0).childCount).GetComponent<Image>();
                lifeMemoryBox[i] = lifeMemoryObject.transform.GetChild(1).GetChild(i - maxLifeObject.transform.GetChild(0).childCount).GetComponent<Image>();
            }
            //�����̑SImage��false�ɂ���
            maxLifeBox[i].enabled = false;
            lifeMemoryFollowerBox[i].enabled = false;
            lifeMemoryBox[i].enabled = false;
        }

        //����������\������
        if (GManager.instance.maxLifeNum > maxLifeBox.Length)
        {
            Debug.Log("�����Image���s����Ԃł�");
            GManager.instance.maxLifeNum = maxLifeBox.Length;
        }
        for (int i = 0; i < GManager.instance.maxLifeNum; i++)
        {
            maxLifeBox[i].enabled = true;
        }

        //�������p�l���̒����ݒ�
        if (GManager.instance.maxLifeNum > maxLifeObject.transform.GetChild(0).childCount)
        {
            lifePanelDown.gameObject.SetActive(true);
            lifePanelDown.sizeDelta = new Vector2(initialValue + ((GManager.instance.maxLifeNum - maxLifeObject.transform.GetChild(0).childCount) * memorySpace), lifePanelDown.sizeDelta.y);
        }
        else
        {
            lifePanelDown.gameObject.SetActive(false);
            lifePanelUp.sizeDelta = new Vector2(initialValue + (GManager.instance.maxLifeNum * memorySpace), lifePanelUp.sizeDelta.y);
        }
        /*
        //���������\���ݒ�
        if (GManager.instance.lifeNum > GManager.instance.maxLifeNum)
        {
            GManager.instance.lifeNum = GManager.instance.maxLifeNum;
            Debug.Log("����𒴂������߁Amax�l�ɕύX");
        }
        memoryGage = GManager.instance.lifeNum;
         */
        memoryGage = GManager.instance.maxLifeNum;
        memoryCountMg.InitializeMemoryCount(memoryGage);
        DisplayMemoryGage(memoryGage);
    }

    public void DotweenBlueGage(int nowMemory, int nextMemory, float time)
    {
        DOTween.Kill(decreaseTween);
        decreaseTween = DOTween.To(() => nowMemory, (val) =>
        {
            //Debug.Log(val);
            DisplayMemoryGage(val);
            memoryGage = val;

        }, nextMemory, time);
    }

    public void DisplayMemoryGage(int receiveGageMemory)
    {
        for (int i = 0; i < lifeMemoryBox.Length; i++)
        {
            lifeMemoryBox[i].enabled = (receiveGageMemory > i);
            /*
            if (receiveGageMemory == i + 1)
            {
                lifeMemoryBox[i].rectTransform.localScale = initScale;
                lifeMemoryBox[i].rectTransform.DOScale(defaultScale, 0.12f).SetEase(Ease.OutBack, defaultScale.x * 20f);
                lifeMemoryFollowerBox[i].rectTransform.localScale = initScale;
                lifeMemoryFollowerBox[i].rectTransform.DOScale(defaultScale, 0.12f).SetEase(Ease.OutBack, defaultScale.x * 20f);
            }
             */
        }
        memoryCountMg.DisplayMemoryNumber(receiveGageMemory);

        /*
        if (oldReciveGageMemory != receiveGageMemory)
        {
            if (oldReciveGageMemory > receiveGageMemory)
            {
                memoryCountMg.DisplayMemoryNumber(receiveGageMemory);
                //memoryCountMg.DownButton(receiveGageMemory);
            }
            else
            {
                memoryCountMg.DisplayMemoryNumber(receiveGageMemory);
                //memoryCountMg.UpButton(receiveGageMemory);
            }
            oldReciveGageMemory = receiveGageMemory;
        }
         */

    }

    public void DotweenRedGage(int nowMemory, int nextMemory, float time)
    {
        DOTween.Kill(decreaseTween);
        decreaseTween = DOTween.To(() => nowMemory, (val) =>
        {
            DisplayRedMemoryGage(val);

        }, nextMemory, time);
    }

    public void DisplayRedMemoryGage(int memory)
    {
        for (int i = 0; i < lifeMemoryFollowerBox.Length; i++)
        {
            lifeMemoryFollowerBox[i].enabled = (memory > i);
        }
    }

    /*
    public int RecPassedMemoryGage()
    {
        if (memoryGage > 0)
        {
            passedMemoryGage = memoryGage;
        }
        return passedMemoryGage;
    }
     */
}
