using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GuideManager : MonoBehaviour
{
    //�~�b�V�����̕\��/��\��
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

    //OpenGuide() :�K�C�h��\������.<= �K�C�h�̉��Ԃ�\�����邩 + ���̃K�C�h�̃e�L�X�g�{�b�N�X�̗v�f���Ń|�b�v��؂�ւ���.
    /*
     * ���Ԃ�ۑ����A�~�߂�.
     * �|�b�v���J��.
     * �K�C�h�̎�ނ��w�肷��.
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

        //�X���[��ԂŃ|�[�Y��ʂɓ����Ă��A�X���[��ԂŖ߂���悤�ɂ��邽��.
        if (Mathf.Approximately(Time.timeScale, 1f)) //timescale= 1f�ɋ߂����1f�ɐݒ�.
        {
            cinemachineCrl.DOTimeScale(1f, 0f, Ease.Linear);
        }
        else
        {
            cinemachineCrl.DOTimeScale(savedTimeScale, 0f, Ease.Linear);
        }
    }
    //NextText() :����K�C�h�̃e�L�X�g�{�b�N�X�̗v�f�������X�V����.

    //CloseGuide() :�K�C�h���\���ɂ���.
    /*
     * ���Ԃ𓮂���.
     * �|�b�v�����.
     */


}


[System.Serializable]
public class GuidData
{
    [Multiline(5)] public List<string> guidePage;
}
