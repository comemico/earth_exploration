using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("ボタン設定")]
    [Header("-----------------------------")]
    public Button push_Pause;
    public Button push_Retry;
    public Button push_WorldMap;

    [Space(PreHeader)]
    [Header("ポーズボタン")]
    [Header("-----------------------------")]
    public RectTransform rect_Base;
    public Ease easeType_TimeScale;
    public float duration_TimeScale;


    [Space(PreHeader)]
    [Header("確認")]
    [Header("-----------------------------")]
    public bool isPause;

    public Text pauseText;
    public Image bloomImage;
    StageCtrl stageCrl;
    PopController popCtrl;
    void Start()
    {
        InitializeGetComponent();
        AddListener();
    }

    private void InitializeGetComponent()
    {
        popCtrl = GetComponent<PopController>();
        stageCrl = GetComponentInParent<StageCtrl>();
        isPause = false;
        pauseText.enabled = false;
        bloomImage.enabled = false;
        push_Pause.interactable = false;
        /*
        pauseText = transform.GetChild(2).GetComponent<Text>();
        bloomImage = transform.GetChild(3).GetChild(1).GetComponent<Image>();
         */
        //ClosePausePanel(true);
    }

    private void AddListener()
    {
        push_Pause.onClick.AddListener(PushPauseButton);
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
    Tween t;
    public void OpenPausePanel()
    {
        t.Kill(true);
        isPause = true;
        pauseText.enabled = true;
        bloomImage.enabled = true;
        rect_Base.DOLocalRotate(new Vector3(0f, 0f, -30f), 0.2f).SetEase(easeType_TimeScale);
        t = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 0.125f).SetEase(easeType_TimeScale).OnComplete(() => push_Pause.interactable = true);
        popCtrl.OpenPanel();
    }

    public void ClosePausePanel(bool isComplete)
    {
        t.Kill(true);
        if (stageCrl.controlStatus == StageCtrl.ControlStatus.unControl) push_Pause.interactable = false;//空中時もポーズを押せなくなるが、変に連打されるより着地後に戻すほうが都合が良いかもしれない
        pauseText.enabled = false;
        bloomImage.enabled = false;
        rect_Base.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.15f).SetEase(easeType_TimeScale);
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.125f).SetEase(easeType_TimeScale);

        if (isComplete)
        {
            popCtrl.ClosePanel(true);
        }
        else
        {
            popCtrl.ClosePanel(false);
        }
    }

}
