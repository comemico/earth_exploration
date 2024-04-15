using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class CurtainManagerSelect : MonoBehaviour
{
    [Range(0.1f, 1f)] public float fadeInTime;
    [Range(0.1f, 1f)] public float fadeOutTime_Start;
    [Range(0.1f, 1f)] public float fadeOutTime_Home;

    [Range(0.1f, 0.5f)] public float iconDuration = 0.15f;

    public Image backPanel;
    public CanvasGroup icon;
    public Image iconEmi;

    public Color loadColor;
    public Color normalColor;

    AsyncOperation async;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
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
        seq_fadeIn.Append(backPanel.DOFade(0f, fadeInTime).SetEase(Ease.OutSine));
        seq_fadeIn.Join(icon.DOFade(0f, iconDuration).SetEase(Ease.OutQuint));

        return seq_fadeIn;

    }


    public void CloseCurtain(string sceneName, float fadeOutTime)//Scene�ړ�����
    {
        backPanel.raycastTarget = true;
        iconEmi.color = loadColor;

        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Tween emi = iconEmi.DOFade(1f, 0.2f).SetEase(Ease.InOutQuad) //����̓_���ݒ�.
        .OnComplete(() =>
        {
            Tween emi = iconEmi.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash, 2)
            .OnStepComplete(() =>
            {
                CheckLoad();//���[�v��񖈂� (progress >= 0.9f) �����肷��
            }
            );
            //tweenList.Add(emi);//���[�v��kill���ƂŃG���[���o���Ȃ��悤�ɂ���
        }
        );

        Sequence seq_close = DOTween.Sequence();
        seq_close.Append(backPanel.DOFade(1f, fadeOutTime).SetEase(Ease.OutSine));
        seq_close.Join(icon.DOFade(1f, iconDuration).SetEase(Ease.OutQuint));
    }

    public void CheckLoad()
    {
        if (async.progress >= 0.9f)
        {
            Time.timeScale = 1f;
            async.allowSceneActivation = true;
            iconEmi.DOKill(false);
        }
    }


}
