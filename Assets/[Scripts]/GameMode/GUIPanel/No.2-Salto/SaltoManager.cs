using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SaltoManager : MonoBehaviour
{
    //���Salto�̐���Ɋւ��X�N���v�g.
    //: SaltoStart() => OnButton() => SaltoComplete() => SaltoEnd().

    [Header("LensSize")]
    [Range(10f, 15f)] public float lensOutValue = 13;
    [Range(0.1f, 0.5f)] public float lensOutTime = 0.3f;
    public Ease lensOutType = Ease.OutSine;


    [Header("TimeScale")]
    [NamedArrayAttribute(new string[] { "������", "1��]", "2��]", "3��]" })]
    [Range(0.03f, 1f)] public float[] timeScaleBox;

    [Range(0.1f, 0.5f)] public float slowTime = 0.1f;
    public Ease slowType = Ease.OutQuint;
    [Range(0.1f, 0.5f)] public float returnTime = 0.25f;
    public Ease returnType = Ease.InQuint;


    [Header("SaltoInfo")]
    public int saltoNum;
    public bool isSalto; //Salto�����ǂ���

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
            saltoHudMg.StartUpSaltoHud(flightDuration);
            saltoHudMg.StartTimeGauge(flightDuration);
            stageCrl.grypsCrl.effector.animatorSalto.SetBool("isWing", true);

            cinemachineCrl.DOLensSize(lensOutValue, lensOutTime, lensOutType);
            cinemachineCrl.DOTimeScale(timeScaleBox[0], slowTime, slowType);
        }
    }

    public void SaltoEnd()
    {
        if (saltoHudMg.isHud)
        {
            saltoHudMg.ShutDownSaltoHud();
            saltoNum = 0;
            isSalto = false;

            stageCrl.grypsCrl.effector.animatorSalto.SetBool("isWing", false);

            if (!stageCrl.jetMg.jetHudMg.isHud) //stock��1�ȏ� && Hud��false�̏ꍇ�̂݋N��������
            {
                stageCrl.jetMg.jetHudMg.StartUpJetHud();
            }


            cinemachineCrl.DOLensSize(10, 1f, Ease.Linear);
            cinemachineCrl.DOTimeScale(1, returnTime, returnType);
        }
    }

    public void OnButtonDown()
    {
        saltoHudMg.SaltoButton(true);
    }

    public void OnButtonUp()
    {
        saltoHudMg.SaltoButton(false);

        if (isSalto || !saltoHudMg.isHud) return; //Salto�����V���b�g�_�E����͓��͂��󂯕t���Ȃ�.

        stageCrl.grypsCrl.effector.Salto(saltoNum);
        saltoHudMg.RotateDisc();

        saltoNum++;
        saltoHudMg.DisplayStock(saltoNum);
        cinemachineCrl.DOTimeScale(timeScaleBox[saltoNum], slowTime, slowType);

        isSalto = true;
    }

    public void SaltoComplete()
    {
        stageCrl.controlScreenMg.ProduceMemory(1);
        stageCrl.jetMg.DisplayJetStock(stageCrl.jetMg.stockNum + 1); //�󒆎�(unControl)��stock�������Ă�JetHud�̋N���͂��Ȃ�.

        if (saltoNum >= 3)
        {
            SaltoEnd(); //��]�������SaltoHud������Shutdown.
        }

        isSalto = false;
    }

}
