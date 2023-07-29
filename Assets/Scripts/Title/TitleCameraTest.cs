using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCameraTest : MonoBehaviour
{
    public Button push_toHud;
    public Button push_toGryps;

    LaunchCameraController launchCrl;

    private void Awake()
    {
        launchCrl = Camera.main.GetComponent<LaunchCameraController>();
    }

    void Start()
    {
        AddListener();
        Application.targetFrameRate = 50;
    }

    void AddListener()
    {
        push_toHud.onClick.AddListener(() => launchCrl.ToFloorVcam(0));
        push_toGryps.onClick.AddListener(() => launchCrl.ToFloorVcam(1));
    }

}
