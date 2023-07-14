using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryCountManager_Unused : MonoBehaviour
{
    public RectTransform jumpRect;
    public RectTransform slideRect;
    public float jumpPositionY;

    [Space(20)]

    [Header("�C�[�W���O")]
    public Ease easeType;
    public float duration;

    private RectTransform mid;
    private Text oddNum;
    private Text evenNum;

    public void InitializeMemoryCount(int memoryNum)
    {
        oddNum = jumpRect.gameObject.transform.GetChild(0).GetComponent<Text>();
        evenNum = slideRect.gameObject.transform.GetChild(0).GetComponent<Text>();
        evenNum.text = memoryNum.ToString();

        int num = memoryNum;
        num--;

        if (num % 2 == 0)//����
        {
            evenNum.text = num.ToString();
        }
        else //�
        {
            mid = jumpRect;
            jumpRect = slideRect;
            slideRect = mid;
            oddNum.text = num.ToString();
        }

    }

    public void DownButton(int memoryNum)
    {
        //�Đ��r���ɌĂ΂ꂽ�ꍇ�A�J�n�ʒu������������
        jumpRect.DOComplete();
        slideRect.DOComplete();

        if (memoryNum % 2 == 0)//����
        {
            evenNum.text = memoryNum.ToString();
        }
        else //�
        {
            oddNum.text = memoryNum.ToString();
        }

        //��ʒu�Ɉړ�
        jumpRect.anchoredPosition = new Vector2(0f, jumpPositionY);

        //���������փX���C�h
        jumpRect.DOAnchorPosY(-100f, duration).SetEase(easeType).SetRelative(true);
        slideRect.DOAnchorPosY(-100f, duration).SetEase(easeType).SetRelative(true);

        //�Q�ƌ������ւ���
        mid = jumpRect;
        jumpRect = slideRect;
        slideRect = mid;
    }

    public void UpButton(int memoryNum)
    {
        //�Đ��r���ɌĂ΂ꂽ�ꍇ�A�J�n�ʒu������������
        jumpRect.DOComplete();
        slideRect.DOComplete();

        if (memoryNum % 2 == 0)//����
        {
            evenNum.text = memoryNum.ToString();
        }
        else //�
        {
            oddNum.text = memoryNum.ToString();
        }

        //���ʒu�Ɉړ�
        jumpRect.anchoredPosition = new Vector2(0f, jumpPositionY * -1);
        //jumpRect.anchoredPosition = rectDown.anchoredPosition;

        //��������փX���C�h
        jumpRect.DOAnchorPosY(100f, duration).SetEase(easeType).SetRelative(true);
        slideRect.DOAnchorPosY(100f, duration).SetEase(easeType).SetRelative(true);

        //�Q�ƌ������ւ���
        mid = jumpRect;
        jumpRect = slideRect;
        slideRect = mid;
    }

}
