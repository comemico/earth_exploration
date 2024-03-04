using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class MemoryGageManager : MonoBehaviour
{
    [Header("パネル")]
    public RectTransform lifePanelUp;
    public RectTransform lifePanelDown;
    public float initialValue;//[Header("初期値")]
    public float memorySpace;//[Header("メモリの間隔")]

    [Header("メモリ")]
    public Image[] maxLifeBox; //[Header("上限メモリ配列")] 
    public Image[] lifeMemoryFollowerBox; //[Header("ライフメモリ追従配列")]     
    public Image[] lifeMemoryBox; //[Header("ライフメモリ配列")]
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

    [Header("その他")]
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
            //箱内の全Imageをfalseにする
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
            //上限メモリ表示制限
            memoryGage = MEMORY_LENGTH;
        }

        //メモリパネルの長さ設定
        if (maxLifeNum > MEMORY_LENGTH / 2)
        {
            //下部パネルの増設
            lifePanelDown.gameObject.SetActive(true);
            lifePanelDown.sizeDelta = new Vector2(initialValue + ((maxLifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y);
        }
        else
        {
            //上部パネルの増設
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
        //連続で呼ばれてもいいように前回のTweenをKill
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
        //メモリパネルの長さ設定
        if (LifeNum > MEMORY_LENGTH / 2)
        {
            //下部パネルの増設
            lifePanelDown.gameObject.SetActive(true);
            seq_exceed.Append(lifePanelDown.DOSizeDelta(new Vector2(initialValue + ((LifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y), panelDuration).SetEase(panelType));
        }
        else
        {
            //上部パネルの増設
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
