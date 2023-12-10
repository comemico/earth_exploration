using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleCurtainManager : MonoBehaviour
{
    [Header("BackPanel")]
    public Image backPanel;
    [Range(0.1f, 1f)] public float fadeInDuration = 0.75f;
    public Ease fadeInType = Ease.OutSine;
    [Range(0.1f, 1f)] public float fadeOutDuration = 0.5f;
    public Ease fadeOutType = Ease.OutQuad;

    [Header("Icon")]
    public CanvasGroup icon;
    [Range(0.1f, 0.5f)] public float iconDuration = 0.15f;
    public Ease iconType = Ease.OutQuint;
    public Image iconEmi;
    public Color loadColor;
    public Color normalColor;

    List<Tween> tweenList = new List<Tween>();
    AsyncOperation async;

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        backPanel.color = Color.black;
        icon.alpha = 1f;
        iconEmi.color = normalColor;
    }

    public Sequence OpenCurtain()
    {
        //    .SetUpdate() : true���w�肵���ꍇ�ATimeScale�𖳎����ē��삵�܂�(�f�t�H���g��false)���݁Atrue�ɂ��Ă���
        Sequence seq_fadeIn = DOTween.Sequence();//TimeScale�𖳎����Ă���

        seq_fadeIn.AppendInterval(0.2f);
        seq_fadeIn.Append(backPanel.DOFade(0f, fadeInDuration).SetEase(fadeInType));
        seq_fadeIn.Join(icon.DOFade(0f, iconDuration).SetEase(iconType));

        tweenList.Add(seq_fadeIn);
        return seq_fadeIn;

    }


    public void CloseCurtain(string sceneName)//Scene�ړ�����
    {
        backPanel.raycastTarget = true;
        iconEmi.color = loadColor;

        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Tween emi = iconEmi.DOFade(1f, 0.25f).SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            Tween emi = iconEmi.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash, 2)
            .OnStepComplete(() =>
            {
                CheckLoad();//���[�v��񖈂� (progress >= 0.9f) �����肷��
            }
            );
            tweenList.Add(emi);//���[�v��kill���ƂŃG���[���o���Ȃ��悤�ɂ���
        }
        );

        Sequence seq_close = DOTween.Sequence();
        seq_close.Append(backPanel.DOFade(1f, fadeOutDuration).SetEase(fadeOutType));
        seq_close.Join(icon.DOFade(1f, iconDuration).SetEase(iconType));
        tweenList.Add(seq_close);
    }

    public void CheckLoad()
    {
        if (async.progress >= 0.9f)
        {
            Time.timeScale = 1f;
            async.allowSceneActivation = true;
        }
    }

    public Sequence EndGameCurtain()
    {
        Sequence seq_end = DOTween.Sequence();
        seq_end.Append(backPanel.DOFade(1f, fadeOutDuration).SetEase(fadeOutType));
        tweenList.Add(seq_end);
        return seq_end;
    }

}
