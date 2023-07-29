using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    [Header("ポーズボタン")]
    public Button push_Pause;
    public RectTransform dial;
    public Image bloom;

    const Ease TYPE = Ease.OutQuint;
    const int ANGLE = -30;
    const float DURATION = 0.2f;

    [Header("ポップアップ")]
    public Button push_WorldMap;
    public Button push_Retry;

    PopController popCtrl;
    Text pauseText;
    public bool isPause = false;



    private void Awake()
    {
        InitializeGetComponent();
        AddListener();
    }

    private void InitializeGetComponent()
    {
        popCtrl = GetComponent<PopController>();
        pauseText = transform.GetChild(2).GetComponent<Text>();
        pauseText.enabled = false;
        //push_Pause.interactable = false;
        //bloomImage.enabled = false;
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

    public void OpenPausePanel()
    {
        isPause = true;

        Time.timeScale = 0f;
        pauseText.enabled = true;
        bloom.enabled = true;

        dial.DOLocalRotate(new Vector3(0f, 0f, ANGLE), DURATION).SetEase(TYPE);

        popCtrl.OpenPanel();
    }

    public void ClosePausePanel(bool isComplete)
    {
        isPause = false;

        Time.timeScale = 1f;
        pauseText.enabled = false;
        bloom.enabled = false;

        dial.DOLocalRotate(Vector3.zero, DURATION).SetEase(TYPE);

        popCtrl.ClosePanel(isComplete);
    }

    //if (stageCrl.controlStatus == StageCtrl.ControlStatus.unControl) push_Pause.interactable = false;//空中時もポーズを押せなくなるが、変に連打されるより着地後に戻すほうが都合が良いかもしれない
}
