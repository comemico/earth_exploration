using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    [SerializeField] private GameObject loadUI;
    [SerializeField] private Slider slider;
    private AsyncOperation async;
    public SceneTransitionManager sceneTransition;


    void Start()
    {
        NextScene();
    }

    public void NextScene()
    {
        loadUI.SetActive(true);
        StartCoroutine("LoadData");
    }

    IEnumerator LoadData()
    {
        yield return new WaitForSeconds(1.0f);
        //async.allowSceneActivation = false;
        async = SceneManager.LoadSceneAsync("StageSelect");
        while (!async.isDone)
        {
            var progressVal = Mathf.Clamp01(async.progress / 0.9f);//async.progress�͎��ۂɂ� 0�`0.9 �܂Œl���ω�����
            slider.value = progressVal;
            yield return null;
        }
        //async.allowSceneActivation = true;
    }
}