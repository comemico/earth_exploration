using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoundSystem;


//シーン遷移にかかわるスクリプト
public class SceneTransitionManager : MonoBehaviour
{

    //ボタン入力される関数
    public void SceneTo(string sceneName)
    {
        SoundController.instance.PlayMenuSe("button02a");
        FadeCanvasManager.instance.LoadScene(sceneName);
    }


    public void SceneToAnim(string sceneName)
    {
        SoundController.instance.PlayMenuSe("button02a");
        //FadeMarkManager.instance.FadeOutToInAnim(() => Load(sceneName));
    }

    //シーン遷移
    private void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }



}
