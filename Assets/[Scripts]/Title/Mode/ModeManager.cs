using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModeManager : MonoBehaviour
{
    //仲介を担うスクリプト.

    public SelectMenuManager selectMenuMg;
    public TitleCurtainManager curtainMg;
    public TitleGryps gryps;

    [HideInInspector] public ExitMode exitMode;
    [HideInInspector] public SettingMode settingMode;
    [HideInInspector] public CreditMode creditMode;
    [HideInInspector] public LaunchMode launchMode;

    private void Awake()
    {
        exitMode = GetComponentInChildren<ExitMode>();
        settingMode = GetComponentInChildren<SettingMode>();
        creditMode = GetComponentInChildren<CreditMode>();
        launchMode = GetComponentInChildren<LaunchMode>();
    }

}
