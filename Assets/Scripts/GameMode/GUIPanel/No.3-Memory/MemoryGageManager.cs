using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [Header("その他")]
    public MemoryCountManager memoryCountMg;
    public int memoryGage = 0;

    int oldMemory = 0;
    Tween decreaseTween;

    private void Start()
    {
        InitializeMemoryGage();
        //DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 500, sequencesCapacity: 50);
    }

    private void Update()
    {
        if (oldMemory != memoryGage)
        {
            DotweenRedGage(oldMemory, memoryGage, followDuration);
            oldMemory = memoryGage;
        }
    }

    public void InitializeMemoryGage()
    {
        for (int i = 0; i < MEMORY_LENGTH; i++)
        {
            //箱内の全Imageをfalseにする
            maxLifeBox[i].enabled = false;
            lifeMemoryFollowerBox[i].enabled = false;
            lifeMemoryBox[i].enabled = false;
        }

        for (int i = 0; i < GManager.instance.maxLifeNum; i++)
        {
            maxLifeBox[i].enabled = true;
            lifeMemoryFollowerBox[i].enabled = true;
            lifeMemoryBox[i].enabled = true;
        }

        if (GManager.instance.maxLifeNum > MEMORY_LENGTH)
        {
            //上限メモリ表示制限
            GManager.instance.maxLifeNum = MEMORY_LENGTH;
        }

        //メモリパネルの長さ設定
        if (GManager.instance.maxLifeNum > MEMORY_LENGTH / 2)
        {
            //下部パネルの増設
            lifePanelDown.gameObject.SetActive(true);
            lifePanelDown.sizeDelta = new Vector2(initialValue + ((GManager.instance.maxLifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y);
        }
        else
        {
            //上部パネルの増設
            lifePanelDown.gameObject.SetActive(false);
            lifePanelUp.sizeDelta = new Vector2(initialValue + (GManager.instance.maxLifeNum * memorySpace), lifePanelUp.sizeDelta.y);
        }

        memoryGage = GManager.instance.maxLifeNum;
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

    public void ExceedLimit()
    {
        Debug.Log("ExceedLimit");
    }

}
