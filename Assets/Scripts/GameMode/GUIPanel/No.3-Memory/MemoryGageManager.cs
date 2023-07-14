using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryGageManager : MonoBehaviour
{
    public MemoryCountManager memoryCountMg;
    public GetMemoryManager getMemoryMg;

    [Header("������")]
    public GameObject memoryBox;
    [Header("����������z��")]
    private GameObject maxLifeObject;
    private Image[] maxLifeBox;
    [Header("���C�t�������Ǐ]�z��")]
    private GameObject lifeMemoryFollowerObject;
    private Image[] lifeMemoryFollowerBox;
    [Header("���C�t�������z��")]
    private GameObject lifeMemoryObject;
    private Image[] lifeMemoryBox;

    [Space(20)]
    [Header("�p�l���ɂ���")]
    [Header("�p�l��")] public GameObject lifePanelObject;
    [Header("�p�l��:��")] private RectTransform lifePanelUp;
    [Header("�p�l��:��")] private RectTransform lifePanelDown;
    [Header("�����l")] public float initialValue;
    [Header("�������̊Ԋu")] public float memorySpace;

    [Space(20)]
    [Header("���̑�")]
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
        //�������I�u�W�F�N�g
        maxLifeObject = memoryBox.transform.GetChild(0).gameObject;
        lifeMemoryFollowerObject = memoryBox.transform.GetChild(1).gameObject;
        lifeMemoryObject = memoryBox.transform.GetChild(2).gameObject;
        //�����擾
        maxLifeBox = new Image[(maxLifeObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        lifeMemoryFollowerBox = new Image[(lifeMemoryFollowerObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        lifeMemoryBox = new Image[(lifeMemoryObject.transform.GetChild(0).childCount) + (maxLifeObject.transform.GetChild(1).childCount)];
        //�p�l��
        lifePanelUp = lifePanelObject.transform.GetChild(0).GetComponent<RectTransform>();
        lifePanelDown = lifePanelObject.transform.GetChild(1).GetComponent<RectTransform>();

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

        memoryGage = GManager.instance.maxLifeNum;
        memoryCountMg.InitializeMemoryCount(memoryGage);
        DisplayMemoryGage(memoryGage);
        /*
        //���������\���ݒ�
        if (GManager.instance.lifeNum > GManager.instance.maxLifeNum)
        {
            GManager.instance.lifeNum = GManager.instance.maxLifeNum;
            Debug.Log("����𒴂������߁Amax�l�ɕύX");
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
