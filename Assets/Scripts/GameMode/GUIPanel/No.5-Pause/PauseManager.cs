using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    [Header("ポーズボタン")]
    public Button push_Pause;
    public RectTransform dial;
    public Text pauseText;

    [Header("ポップアップ")]
    public Button push_Retry;
    public Button push_World;
    public Button push_Close;
    public Image[] emissionImg;
    public bool isPause = false;

    PopController popCtrl;
    StageCtrl stageCrl;
    float savedTimeScale;


    private void Awake()
    {
        InitializeGetComponent();
        AddListener();
    }

    private void InitializeGetComponent()
    {
        popCtrl = GetComponent<PopController>();
        stageCrl = GetComponentInParent<StageCtrl>();
        //pauseText.enabled = false;
        //push_Pause.interactable = false;
        //bloomImage.enabled = false;
        //ClosePausePanel(true);
    }

    private void AddListener()
    {
        push_Pause.onClick.AddListener(PushPauseButton);
        push_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        push_World.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
        push_Close.onClick.AddListener(PushPauseButton);
    }

    public void PushPauseButton()
    {
        if (!isPause)
        {
            OpenPausePanel();
        }
        else
        {
            ClosePausePanel(false);
        }
    }

    public void OpenPausePanel()
    {
        isPause = true;
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        stageCrl.saltoMg.saltoTimeGage.DOPause();
        pauseText.enabled = true;
        SwichBloom(true, 0.2f);
        //dial.DOLocalRotate(new Vector3(0f, 0f, ANGLE), DURATION).SetEase(TYPE);

        popCtrl.OpenPanel();
    }

    public void ClosePausePanel(bool isComplete)
    {
        isPause = false;
        stageCrl.saltoMg.saltoTimeGage.DOPlay();

        if (Mathf.Approximately(Time.timeScale, 1f))//timescale= 1fに近ければ1fに設定
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = savedTimeScale;
        }

        pauseText.enabled = false;
        SwichBloom(false, 0.2f);
        //dial.DOLocalRotate(Vector3.zero, DURATION).SetEase(TYPE);

        popCtrl.ClosePanel(isComplete);
    }

    public void SwichBloom(bool isEnabled, float fadeTime) //PanelAnimeで光源が見えるのを防ぐ目的
    {
        foreach (Image img in emissionImg)
        {
            img.enabled = isEnabled;
            img.DOKill(true);
            img.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(Ease.InQuint);
        }
    }

    //if (stageCrl.controlStatus == StageCtrl.ControlStatus.unControl) push_Pause.interactable = false;//空中時もポーズを押せなくなるが、変に連打されるより着地後に戻すほうが都合が良いかもしれない
}
