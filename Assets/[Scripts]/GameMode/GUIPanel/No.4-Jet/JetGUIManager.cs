using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JetGUIManager : MonoBehaviour
{
    //*ボタンを出し入れする処理=>GUIMg <-ここ

    [Header("Button")]
    public RectTransform buttonRight;
    public RectTransform buttonLeft;
    public CanvasGroup buttonCanGrp;
    Image pushRight;
    Image pushLeft;
    [Space(10)]

    public bool isHud;

    [Range(0.25f, 1.0f)] public float openTime = 0.25f;
    public Ease openType = Ease.InOutQuad;
    [Range(0.25f, 1.0f)] public float closeTime = 0.25f;
    public Ease closeType = Ease.InOutQuad;
    [Space(10)]

    public Color pushColor;
    [Range(0.01f, 0.5f)] public float pushTime = 0.2f;
    public Ease pushType = Ease.OutQuad;
    [Range(0.01f, 0.5f)] public float pullTime = 0.2f;
    public Ease pullType = Ease.OutQuad;

    List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        pushRight = buttonRight.transform.GetChild(1).GetComponent<Image>();
        pushLeft = buttonLeft.transform.GetChild(1).GetComponent<Image>();
    }

    public void StartUpJetHud()
    {
        buttonCanGrp.blocksRaycasts = true;
        pushRight.color = Color.white;
        pushLeft.color = Color.white;

        tweenList.KillAllAndClear();
        Sequence s_startup = DOTween.Sequence();

        s_startup.Append(buttonRight.DOLocalRotate(Vector3.zero, openTime).SetEase(openType));
        s_startup.Join(buttonLeft.DOLocalRotate(Vector3.zero, openTime).SetEase(openType));
        s_startup.Join(buttonCanGrp.DOFade(1f, openTime).SetEase(openType));
        s_startup.AppendCallback(() => JetButton(false));

        tweenList.Add(s_startup);
        isHud = true;
    }

    public void ShutDownJetHud()
    {
        buttonCanGrp.blocksRaycasts = false;

        tweenList.KillAllAndClear();
        Sequence s_shutdown = DOTween.Sequence();

        s_shutdown.AppendInterval(0.5f);
        s_shutdown.Append(buttonRight.DOLocalRotate(new Vector3(0f, 0f, -90f), closeTime).SetEase(closeType));
        s_shutdown.Join(buttonLeft.DOLocalRotate(new Vector3(0f, 0f, 90f), closeTime).SetEase(closeType));
        s_shutdown.Join(buttonCanGrp.DOFade(0f, closeTime).SetEase(closeType));
        s_shutdown.AppendCallback(() => JetButton(true));

        tweenList.Add(s_shutdown);
        isHud = false;
    }

    public void JetButton(bool isPush)
    {
        //true=>ボタンOnへ 85
        if (isPush)
        {
            pushRight.color = pushColor;
            pushRight.rectTransform.DOKill(true);
            pushRight.rectTransform.DOAnchorPosX(85, pushTime).SetEase(pushType);

            pushLeft.color = pushColor;
            pushLeft.rectTransform.DOKill(true);
            pushLeft.rectTransform.DOAnchorPosX(-85, pullTime).SetEase(pushType);
        }
        //false=>ボタンOffへ 135
        else
        {
            pushRight.color = Color.white;
            pushRight.DOKill(true);
            pushRight.rectTransform.DOAnchorPosX(135, pullTime).SetEase(pullType);

            pushLeft.color = Color.white;
            pushLeft.rectTransform.DOKill(true);
            pushLeft.rectTransform.DOAnchorPosX(-135, pullTime).SetEase(pullType);
        }
    }

    /*
    public void JugeTapMode()
    {
        if (jetNumber == 3 && stageCrl.controlStatus != StageCtrl.ControlStatus.unControl && !isTapMode)
        {
            jetMg.StartUpJetHud();
            isTapMode = true;
            ringCanvas.gameObject.transform.DOScale(1.2f, 0.25f).SetEase(Ease.InQuint);
            ringCanvas.DOFade(1f, 0.25f).SetEase(Ease.InQuint);
        }
    }
    public void ResetJetRing()
    {
        jetMg.ShutDownJetHud();

        jetNumber = 0;
        limitRingImage.fillAmount = 0f;

        isTapMode = false;
        ringCanvas.gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InQuint);
        ringCanvas.DOFade(0f, 0.25f).SetEase(Ease.InQuint);
    }
    */
}
