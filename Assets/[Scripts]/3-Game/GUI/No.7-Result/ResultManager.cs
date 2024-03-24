using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("CLEAR")]
    public CanvasGroup clearPanel;
    public Text clearText;
    public RectTransform pauseButton;
    [Space(10)]

    [Range(0.1f, 1f)] public float appDuration;
    public Ease appType;
    [Range(0.1f, 1f)] public float slideDuration;
    public Ease slideType;


    [Header("MISS")]
    public CAUSE cause;
    public CanvasGroup missPanel;
    public Text missText;
    public GameObject[] missIcon;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float fadeDuration;
    public Ease fadeType;
    [Range(0.1f, 0.5f)] public float fallDuration;
    public Ease fallType;
    [Range(0.1f, 1.0f)] public float tumbleDuration;
    public Ease tumbleType;
    public enum CAUSE
    {
        [InspectorName("�������Ă��܂���!")] �������Ă��܂��� = 0,
        [InspectorName("�������Ă��܂���!")] �������Ă��܂��� = 1,
        [InspectorName("�o�b�e���[�؂�!?")] �o�b�e���[�؂� = 2
    }


    [Header("Button")]
    public PATTERN pattern;
    public RectTransform button_Right;
    public RectTransform button_Left;

    public Button r_Retry;
    public Button r_World;
    public Button r_Next;

    public Button l_Retry;
    public Button l_World;

    public Image[] emissionImg;
    public enum PATTERN
    {
        onlyMap = 0,
        normal = 1,
        onlyNext = 2
    }


    StageCtrl stageCrl;
    TipsManager tipsMg;

    /*
    [Header("Tips")]
    public Image tipsEdge;
    public CanvasGroup backPanel;
    public Text tipsText;
    public Image rankLamp;
    public float perTextWide;
    public int characterLimit = 10;
    [Range(0.1f, 0.5f)] public float scrollSpeed;

    [Range(0.1f, 0.5f)] public float tipsOpenDuration;
    public Ease tipsOpenType;
    [Range(0.1f, 0.5f)] public float tipsFadeDuration;
    public Ease tipsFadeType;
     */

    [Range(0.1f, 0.5f)] public float lampDuration;
    public Ease lampType;

    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        stageCrl = GetComponentInParent<StageCtrl>();
        tipsMg = GetComponentInChildren<TipsManager>();

        AddListener();
        tipsMg.rankLamp.color = stageCrl.rankColor[stageCrl.stageRank - 1];
    }

    private void AddListener()
    {
        r_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        r_World.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
        r_Next.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("Area[" + stageCrl.areaNum + "]" + stageCrl.stageType + "[" + (stageCrl.stageNum + 1) + "]"));
    }

    public void OpenClearPanel()
    {
        ButtonPattern(pattern);

        Sequence seq_clear = DOTween.Sequence();
        seq_clear.Append(clearText.rectTransform.DOAnchorPosX(0f, slideDuration).SetEase(slideType, 3));
        seq_clear.Join(clearPanel.DOFade(1f, appDuration).SetEase(appType));
        seq_clear.Join(pauseButton.DOAnchorPosY(160f, 0.35f));
        seq_clear.AppendCallback(() => stageCrl.JudgeStageData());
        seq_clear.AppendInterval(0.75f);
        seq_clear.Append(button_Right.DOAnchorPosX(0f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));

        tweenList.Add(seq_clear);
    }


    public void OpenMissPanel(CAUSE setCause)
    {
        ButtonPattern(PATTERN.normal);

        cause = setCause;
        missText.text = Enum.GetName(typeof(CAUSE), cause) + "!?";
        for (int i = 0; i < missIcon.Length; i++) missIcon[i].SetActive(false);
        missIcon[(int)cause].SetActive(true);


        Sequence seq_miss = DOTween.Sequence();
        seq_miss.Append(missPanel.DOFade(1f, fadeDuration).SetEase(fadeType));
        seq_miss.Append(missText.rectTransform.DOAnchorPosY(-225f, fallDuration).SetEase(fallType));
        seq_miss.Append(button_Right.DOAnchorPosX(0f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));
        seq_miss.AppendInterval(0.15f);
        seq_miss.Append(missText.transform.DOLocalRotate(new Vector3(0f, 0f, -7.5f), tumbleDuration).SetEase(tumbleType));
        // seq_miss.Join(OpenTips());
        seq_miss.AppendCallback(() =>
        {
            tipsMg.ShowTips();
            //ScrollText();
        });

        tweenList.Add(seq_miss);
    }


    public void ButtonPattern(PATTERN setPattern)
    {
        switch (setPattern)
        {
            //�ʏ�X�e�[�W�N���A��.
            case PATTERN.onlyMap:
                r_World.transform.parent.gameObject.SetActive(true);
                r_World.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-160f, -60f);

                r_Retry.transform.parent.gameObject.SetActive(false);
                r_Next.transform.parent.gameObject.SetActive(false);
                break;

            //�X�e�[�W���s��.
            case PATTERN.normal:
                r_Retry.transform.parent.gameObject.SetActive(true);
                r_World.transform.parent.gameObject.SetActive(true);

                r_Next.transform.parent.gameObject.SetActive(false);
                break;

            //�K�C�h�X�e�[�W�N���A��.
            case PATTERN.onlyNext:
                r_Next.transform.parent.gameObject.SetActive(true);
                r_Next.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-160f, -60f);

                r_Retry.transform.parent.gameObject.SetActive(false);
                r_World.transform.parent.gameObject.SetActive(false);
                break;
        }
    }


    public void SwichBloom(bool isEnabled, float fadeTime) //FadeIn����ꍇ�APanelAnime�Ō�����������̂�h���ړI
    {
        foreach (Image img in emissionImg)
        {
            img.enabled = isEnabled;
            img.DOKill(true);
            img.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(lampType);
        }
    }




    /*
    public Sequence OpenTips()
    {
        backPanel.alpha = 0f;
        tipsEdge.color = new Color(1, 1, 1, 0);
        tipsEdge.rectTransform.sizeDelta = new Vector2(140.8f, 140.8f);

        Sequence sq_tips = DOTween.Sequence();
        sq_tips.AppendInterval(0.1f);
        sq_tips.Append(tipsEdge.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));
        sq_tips.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(875f, 140.8f), tipsOpenDuration).SetEase(tipsOpenType));
        sq_tips.Append(backPanel.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));

        tweenList.Add(sq_tips);
        return sq_tips;
    }

    public void ScrollText()
    {
        tipsText.rectTransform.anchoredPosition = new Vector2(120, 0f);
        Sequence sq_scroll = DOTween.Sequence();
        sq_scroll.Append(tipsText.DOFade(1f, 0.5f));
        sq_scroll.Join(tipsText.rectTransform.DOAnchorPosX(80, 0.5f).SetEase(Ease.Linear));
        if (tipsText.text.Length <= characterLimit) return;

        sq_scroll.AppendInterval(2.5f);
        //�E�e�L�X�g������7.5����i�ށA6������FadeOut �Esequence.Append().SetLoop(-1) �|�C���g��SetLoops()��sequence�ɂ����邱��
        sq_scroll.Append(tipsText.rectTransform.DOAnchorPosX(-1 * tipsText.rectTransform.sizeDelta.x * 0.75f, (tipsText.text.Length * scrollSpeed) * 0.75f).SetRelative(true).SetEase(Ease.Linear));
        sq_scroll.Join(tipsText.DOFade(0f, 0.75f).SetDelay((tipsText.text.Length * scrollSpeed) * 0.6f).SetEase(Ease.Linear));//�e�L�X�g�����̔����𒴂��������肩��N��
        sq_scroll.Append(tipsText.rectTransform.DOAnchorPosX(120, 0)).SetLoops(-1);
        sq_scroll.AppendInterval(0.25f);
        tweenList.Add(sq_scroll);
    }
     */


}
