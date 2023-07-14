using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{

    SceneTransitionManager sceneTransitionMg;

    private void Start()
    {
        sceneTransitionMg = GetComponent<SceneTransitionManager>();
    }

    public void StartGame()
    {
        FadeCanvasManager.instance.LoadFade("StageSelect");
    }

    public void SelectButton()
    {
        Invoke("SceneTransition", 1f);
    }

    void SceneTransition()
    {
        sceneTransitionMg.SceneTo("StageSelect");
    }

    public void DeleteDate()
    {
        PlayerPrefs.DeleteAll();
    }




}
