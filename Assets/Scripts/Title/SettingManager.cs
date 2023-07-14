using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Button push_Close;
    PopController popCtrl;

    void Start()
    {
        popCtrl = GetComponent<PopController>();
        AddListener();
    }

    void AddListener()
    {
        push_Close.onClick.AddListener(() => popCtrl.ClosePanel(false));
    }
}
