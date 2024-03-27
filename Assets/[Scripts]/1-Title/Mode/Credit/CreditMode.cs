using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditMode : MonoBehaviour
{
    //�N���W�b�g��ʂ̕\��/��\���𐧌䂷��X�N���v�g.

    [Header("View")]
    public CanvasGroup canvasGrp;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float fadeTime = 0.25f;
    public Ease fadeType;


    [Header("Button")]
    public RectTransform closeRect;
    Button closeButton;
    [Space(10)]

    public float buttonShowValue = 280;
    public float buttonHideValue = -150;
    [Range(0.1f, 0.5f)] public float buttonTime;
    public Ease buttonType;

    ModeManager modeMg;

    private void Awake()
    {
        modeMg = transform.parent.GetComponent<ModeManager>();

        closeButton = closeRect.transform.GetChild(1).GetComponent<Button>();
        closeButton.onClick.AddListener(ShutDownCreditMode);

        canvasGrp.alpha = 0f;
        canvasGrp.blocksRaycasts = false;
    }

    public void StartUpCreditMode()
    {
        canvasGrp.blocksRaycasts = true;
        canvasGrp.gameObject.SetActive(true);

        Sequence s_startupSetting = DOTween.Sequence();

        //�Z���N�g�{�^������.
        s_startupSetting.Append(modeMg.selectMenuMg.HideSelectButton(SelectMenuManager.BAND.up, SelectMenuManager.COLOR.credit));

        //�ݒ��ʏo��.
        s_startupSetting.Append(canvasGrp.DOFade(1f, fadeTime).SetEase(fadeType));
        s_startupSetting.Join(closeRect.DOAnchorPosX(buttonShowValue, buttonTime).SetEase(buttonType));
    }

    public void ShutDownCreditMode()
    {
        canvasGrp.blocksRaycasts = false;

        Sequence s_shutDownSetting = DOTween.Sequence();

        //�ݒ��ʏ���.
        s_shutDownSetting.Append(closeRect.DOAnchorPosX(buttonHideValue, buttonTime).SetEase(buttonType));
        s_shutDownSetting.Join(canvasGrp.DOFade(0f, fadeTime).SetEase(fadeType));

        //�Z���N�g�{�^���o��.
        s_shutDownSetting.Append(modeMg.selectMenuMg.ShowSelectButton());
        s_shutDownSetting.AppendCallback(() => canvasGrp.gameObject.SetActive(false));
    }

}
