using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SaltoManager : MonoBehaviour
{

    [Header("SaltoPanel")]
    public int saltoNum;
    public bool isSalto; //Salto�����ǂ���

    const int MAX_STOCK = 3;


    [Header("TimeScale")]
    [NamedArrayAttribute(new string[] { "������", "1��]", "2��]", "3��]" })]
    [Range(0.03f, 1f)] public float[] timeScaleBox;

    [Range(0.1f, 0.5f)] public float slowTime = 0.1f;
    public Ease slowType = Ease.OutQuint;
    [Range(0.1f, 0.5f)] public float returnTime = 0.25f;
    public Ease returnType = Ease.InQuint;


    [HideInInspector] public SaltoHudManager saltoHudMg;
    StageCtrl stageCrl;
    CinemachineManager cinemachineCrl;


    private void Awake()
    {
        saltoHudMg = GetComponent<SaltoHudManager>();
        stageCrl = transform.root.GetComponent<StageCtrl>();
        cinemachineCrl = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
    }

    public void SaltoStart(float flightDuration) //Salto�G���A�ɓ���N��
    {
        if (!saltoHudMg.isHud)
        {
            saltoHudMg.StartUpSaltoHud();
            saltoHudMg.StartTimeGauge(flightDuration);
            stageCrl.grypsCrl.effector.animatorSalto.SetBool("isWing", true);

            cinemachineCrl.DOTimeScale(timeScaleBox[0], slowTime);
        }
    }

    public void SaltoEnd()
    {
        if (saltoHudMg.isHud)
        {
            saltoHudMg.ShutDownSaltoHud();
            saltoNum = 0;
            stageCrl.grypsCrl.effector.animatorSalto.SetBool("isWing", false);

            cinemachineCrl.DOTimeScale(1, returnTime);
            //cinemachineCrl.DefaultZoom();
        }
    }

    public void OnButtonDown()
    {
        saltoHudMg.SaltoButton(true);
    }

    public void OnButtonUp()
    {
        saltoHudMg.SaltoButton(false);

        if (isSalto) return; //Salto���̓X���[������

        stageCrl.grypsCrl.effector.Salto();
        saltoHudMg.RotateDisc();
        cinemachineCrl.DOTimeScale(timeScaleBox[saltoNum], slowTime);

        isSalto = true;
    }

    public void SaltoComplete()
    {
        stageCrl.controlScreenMg.ProduceMemory(1);
        stageCrl.jetMg.DisplayJetStock(stageCrl.jetMg.stockNum + 1); //�󒆎�(unControl)��stock�������Ă�JetHud�̋N���͂��Ȃ�

        saltoNum++;
        if (saltoNum >= 3)
        {
            SaltoEnd(); //��]�������SaltoHud������Shutdown
            //stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
            //return;
        }

        isSalto = false;
    }

}
