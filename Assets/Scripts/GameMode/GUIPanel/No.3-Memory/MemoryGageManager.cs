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

    [Header("���̑�")]
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
            //�����̑SImage��false�ɂ���
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
            //����������\������
            GManager.instance.maxLifeNum = MEMORY_LENGTH;
        }

        //�������p�l���̒����ݒ�
        if (GManager.instance.maxLifeNum > MEMORY_LENGTH / 2)
        {
            //�����p�l���̑���
            lifePanelDown.gameObject.SetActive(true);
            lifePanelDown.sizeDelta = new Vector2(initialValue + ((GManager.instance.maxLifeNum - (MEMORY_LENGTH / 2)) * memorySpace), lifePanelDown.sizeDelta.y);
        }
        else
        {
            //�㕔�p�l���̑���
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

    public void ExceedLimit()
    {
        Debug.Log("ExceedLimit");
    }

}
