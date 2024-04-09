using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleMode : MonoBehaviour
{
    //�^�C�g����ʂ̋N�����o�𐧌䂷��X�N���v�g.
    [Header("�^�C�g�����o����")]
    public bool isCompleteTitle;

    [Header("Title")]
    public CanvasGroup backPanel;
    public Image tipsEdge;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float tipsTime;
    public Ease tipsType;


    [Header("Panel")]
    public Button powerButton;
    public RectTransform panel;
    Image lampImg;
    [Space(10)]

    [Range(0.15f, 0.5f)] public float moveTime;
    public Ease moveType;


    [Header("Guide")]
    public RectTransform pressStart;
    public RectTransform checkMark;
    public CanvasGroup textCanvas;
    Tween L_pressPower;
    Tween L_check;
    [Space(10)]

    public float distance;
    public float easeDuration;
    public Ease easeType;


    CinemachineManager cinemachineMg;
    ModeManager modeMg;

    List<Tween> tweenList = new List<Tween>();
    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        GetComponent();
        Guide(false);
        if (isCompleteTitle) PowerOn(true);
    }

    void GetComponent()
    {
        powerButton.onClick.AddListener(() => PowerOn(false));
        lampImg = powerButton.transform.GetChild(0).GetComponent<Image>();
        cinemachineMg = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
        modeMg = GetComponentInParent<ModeManager>();
    }


    void Guide(bool isComplete) //�K�C�h�}�[�N�̃A�j���[�V�����N��.
    {
        if (isComplete)
        {
            L_check.Kill(true);
            L_pressPower.Kill(true);
        }
        else
        {
            L_check = checkMark.DOAnchorPosY(distance, easeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetRelative(true); //�㉺�ړ�.
            L_pressPower = textCanvas.DOFade(0.25f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); //�_��.
        }
    }

    public void PowerOn(bool isComplete) //Power�{�^���ŌĂ΂��.
    {
        powerButton.image.raycastTarget = false; //��x�����h�~ & �������o��.
        Sequence s_powerOn = DOTween.Sequence();

        //�K�C�h�}�[�N����.
        s_powerOn.Append(pressStart.DOAnchorPosY(-150, 0.3f).SetEase(Ease.OutSine));
        s_powerOn.AppendCallback(() => Guide(true));

        //�����v�_��.
        s_powerOn.Append(lampImg.DOFade(1f, 0.35f).SetEase(Ease.OutSine));
        s_powerOn.Join(modeMg.gryps.WakeUp());

        //BGM�Đ�.
        s_powerOn.AppendInterval(0.25f); //���̊ԂɃO���v�X�̋N���A�j��������������
        s_powerOn.AppendCallback(() => SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title)); //selectScene����ڂ�ꍇ�A�����I�ɍĐ��ł���悤�ɂ�����.

        //�^�C�g������.
        s_powerOn.Append(backPanel.DOFade(0f, tipsTime).SetEase(tipsType));
        s_powerOn.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(235, 235), tipsTime).SetEase(tipsType));
        s_powerOn.Append(tipsEdge.DOFade(0f, tipsTime).SetEase(tipsType));
        s_powerOn.Join(panel.DOAnchorPosY(-450, moveTime).SetEase(moveType));

        // �p�l������.
        s_powerOn.Append(modeMg.selectMenuMg.ShowSelectButton().SetDelay(0.5f));
        s_powerOn.Join(cinemachineMg.DOLensSize(8f, 1.5f, Ease.Linear));

        s_powerOn.AppendCallback(() =>
        {
            //���߂�tween�ōł��x��tween�̌�ɌĂ΂�� <= cinemachineMg.DOLensSize() �I�����ɋN��.
            modeMg.selectMenuMg.launchButton.enabled = true;
        });

        //tweenList.Add(s_powerOn);

        if (isComplete)
        {
            modeMg.selectMenuMg.launchButton.enabled = true;
            L_check.Kill(true);
            L_pressPower.Kill(true);
            s_powerOn.Kill(true);
        }
    }

}
