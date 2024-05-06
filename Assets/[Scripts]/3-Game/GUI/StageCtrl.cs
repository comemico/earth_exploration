using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using DG.Tweening;

public class StageCtrl : MonoBehaviour
{
    public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string fileName = "Data.json";              // jsonファイル名

    public enum StageType
    {
        Linear = 0,
        Scatter
    }
    public enum StageMode
    {
        Normal = 0,
        Exceed
    }

    [Header("ステージ情報 -保存-")]
    public StageType stageType; //リニアルートかスキャッター(分散した)か
    public StageMode stageMode; //ノーマル(特に無し)かエクシード(上限追加)か
    [Range(0, 5)] public int areaNum; //今のエリア番号
    [Range(0, 19)] public int stageNum; //今のステージ番号

    //[Header("このエリアの最終面か")]public bool isUpperLimit;リザルトの「次のステージ」ボタンを非表示にするため.


    [Header("分散ステージを開放させるか")]
    public string stageName;
    public string areaName;
    public bool isReleaseScatter; //リニアルート以外の分岐ステージを開放させるか
    public int releaseScatterNum; //開放するスキャッターステージ番号

    [Header("ステージ情報 -表示-")]
    public string tipsText;
    [Range(1, 5)] public int stageRank;

    public enum ControlStatus
    {
        [InspectorName("UnControl")] unControl,
        [InspectorName("Control")] control,
        [InspectorName("Restricted")] restrictedControl
    }
    public enum State
    {
        Ready,
        Play,
        Lack,
        GameOver,
        Clear
    }

    [Header("プレイヤー情報とスタートフラッグ")]
    public GrypsController grypsCrl;
    public GateStartManager startGateMg;
    public ControlStatus controlStatus;
    public State state;

    [Header("落下判定位置")]
    public float deadLineY;

    [Header("No.1")] [HideInInspector] public ControlScreenManager controlScreenMg;
    [Header("No.2")] [HideInInspector] public SaltoManager saltoMg;
    [Header("No.3")] [HideInInspector] public MemoryGageManager memoryGageMg;
    [Header("No.4")] [HideInInspector] public JetManager jetMg;
    [Header("No.5")] [HideInInspector] public GuideManager guideMg;
    [Header("No.6")] [HideInInspector] public PauseManager pauseMg;
    [Header("No.7")] [HideInInspector] public ResultManager resultMg;
    [Header("No.8")] [HideInInspector] public CurtainManager curtainMg;
    private List<Tween> tweenList = new List<Tween>();
    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Awake()
    {
#if   UNITY_EDITOR
        Debug.Log("UniryEditorから起動");
        filepath = Application.dataPath + "/" + fileName;// パス名取得
#elif UNITY_ANDROID
        Debug.Log("UniryAndroidから起動");
        filepath = Application.persistentDataPath + "/" + fileName;
#endif

        if (!File.Exists(filepath))
        {
            Save(data);
            Debug.Log("ファイルが見つかりません");// ファイルがないとき、ファイル作成  
        }
        data = Load(filepath); // ファイルを読み込んでdataに格納

        Application.targetFrameRate = 50;
        GetAllComponent();
        memoryGageMg.InitializeMemoryGage(data.maxLifeNum);
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定 開く
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる
    }

    SaveData Load(string path)// jsonファイル読み込み
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定　開く
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる
        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }

    void GetAllComponent()
    {
        controlScreenMg = GetComponentInChildren<ControlScreenManager>();
        saltoMg = GetComponentInChildren<SaltoManager>();
        memoryGageMg = GetComponentInChildren<MemoryGageManager>();
        jetMg = GetComponentInChildren<JetManager>();
        guideMg = GetComponentInChildren<GuideManager>();
        pauseMg = GetComponentInChildren<PauseManager>();
        resultMg = GetComponentInChildren<ResultManager>();
        curtainMg = GetComponentInChildren<CurtainManager>();
    }

    void Start()
    {
        state = State.Play;
        StartGame();
    }

    public void StartGame() //タイムラインから呼ばれる
    {
        Sequence s_start = DOTween.Sequence();

        s_start.AppendInterval(0.65f);
        s_start.Append(curtainMg.ShowNameInfo(areaName, stageName));
        s_start.AppendCallback(() => startGateMg.SetStartPosition(grypsCrl.gameObject)); //スタート位置に移動.
        s_start.AppendInterval(0.25f);
        s_start.Append(curtainMg.OpenCurtain());
        s_start.AppendInterval(0.25f);
        s_start.AppendCallback(() => startGateMg.RaiseFlag()); //StartFlag().

        tweenList.Add(s_start);
    }

    public void ChangeControlStatus(ControlStatus status)
    {
        controlStatus = status;
        controlScreenMg.ChangeControlLimit(status);
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
                    resultMg.OpenMissPanel(ResultManager.CAUSE.落下してしまった);
                    //                    resultMg.Result(ResultManager.CAUSE.missFall);
                    GameOver();
                    return;
                }
                break;

            case State.Lack:
                if (grypsCrl.transform.position.y <= deadLineY)
                {
                    resultMg.OpenMissPanel(ResultManager.CAUSE.落下してしまった);
                    //                    resultMg.Result(ResultManager.CAUSE.missFall);
                    GameOver();
                    return;
                }
                if (Mathf.Abs(grypsCrl.rb.velocity.x) < 0.1f && controlStatus == ControlStatus.control)
                {
                    ChangeControlStatus(ControlStatus.unControl);

                    grypsCrl.effector.PowerOffLamp(); //目を暗くする => Result(ResultManager.CAUSE.missLack);
                    //resultMg.Result(ResultManager.CAUSE.missLack);
                    GameOver();
                    return;
                }
                break;
        }

    }


    public void Ready()
    {
        state = State.Ready;
    }

    public void Lack()
    {
        state = State.Lack;
        //目を赤くする
        grypsCrl.effector.headLamp.color = grypsCrl.effector.lampColor[1];
    }

    public void Regeneration()
    {
        state = State.Play;
        //目を青に戻す
        grypsCrl.effector.headLamp.color = grypsCrl.effector.lampColor[0];
    }

    public void GameOver()
    {
        state = State.GameOver;
        grypsCrl.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void StageClear()
    {
        state = State.Clear;
    }

    public void JudgeStageData()
    {
        if (stageType == StageType.Linear)
        {
            if (stageNum >= data.linearData[areaNum]) //初回ステージクリア時
            {
                data.linearData[areaNum]++;
                if (stageMode == StageMode.Exceed)
                {
                    if (data.maxLifeNum >= 40)
                    {
                        data.maxLifeNum++;
                        memoryGageMg.ExceedLimit(data.maxLifeNum);
                    }
                }
            }
        }
        else if (stageType == StageType.Scatter)
        {
            if (!data.scatterClear[stageNum]) //初回ステージクリア時 => clearData[Num]がfalseの場合.
            {
                data.scatterClear[stageNum] = true;
                if (stageMode == StageMode.Exceed)
                {
                    if (data.maxLifeNum >= 40)
                    {
                        data.maxLifeNum++;
                        memoryGageMg.ExceedLimit(data.maxLifeNum);
                    }
                }
            }
        }

        if (isReleaseScatter) //分散ステージ＆隠しステージ開放させるか
        {
            if (data.scatterDiscover[releaseScatterNum] == false)
            {
                data.scatterDiscover[releaseScatterNum] = true;
            }
        }

        //data.maxLifeNum++; //デバック用
        //memoryGageMg.ExceedLimit(data.maxLifeNum); //デバック用
        Save(data);
    }




}
