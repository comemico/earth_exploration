using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GuideManager : MonoBehaviour
{
    //ミッションの表示/非表示
    public PopController pop;
    public Button nextButton;
    public Button closeButton;

    public Text content;
    public Text pageText;

    public List<GuidData> guideOrder;

    public int guideNum;
    public int maxPage;
    public int currentPage;

    StageCtrl stageCrl;
    CinemachineManager cinemachineCrl;
    float savedTimeScale;


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
        pop.OpenPanel();

        savedTimeScale = Time.timeScale;
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
        stageCrl.saltoMg.saltoHudMg.gauge.DOPlay();
        pop.ClosePanel(false);

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
    //NextText() :あるガイドのテキストボックスの要素数だけ更新する.

    //CloseGuide() :ガイドを非表示にする.
    /*
     * 時間を動かす.
     * ポップを閉じる.
     */


}


[System.Serializable]
public class GuidData
{
    [Multiline(5)] public List<string> guidePage;
}
