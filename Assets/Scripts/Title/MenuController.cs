using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button push_Setting;
    public Button push_Credit;
    public Button push_Exit;
    public Button push_Start;

    public PopController popCtrl_Setting;
    public PopController popCtrl_Credit;
    public PopController popCtrl_Exit;
    public StartManager startMg;

    void Start()
    {
        AddListener();
    }

    void AddListener()
    {
        push_Setting.onClick.AddListener(popCtrl_Setting.OpenPanel);
        push_Credit.onClick.AddListener(popCtrl_Credit.OpenPanel);
        push_Exit.onClick.AddListener(popCtrl_Exit.OpenPanel);
        push_Start.onClick.AddListener(startMg.StartGame);
    }

}
