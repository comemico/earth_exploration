using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    public StartBandManager startBandMg;
    public SelectBandManager selectBandMg;
    public TitleModeManager modeMg;
    public TitleCurtainManager curtainMg;
    public TitleCinemachine cinemachineMg;

    [Header("LaunchButton")]
    public Button launchButton;

    [Header("TrafficButton")]
    public Button exitButton;
    public Button settingButton;
    public Button creditButton;

    List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        launchButton.onClick.AddListener(() => curtainMg.CloseCurtain("StageSelect"));
        exitButton.onClick.AddListener(() => modeMg.OpenExit());
        settingButton.onClick.AddListener(() => modeMg.OpenSetting());
        creditButton.onClick.AddListener(() => modeMg.OpenCredit());
    }


    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }



}
