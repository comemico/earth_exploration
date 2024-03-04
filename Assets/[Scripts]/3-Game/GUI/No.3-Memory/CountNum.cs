using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CountNum : MonoBehaviour
{
    public Text count;
    private int countNum;
    public Ease easeType;

    public void PressButtonCount()
    {
        Initialized();
        countNum++;
        count.text = countNum.ToString();
        count.DOFade(1, 0.15f);
        count.transform.DOScale(1f, 0.2f).SetEase(easeType);
    }

    private void Initialized()
    {
        count.rectTransform.localScale = new Vector3(0.6f, 0.6f, 1f);
        count.color = new Color(1f, 1f, 1f, 0.3f);
    }

}

