using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModeManager : MonoBehaviour
{
    //�����S���X�N���v�g.

    public ExitMode exitMode;
    public SettingMode settingMode;
    public CreditMode creditMode;

    public SelectMenuManager selectMenuMg;
    public TitleCurtainManager curtainMg;


    private void Awake()
    {
        exitMode = GetComponentInChildren<ExitMode>();
        settingMode = GetComponentInChildren<SettingMode>();
        creditMode = GetComponentInChildren<CreditMode>();
    }

}
