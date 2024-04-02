using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

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
    public CanvasGroup exceedCanvas;
    public Image lampImg;
    [Range(0.2f, 1f)] public float panelDuration;
    public Ease panelType;
    public Ease lampLoopType;
    public Color lampColor;
    public Material bloom;

    [Header("���̑�")]
    public MemoryCountManager memoryCountMg;
    public int memoryGage = 0;

    int oldMemory = 0;
    Tween decreaseTween;
    List<Tween> tweenList = new List<Tween>();

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
        maxLifeBox[LifeNum - 1].enabled = true;
        maxLifeBox[LifeNum - 1].color = lampColor;
        maxLifeBox[LifeNum - 1].rectTransform.anchoredPosition = new Vector2(maxLifeBox[LifeNum - 1].rectTransform.anchoredPosition.x - memorySpace, maxLifeBox[LifeNum - 1].rectTransform.anchoredPosition.y);
        maxLifeBox[LifeNum - 1].material = bloom;

        exceedCanvas.transform.localRotation = Quaternion.Euler(0f, 0f, 60f);

        Sequence seq_exceed = DOTween.Sequence();
        //�������p�l���̒����ݒ�
        if (LifeNum > MEMORY_LENGTH / 2)
        {
            //�����p�l���̑���
            lifePanelDown.gameObject.SetActive(true);
            seq_exceed.Append(lifePanelDown.DOSizeDelta(new Vector2(initialValue + ((LifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y), panelDuration).SetEase(panelType));
        }
        else
        {
            //�㕔�p�l���̑���
            lifePanelDown.gameObject.SetActive(false);
            seq_exceed.Append(lifePanelUp.DOSizeDelta(new Vector2(initialValue + (LifeNum * memorySpace), lifePanelUp.sizeDelta.y), panelDuration).SetEase(panelType));
        }

        seq_exceed.Join(maxLifeBox[LifeNum - 1].rectTransform.DOAnchorPosX(memorySpace, panelDuration).SetRelative(true).SetEase(panelType));
        seq_exceed.Join(exceedCanvas.DOFade(1f, 0.5f).SetEase(Ease.OutBack));
        seq_exceed.Join(exceedCanvas.transform.DOLocalRotate(Vector3.zero, 0.55f).SetEase(Ease.OutBack));
        seq_exceed.AppendCallback(() =>
        {
            Tween loop = maxLifeBox[LifeNum - 1].DOFade(1f, 0.65f).SetLoops(-1, LoopType.Yoyo).SetEase(lampLoopType);
            memoryCountMg.DisplayMemoryNumber(LifeNum);
            tweenList.Add(loop);
            lampImg.enabled = true;
        });

        tweenList.Add(seq_exceed);
    }

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }
}
