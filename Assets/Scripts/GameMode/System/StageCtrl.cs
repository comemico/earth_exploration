using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageCtrl : MonoBehaviour
{
    #region//StageSelectシーンから受け取ってくる
    [Header("エリア番号")] public int areaNum;
    [Header("ステージ番号")] public int stageNum;
    [Header("このステージのメモリ数")] public int totalMemoryGage;
    #endregion

    [Header("プレイヤー情報とスタート")] public GrypsController grypsCrl;
    [Header("スタートポジション")] public GateManager gateMg;

    public ControlStatus controlStatus;
    public State state;

    SceneTransitionManager sceneTransitionManager;

    public enum ControlStatus
    {
        [InspectorName("操作不能")] unControl,
        [InspectorName("操作可能")] control,
        [InspectorName("操作一部可能")] restrictedControl
    }

    public enum State
    {
        Ready,
        Play,
        Lack,
        GameOver,
        Clear
    }

    [Header("No.1")] [HideInInspector] public ControlScreenManager controlScreenMg;

    [Header("No.3")] [HideInInspector] public MemoryGageManager memoryGageMg;
    [Header("No.4")] [HideInInspector] public JetManager jetMg;
    [Header("No.5")] [HideInInspector] public PauseManager pauseMg;
    [Header("No.6")] [HideInInspector] public ResultManager resultMg;
    [Header("No.7")] [HideInInspector] public ShutterController shutterMg;

    private void Awake()
    {
        GetAllComponent();
    }

    void GetAllComponent()
    {
        controlScreenMg = GetComponentInChildren<ControlScreenManager>();
        memoryGageMg = GetComponentInChildren<MemoryGageManager>();
        jetMg = GetComponentInChildren<JetManager>();
        resultMg = GetComponentInChildren<ResultManager>();
        pauseMg = GetComponentInChildren<PauseManager>();
        shutterMg = GetComponentInChildren<ShutterController>();

        sceneTransitionManager = GetComponent<SceneTransitionManager>();
    }

    void Start()
    {
        Application.targetFrameRate = 50;
        GameStart();
    }

    public void GameStart()
    {
        state = State.Play;
        gateMg.SetStartPosition(grypsCrl.gameObject); //スタート位置に移動　
    }

    public void EnterStageSequence()
    {
        Sequence sequenceStart = DOTween.Sequence();

        sequenceStart.Append(gateMg.OpenHole());//鍵穴が開く 
        sequenceStart.AppendCallback(() =>
        {
            grypsCrl.rb.constraints = RigidbodyConstraints2D.None;
            grypsCrl.ForceDash((int)grypsCrl.transform.localScale.x, 1);
            sequenceStart.Kill();
        });
        /*
        //shutterMg.ShutterClose(true);
        sequenceStart.AppendInterval(0.5f);
        sequenceStart.Append(shutterMg.ShutterOpen(FadeCanvasManager.instance.isFade));//シャッターが開く
        sequenceStart.AppendInterval(0.5f);//待つ delay
         */
    }

    public void ChangeControlStatus(ControlStatus status)
    {
        controlStatus = status;
        controlScreenMg.ChangeControlLimit(controlStatus == ControlStatus.unControl);
        pauseMg.push_Pause.interactable = (controlStatus != ControlStatus.unControl);
    }


    public void Ready()
    {
        state = State.Ready;
        /*
        if (GManager.instance.lifeNum > GManager.instance.maxLifeNum)
        {
            GManager.instance.lifeNum = GManager.instance.maxLifeNum;
            Debug.Log("上限を超えたため、max値に変更");
        }
        memoryGageMg.memoryGage = GManager.instance.lifeNum;
        memoryGageMg.DisplayMemoryGage(GManager.instance.lifeNum);
        */
    }

    public void Lack()
    {
        state = State.Lack;
        //目を点滅
    }

    public void Regeneration()
    {
        state = State.Play;
        //目の点滅を戻す
    }
    /*
    public void ResultA(ResultManager.RESULT result)
    {
        state = State.Clear;
        ChangeControlStatus(ControlStatus.unControl);
    }
     */

    public void GameOver()
    {
        state = State.GameOver;
        //timelineMg.TapeChangeUI(2);
        //timelineMg.PlayTimeline();
        //目を暗くする
    }


    public void StageClear()
    {
        state = State.Clear;
        //timelineMg.TapeChangeUI(1);
        //timelineMg.PlayTimeline();

        //最大ステージ番号更新(エリアの最大到達番号と同じ)
        if (stageNum == GManager.instance.courseDate[areaNum])
        {
            GManager.instance.courseDate[areaNum]++;
        }
    }


    void LateUpdate()
    {

        switch (state)
        {

            case State.Ready:
                break;


            case State.Play:
                break;

            case State.Lack:
                if (Mathf.Abs(grypsCrl.rb.velocity.x) < 0.1f && !grypsCrl.isFreeze)
                {
                    //grypsCrl.PausePlayer();
                    ChangeControlStatus(ControlStatus.unControl);
                    GameOver();
                    return;
                }
                break;

        }

    }

    public void Retry() //ゲームオーバー時、ボタン入力可能
    {
        //SoundController.instance.PlayMenuSe("button02a");
        FadeCanvasManager.instance.LoadFade(SceneManager.GetActiveScene().name);
        //        Time.timeScale = 1f;
        //        FadeCanvasManager.instance.LoadScene(SceneManager.GetActiveScene().name);
        //        FadeCanvasManager.instance.FadeOutTest(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    //ボタン入力
    public void Next()
    {
        stageNum++;
        sceneTransitionManager.SceneTo("Stage" + stageNum);
    }

    //ボタン入力
    public void SelectStage()
    {
        FadeCanvasManager.instance.LoadFade("StageSelect");
    }


    /*
    private void UpdateStageNum()
    {
        PlayerPrefs.SetFloat(GManager.instance.loadStageNum, GManager.instance.stageNum);
    }
    */


}
