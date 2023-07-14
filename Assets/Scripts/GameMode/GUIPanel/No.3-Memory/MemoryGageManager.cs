using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryGageManager : MonoBehaviour
{
    public MemoryCountManager memoryCountMg;
    public GetMemoryManager getMemoryMg;

    [Header("メモリ")]
    public GameObject memoryBox;
    [Header("上限メモリ配列")]
    private GameObject maxLifeObject;
    private Image[] maxLifeBox;
    [Header("ライフメモリ追従配列")]
    private GameObject lifeMemoryFollowerObject;
    private Image[] lifeMemoryFollowerBox;
    [Header("ライフメモリ配列")]
    private GameObject lifeMemoryObject;
    private Image[] lifeMemoryBox;

    [Space(20)]
    [Header("パネルについて")]
    [Header("パネル")] public GameObject lifePanelObject;
    [Header("パネル:上")] private RectTransform lifePanelUp;
    [Header("パネル:下")] private RectTransform lifePanelDown;
    [Header("初期値")] public float initialValue;
    [Header("メモリの間隔")] public float memorySpace;

    [Space(20)]
    [Header("その他")]
    public Ease easeType;
    public float followDuration;

    public int memoryGage = 0;
    private int oldMemory = 0;
    private Tween decreaseTween = null;
    /*
    private void Awake()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 500, sequencesCapacity: 50);
    }
     */

    private void Start()
    {
        InitializeMemoryGage();
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
        //メモリオブジェクト
        maxLifeObject = memoryBox.transform.GetChild(0).gameObject;
        lifeMemoryFollowerObject = memoryBox.transform.GetChild(1).gameObject;
        lifeMemoryObject = memoryBox.transform.GetChild(2).gameObject;
        //箱数取得
        maxLifeBox = new Image[(maxLifeObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        lifeMemoryFollowerBox = new Image[(lifeMemoryFollowerObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        lifeMemoryBox = new Image[(lifeMemoryObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        //パネル
        lifePanelUp = lifePanelObject.transform.GetChild(0).GetComponent<RectTransform>();
        lifePanelDown = lifePanelObject.transform.GetChild(1).GetComponent<RectTransform>();

        for (int i = 0; i < maxLifeBox.Length; i++)
        {
            //箱にImageコンポーネントを入れる
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
            //箱内の全Imageをfalseにする
            maxLifeBox[i].enabled = false;
            lifeMemoryFollowerBox[i].enabled = false;
            lifeMemoryBox[i].enabled = false;
        }

        //上限メモリ表示制限
        if (GManager.instance.maxLifeNum > maxLifeBox.Length)
        {
            Debug.Log("上限のImageが不足状態です");
            GManager.instance.maxLifeNum = maxLifeBox.Length;
        }
        for (int i = 0; i < GManager.instance.maxLifeNum; i++)
        {
            maxLifeBox[i].enabled = true;
        }

        //メモリパネルの長さ設定
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

        memoryGage = GManager.instance.maxLifeNum;
        memoryCountMg.InitializeMemoryCount(memoryGage);
        DisplayMemoryGage(memoryGage);
        /*
        //メモリ数表示設定
        if (GManager.instance.lifeNum > GManager.instance.maxLifeNum)
        {
            GManager.instance.lifeNum = GManager.instance.maxLifeNum;
            Debug.Log("上限を超えたため、max値に変更");
        }
         */
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

}
