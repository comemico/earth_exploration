using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryGageManager : MonoBehaviour
{
    [Header("�p�l��")]
    public RectTransform lifePanelUp;
    public RectTransform lifePanelDown;
    public float initialValue;//[Header("�����l")]
    public float memorySpace;//[Header("�������̊Ԋu")]

    [Header("������")]
    public Image[] maxLifeBox; //[Header("����������z��")] 
    public Image[] lifeMemoryFollowerBox; //[Header("���C�t�������Ǐ]�z��")]     
    public Image[] lifeMemoryBox; //[Header("���C�t�������z��")]
    const int MEMORY_LENGTH = 40;
    public float followDuration;
    public Ease easeType;

    [Header("ExceedLimit")]
    [Range(0.2f, 1f)] public float exceedDuration;
    public Ease exceedType;
    public Ease exceedLampType;
    public Color bloomColor;
    public Material bloom;
    Tween tweenLamp;

    [Header("���̑�")]
    public MemoryCountManager memoryCountMg;
    public int memoryGage = 0;

    int oldMemory = 0;
    Tween decreaseTween;

    private void Update()
    {
        if (oldMemory != memoryGage)
        {
            DotweenRedGage(oldMemory, memoryGage, followDuration);
            oldMemory = memoryGage;
        }
    }

    public void InitializeMemoryGage(int maxLifeNum)
    {
        for (int i = 0; i < MEMORY_LENGTH; i++)
        {
            //�����̑SImage��false�ɂ���
            maxLifeBox[i].enabled = false;
            lifeMemoryFollowerBox[i].enabled = false;
            lifeMemoryBox[i].enabled = false;
        }

        for (int i = 0; i < maxLifeNum; i++)
        {
            maxLifeBox[i].enabled = true;
            lifeMemoryFollowerBox[i].enabled = true;
            lifeMemoryBox[i].enabled = true;
        }

        if (maxLifeNum > MEMORY_LENGTH)
        {
            //����������\������
            memoryGage = MEMORY_LENGTH;
        }

        //�������p�l���̒����ݒ�
        if (maxLifeNum > MEMORY_LENGTH / 2)
        {
            //�����p�l���̑���
            lifePanelDown.gameObject.SetActive(true);
            lifePanelDown.sizeDelta = new Vector2(initialValue + ((maxLifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y);
        }
        else
        {
            //�㕔�p�l���̑���
            lifePanelDown.gameObject.SetActive(false);
            lifePanelUp.sizeDelta = new Vector2(initialValue + (maxLifeNum * memorySpace), lifePanelUp.sizeDelta.y);
        }

        memoryGage = maxLifeNum;
        memoryCountMg.InitializeMemoryCount(memoryGage);
    }

    public void DisplayMemoryGage(int receiveGageMemory)
    {
        for (int i = 0; i < lifeMemoryBox.Length; i++)
        {
            lifeMemoryBox[i].enabled = (receiveGageMemory > i);
        }
        memoryCountMg.DisplayMemoryNumber(receiveGageMemory);
    }

    public void DotweenRedGage(int nowMemory, int nextMemory, float time)
    {
        //�A���ŌĂ΂�Ă������悤�ɑO���Tween��Kill
        DOTween.Kill(decreaseTween);

        decreaseTween = DOTween.To(() => nowMemory, (val) =>
           {
               DisplayRedMemoryGage(val);
           }, nextMemory, time).SetEase(easeType);
    }

    public void DisplayRedMemoryGage(int memory)
    {
        for (int i = 0; i < lifeMemoryFollowerBox.Length; i++)
        {
            lifeMemoryFollowerBox[i].enabled = (memory > i);
        }
    }

    public void ExceedLimit(int LifeNum)
    {
        Debug.Log("ExceedLimit");
        //�������p�l���̒����ݒ�
        if (LifeNum > MEMORY_LENGTH / 2)
        {
            //�����p�l���̑���
            lifePanelDown.gameObject.SetActive(true);
            lifePanelDown.DOSizeDelta(new Vector2(initialValue + ((LifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y), exceedDuration).SetEase(exceedType);
        }
        else
        {
            //�㕔�p�l���̑���
            lifePanelDown.gameObject.SetActive(false);
            lifePanelUp.DOSizeDelta(new Vector2(initialValue + (LifeNum * memorySpace), lifePanelUp.sizeDelta.y), exceedDuration).SetEase(exceedType);
        }

        maxLifeBox[LifeNum - 1].rectTransform.anchoredPosition = new Vector2(maxLifeBox[LifeNum - 1].rectTransform.anchoredPosition.x - memorySpace, maxLifeBox[LifeNum - 1].rectTransform.anchoredPosition.y);
        maxLifeBox[LifeNum - 1].enabled = true;
        maxLifeBox[LifeNum - 1].color = bloomColor;
        maxLifeBox[LifeNum - 1].material = bloom;

        tweenLamp = maxLifeBox[LifeNum - 1].rectTransform.DOAnchorPosX(memorySpace, 0.25f).SetRelative(true).SetDelay(0.5f);
        tweenLamp = maxLifeBox[LifeNum - 1].DOFade(1f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(exceedLampType).SetDelay(0.75f);

        memoryCountMg.InitializeMemoryCount(LifeNum);
    }
    private void OnDestroy()
    {
        tweenLamp.Kill(true);
    }
}
