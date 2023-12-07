using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    public TitleCurtainManager curtainMg;
    public StartBandManager startBandMg;
    public SelectBandManager selectBandMg;
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
    }

    void Start()
    {
        Sequence seq_start = DOTween.Sequence();
        seq_start.Append(curtainMg.OpenCurtain());
        seq_start.AppendInterval(0f);
        seq_start.Append(startBandMg.StartUp());

    }




}
