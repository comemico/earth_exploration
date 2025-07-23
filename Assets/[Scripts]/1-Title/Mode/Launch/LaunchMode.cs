using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class LaunchMode : MonoBehaviour
{
    DataManager dataMg;

    CanvasGroup canvasGrp;
    ModeManager modeMg;
    CinemachineManager cinemachineMg;

    private void Awake()
    {
        dataMg = GetComponentInParent<DataManager>();
        cinemachineMg = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();

        modeMg = transform.parent.GetComponent<ModeManager>();
        canvasGrp = GetComponent<CanvasGroup>();
        canvasGrp.alpha = 0f;
        canvasGrp.blocksRaycasts = false;
    }

    public void StartUpLaunchMode()
    {
        Sequence s_Launch = DOTween.Sequence();

        cinemachineMg.cinemachineVirtualCamera.Follow = null;
        //セレクトボタン消失.
        s_Launch.AppendCallback(() => modeMg.gryps.animatorJet.SetBool("isDown", true));
        s_Launch.Append(modeMg.selectMenuMg.HideSelectButton(SelectMenuManager.BAND.normal, SelectMenuManager.COLOR.launch));

        //ジェットの前にパネルを降ろす.
        s_Launch.AppendInterval(0.25f);
        s_Launch.Append(modeMg.selectMenuMg.HideSelectButton(SelectMenuManager.BAND.down, SelectMenuManager.COLOR.launch));
        s_Launch.AppendCallback(() => modeMg.gryps.ForceJet(0));
        s_Launch.AppendInterval(1f);
        s_Launch.AppendCallback(() =>
        {
            /*
            modeMg.curtainMg.CloseCurtain("StageSelect");
             */
            if (dataMg.data.isGuide)
            {
                modeMg.curtainMg.CloseCurtain("StageSelect");
            }
            else
            {
                dataMg.data.isGuide = true;
                modeMg.curtainMg.CloseCurtain("Area[0]Linear[0]");
                //stageMap : recentStageをArea[1]Linear[0]にする.
            }
        }
        );
    }




}
