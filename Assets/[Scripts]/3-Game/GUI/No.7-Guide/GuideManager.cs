using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GuideManager : MonoBehaviour
{
    //ミッションの表示/非表示
    [Header("ガイドパネル")]
    public PopController pop;
    public Button nextButton;
    public Button closeButton;
    public Text content;
    public Text pageText;
    [Space(10)]

    public List<GuidData> guideOrder;
    public int guideNum;
    public int maxPage;
    public int currentPage;
    float savedTimeScale;
    StageCtrl stageCrl;
    CinemachineManager cinemachineCrl;


    [Header("ガイド矢印")]
    public RectTransform[] guideArrow;
    [Space(10)]

    public GUIDE guide;
    public enum GUIDE
    {
        boost = 0,
        jet = 1,
        salto = 2
    }
    [Range(0, 10)] public int arrowNum;
    public float arrowDistance;
    [Range(0f, 1f)] public float arrowTime;
    public Ease arrowType;
    public bool isPlay;
    Tween L_arrow;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => DisplayText());
        closeButton.onClick.AddListener(() => CloseGuide());
        stageCrl = GetComponentInParent<StageCtrl>();
        cinemachineCrl = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
    }

    //OpenGuide() :ガイドを表示する.<= ガイドの何番を表示するか + そのガイドのテキストボックスの要素数でポップを切り替える.
    /*
     * 時間を保存し、止める.
     * ポップを開く.
     * ガイドの種類を指定する.
     */
    public void OpenGuide(int guideNum)
    {
        this.guideNum = guideNum;
        this.maxPage = guideOrder[guideNum].guidePage.Count;
        this.currentPage = 0;
        pageText.text = (this.currentPage + 1) + " / " + this.maxPage;

        if (currentPage == maxPage - 1)
        {
            closeButton.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            closeButton.transform.parent.gameObject.SetActive(false);
        }

        content.text = guideOrder[guideNum].guidePage[currentPage];

        stageCrl.saltoMg.saltoHudMg.gauge.DOPause();
        stageCrl.jetMg.isCoolDown = true;
        pop.OpenPanel();

        //時間を止める.
        savedTimeScale = Time.timeScale;
        cinemachineCrl.DOPauseTimeScale();
        cinemachineCrl.DOTimeScale(0f, 0f, Ease.Linear);
    }

    public void DisplayText()
    {
        if (currentPage < maxPage - 1)
        {
            this.currentPage++;
            pageText.text = (this.currentPage + 1) + " / " + this.maxPage;

            content.text = guideOrder[this.guideNum].guidePage[currentPage];

            if (currentPage == maxPage - 1)
            {
                closeButton.transform.parent.gameObject.SetActive(true);
            }
        }

    }

    public void CloseGuide()
    {
        this.currentPage = 0;
        stageCrl.jetMg.isCoolDown = false;
        stageCrl.jetMg.OnButtonUp();
        stageCrl.saltoMg.saltoHudMg.gauge.DOPlay();
        //cinemachineCrl.DOPlayTimeScale();

        pop.ClosePanel(false);

        //ガイド番号の照合でで矢印ガイドを開始&停止を行なう.
        if (this.guideNum == arrowNum)// && !isPlay).
        {
            GuideArrow(true);
        }
        else if (isPlay)
        {
            GuideArrow(false);
        }

        //スロー状態でポーズ画面に入っても、スロー状態で戻せるようにするため.
        if (Mathf.Approximately(Time.timeScale, 1f)) //timescale= 1fに近ければ1fに設定.
        {
            cinemachineCrl.DOTimeScale(1f, 0f, Ease.Linear);
        }
        else
        {
            cinemachineCrl.DOTimeScale(savedTimeScale, 0f, Ease.Linear);
        }
    }

    public void GuideArrow(bool isShow)
    {
        if (isShow)
        {
            L_arrow.Kill(true);
            guideArrow[(int)guide].anchoredPosition = Vector2.zero;
            guideArrow[(int)guide].GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.Linear);
            if ((int)guide != 1)//ガイドが「ジェット」じゃない場合はX方向にアニメーション.
            {
                L_arrow = guideArrow[(int)guide].DOAnchorPosX(arrowDistance, arrowTime).SetEase(arrowType).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
            }
            else
            {
                L_arrow = guideArrow[(int)guide].DOAnchorPosY(arrowDistance, arrowTime).SetEase(arrowType).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
            }
            isPlay = true;
        }
        else
        {
            guideArrow[(int)guide].GetComponent<CanvasGroup>().DOFade(0f, 0.3f).SetEase(Ease.Linear);
            L_arrow.Kill(true);
            isPlay = false;
        }
    }

}


[System.Serializable]
public class GuidData
{
    [Multiline(5)] public List<string> guidePage;
}
