using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoundSystem;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    #region//StageSelectシーンから受け取ってくる
    [Header("エリア番号")] public int areaNum;
    [Header("ステージ番号")] public int stageNum;
    [Header("このステージのメモリ数")] public int totalMemoryGage;
    #endregion

    //[Header("プレイヤー情報とスタート")] public GrypsManager grypsMg;
    [Header("プレイヤー情報とスタート")] public GrypsController grypsCrl;
    [Header("スタートポジション")] public GameObject startPosition;
    [Header("タイムライン")] public TimelineManager timelineMg;

    [HideInInspector] public ControlScreenManager controlScreenMg;
    [HideInInspector] public MemoryGageManager memoryGageMg;
    [HideInInspector] public JetMemoryManager jetMemoryMg;
    private SceneTransitionManager sceneTransitionManager;
    private CinemachineController cinemachineCtrl;

    public ControlStatus controlStatus;
    public enum ControlStatus
    {
        [InspectorName("操作不能")] unControl,
        [InspectorName("操作可能")] control,
        [InspectorName("操作一部可能")] restrictedControl
    }

    public State state;
    public enum State
    {
        Ready,
        Play,
        Lack,
        GameOver,
        Clear
    }


    private void Awake()
    {
        GetAllComponent();
    }
    void Start()
    {
        grypsCrl.transform.position = startPosition.transform.position;
        ChangeToUncontrol();
        //grypsCrl.isFreeze = true;
        //Ready();
    }

    void GetAllComponent()
    {
        sceneTransitionManager = GetComponent<SceneTransitionManager>();
        controlScreenMg = GetComponentInChildren<ControlScreenManager>();
        memoryGageMg = GetComponentInChildren<MemoryGageManager>();
        jetMemoryMg = GetComponentInChildren<JetMemoryManager>();
        cinemachineCtrl = Camera.main.GetComponent<CinemachineController>();
        if (sceneTransitionManager == null || memoryGageMg == null || jetMemoryMg == null || cinemachineCtrl == null)
        {
            Debug.Log("StageCtrl.cs: warning : スクリプトが正しくアタッチされていません");
        }

    }

    public void ChangeToUncontrol()
    {
        controlStatus = ControlStatus.unControl;
        controlScreenMg.ChangeControlLimit(false);
    }

    public void ChangeToControl()
    {
        controlStatus = ControlStatus.control;
        controlScreenMg.ChangeControlLimit(true);
    }

    public void ChangeToRestrictedControl()
    {
        controlStatus = ControlStatus.restrictedControl;
        controlScreenMg.ChangeControlLimit(true);
    }

    public void Ready()
    {

        state = State.Ready;
        //ChangeToUncontrol();
        /*
        if (GManager.instance.lifeNum > GManager.instance.maxLifeNum)
        {
            GManager.instance.lifeNum = GManager.instance.maxLifeNum;
            Debug.Log("上限を超えたため、max値に変更");
        }
        memoryGageMg.memoryGage = GManager.instance.lifeNum;
        memoryGageMg.DisplayMemoryGage(GManager.instance.lifeNum);

         */
        //memoryGageMg.DotweenBlueGage(0, GManager.instance.lifeNum, 1.0f);
        //UpdateStageNum();
    }


    public void GameStart()
    {
        state = State.Play;
        ChangeToControl();
        //grypsCrl.isFreeze = false;
        cinemachineCtrl.isRock = false;
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

    public void GameOver()
    {
        state = State.GameOver;
        timelineMg.TapeChangeUI(2);
        timelineMg.PlayTimeline();
        //目を暗くする
    }


    public void StageClear()
    {
        state = State.Clear;
        timelineMg.TapeChangeUI(1);
        timelineMg.PlayTimeline();

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
                    grypsCrl.PausePlayer();
                    GameOver();
                    return;
                }
                break;

        }

    }

    public void Retry() //ゲームオーバー時、ボタン入力可能
    {
        SoundController.instance.PlayMenuSe("button02a");
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
