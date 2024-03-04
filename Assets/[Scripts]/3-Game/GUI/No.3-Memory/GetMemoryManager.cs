using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GetMemoryManager : MonoBehaviour
{
    [Header("RectTransform")] public RectTransform rectTransform;
    [Header("CanvasGroup")] public CanvasGroup canvasGroup;
    [Header("+")] public Text plus;

    [Header("SomerSaultCount")] public Text somerSaultCount;
    public Ease easeType;
    private bool isAppear;

    public void AppearPlusMark()
    {
        if (!isAppear)
        {
            somerSaultCount.color = new Color(1f, 1f, 1f, 0f);
            canvasGroup.DOFade(1, 0.25f).SetEase(easeType);
            rectTransform.DOAnchorPos(new Vector2(0f, 0f), 0.25f).SetEase(easeType);
            isAppear = true;
        }
    }

    public void DisAppeearPlusMark()
    {
        canvasGroup.DOFade(0, 0.1f).SetDelay(0.75f).SetEase(easeType);
        rectTransform.DOAnchorPos(new Vector2(-70f, 0f), 0.15f).SetDelay(0.75f).SetEase(easeType);

        isAppear = false;
    }

    public void DisplaySomerSaultCount(int somersaultcount)
    {
        somerSaultCount.DOComplete();
        Initialized();
        somerSaultCount.text = somersaultcount.ToString();
        somerSaultCount.DOFade(1, 0.15f).SetEase(easeType);
        somerSaultCount.transform.DOScale(1f, 0.2f).SetEase(Ease.OutElastic);//OutBackもあり
    }
    //振れ幅は、SetEaseメソッドの第2引数で指定することが出来る
    //振動の周期を第3引数で指定することが出来る
    //somerSaultCount.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack,2f);
    private void Initialized()
    {
        somerSaultCount.rectTransform.localScale = new Vector3(0.6f, 0.6f, 1f);
        somerSaultCount.color = new Color(1f, 1f, 1f, 0.3f);
    }


}
