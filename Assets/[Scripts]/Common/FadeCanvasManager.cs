using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class FadeCanvasManager : MonoBehaviour
{
    #region//シングルトン
    public static FadeCanvasManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("フェード演出")]
    [Header("-----------------------------")]
    public bool isFade;

    [Space(PreHeader)]
    [Header("バックパネル")]
    [Header("-----------------------------")]
    [Header("イージング")]
    public Ease easeType_BackPanel = Ease.OutSine;
    [Range(0.25f, 1f)] public float fadeInTime = 0.25f;
    [Range(0.25f, 1f)] public float fadeOutTime = 0.25f;

    CanvasGroup backPanel;
    LoadingAnimPlayer loadingAnimPlayer;
    AsyncOperation async;

    Tween tween = null;

    private void OnDisable()
    {
        if (DOTween.instance != null)
        {
            tween?.Kill();// Tween破棄
        }
    }

    void Start()
    {
        backPanel = transform.GetChild(0).GetComponent<CanvasGroup>();
        loadingAnimPlayer = GetComponentInChildren<LoadingAnimPlayer>();
    }


    public void LoadScene(string sceneName)
    {
        isFade = false;
        backPanel.blocksRaycasts = true;//タッチブロック
        loadingAnimPlayer.PlayLoadAnim();//グルグル起動

        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
    }


    public void CheckLoad()
    {
        StartCoroutine(CoroutineLoad());
    }

    IEnumerator CoroutineLoad()
    {
        while (!async.isDone)
        {
            yield return null;
            if (async.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.35f);
                loadingAnimPlayer.KillLoadAnim();
                backPanel.blocksRaycasts = false;
                async.allowSceneActivation = true;
                break;//11/04解決の糸口:whileの脱出方法
            }
        }
    }

    public void LoadFade(string sceneName)
    {
        isFade = true;
        backPanel.blocksRaycasts = true;//タッチブロック
        loadingAnimPlayer.PlayLoadAnim();//グルグル起動

        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        tween = backPanel.DOFade(1f, fadeOutTime).OnComplete(() =>
            {
                StartCoroutine(CoroutineFade());
                Time.timeScale = 0f;
                tween.Kill();
            });
    }

    IEnumerator CoroutineFade()
    {
        while (!async.isDone)
        {
            yield return null;
            //float progressVal = Mathf.Clamp01(async.progress / 0.9f);Slider用: async.progressは実際には 0～0.9 まで値が変化する
            if (async.progress >= 0.9f)
            {
                //yield return new WaitForSeconds(0.5f);
                loadingAnimPlayer.KillLoadAnim();
                async.allowSceneActivation = true;
                FadeIn();
                break;//11/04解決の糸口:whileの脱出方法
            }
        }
    }

    public void FadeIn()
    {
        tween = backPanel.DOFade(0, fadeInTime).SetDelay(0.5f).OnComplete(() =>
        {
            backPanel.blocksRaycasts = false;
            Time.timeScale = 1.0f;
            tween.Kill();
        });
    }


}

