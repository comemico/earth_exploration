using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleMode : MonoBehaviour
{
    //タイトル画面の起動演出を制御するスクリプト.
    [Header("タイトル演出完了")]
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


    void Guide(bool isComplete) //ガイドマークのアニメーション起動.
    {
        if (isComplete)
        {
            L_check.Kill(true);
            L_pressPower.Kill(true);
        }
        else
        {
            L_check = checkMark.DOAnchorPosY(distance, easeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetRelative(true); //上下移動.
            L_pressPower = textCanvas.DOFade(0.25f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); //点滅.
        }
    }

    public void PowerOn(bool isComplete) //Powerボタンで呼ばれる.
    {
        powerButton.image.raycastTarget = false; //二度押し防止 & 押下演出無.
        Sequence s_powerOn = DOTween.Sequence();

        //ガイドマーク消失.
        s_powerOn.Append(pressStart.DOAnchorPosY(-150, 0.3f).SetEase(Ease.OutSine));
        s_powerOn.AppendCallback(() => Guide(true));

        //ランプ点灯.
        s_powerOn.Append(lampImg.DOFade(1f, 0.35f).SetEase(Ease.OutSine));
        s_powerOn.Join(modeMg.gryps.WakeUp());

        //BGM再生.
        s_powerOn.AppendInterval(0.25f); //この間にグリプスの起動アニメを完了させる
        s_powerOn.AppendCallback(() => SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title)); //selectSceneから移る場合、自動的に再生できるようにしたい.

        //タイトル消失.
        s_powerOn.Append(backPanel.DOFade(0f, tipsTime).SetEase(tipsType));
        s_powerOn.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(235, 235), tipsTime).SetEase(tipsType));
        s_powerOn.Append(tipsEdge.DOFade(0f, tipsTime).SetEase(tipsType));
        s_powerOn.Join(panel.DOAnchorPosY(-450, moveTime).SetEase(moveType));

        // パネル消失.
        s_powerOn.Append(modeMg.selectMenuMg.ShowSelectButton().SetDelay(0.5f));
        s_powerOn.Join(cinemachineMg.DOLensSize(8f, 1.5f, Ease.Linear));

        s_powerOn.AppendCallback(() =>
        {
            //直近のtweenで最も遅いtweenの後に呼ばれる <= cinemachineMg.DOLensSize() 終了時に起動.
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
