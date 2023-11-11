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

    [Header("プレイヤー情報とスタート")]
    public GrypsController grypsCrl;

    [Header("スタートポジション")]
    public GateManager gateMg;

    [Header("ステージ情報")]
    public string tipsText;
    [Range(1, 5)] public int stageRank;
    public Color[] rankColor;

    [Header("エリア範囲")]
    public float deadLineY;

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
    [Header("No.2")] [HideInInspector] public SaltoManager saltoMg;
    [Header("No.3")] [HideInInspector] public MemoryGageManager memoryGageMg;
    [Header("No.4")] [HideInInspector] public JetManager jetMg;
    [Header("No.5")] [HideInInspector] public PauseManager pauseMg;
    [Header("No.6")] [HideInInspector] public ResultManager resultMg;
    [Header("No.7")] [HideInInspector] public CurtainManager curtainMg;

    private void Awake()
    {
        GetAllComponent();
    }

    void GetAllComponent()
    {
        controlScreenMg = GetComponentInChildren<ControlScreenManager>();
        saltoMg = GetComponentInChildren<SaltoManager>();
        memoryGageMg = GetComponentInChildren<MemoryGageManager>();
        jetMg = GetComponentInChildren<JetManager>();
        pauseMg = GetComponentInChildren<PauseManager>();
        resultMg = GetComponentInChildren<ResultManager>();
        curtainMg = GetComponentInChildren<CurtainManager>();

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

        sequenceStart.Append(curtainMg.StartUp());
        sequenceStart.AppendInterval(0.15f);
        sequenceStart.Append(gateMg.OpenHole());//鍵穴が開く 
        sequenceStart.AppendCallback(() =>
        {
            grypsCrl.rb.constraints = RigidbodyConstraints2D.None;
            grypsCrl.ForceDash((int)grypsCrl.transform.localScale.x, 1);
            sequenceStart.Kill();
        });
    }

    public void ChangeControlStatus(ControlStatus status)
    {
        controlStatus = status;
        controlScreenMg.ChangeControlLimit(status);
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

    public void GameOver()
    {
        state = State.GameOver;
        grypsCrl.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //目を暗くする
    }

    public void StageClear()
    {
        state = State.Clear;
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
                if (grypsCrl.transform.position.y <= deadLineY)
                {
                    resultMg.Result(ResultManager.CAUSE.missFall);
                    GameOver();
                    return;
                }
                break;

            case State.Lack:
                if (grypsCrl.transform.position.y <= deadLineY)
                {
                    resultMg.Result(ResultManager.CAUSE.missFall);
                    GameOver();
                    return;
                }
                if (Mathf.Abs(grypsCrl.rb.velocity.x) < 0.1f && controlStatus == ControlStatus.control)
                {
                    ChangeControlStatus(ControlStatus.unControl);
                    resultMg.Result(ResultManager.CAUSE.missLack);
                    GameOver();
                    return;
                }
                break;
        }

    }

    public void Retry() //ゲームオーバー時、ボタン入力可能
    {
        FadeCanvasManager.instance.LoadFade(SceneManager.GetActiveScene().name);
        //        Time.timeScale = 1f;
    }

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
