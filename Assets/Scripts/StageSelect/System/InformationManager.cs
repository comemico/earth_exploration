using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;


public class InformationManager : MonoBehaviour
{
    public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string fileName = "Data.json";              // jsonファイル名

    [Header("全エリアのScatterTypeを入れておく")]
    public List<StageInformation> scatterList; //コースエリアをまたいで全部入れる（ScatterTypeステージ）

    public int stageAdress;//最後に選択したステージの情報（）
    public enum StageType
    {
        Linear = 0,
        Scatter
    }
    [Header("選択されたステージ情報")]
    public int courseNum;
    public StageType stageType;
    public int stageNum;
    [Range(1, 37)] public int stageLevel;

    [HideInInspector] public StageFrameManager stageFrameMg;
    ShutterManager shutterMg;

    private void Awake()
    {
        GetComponent();

#if   UNITY_EDITOR
        //Debug.Log("UniryEditorから起動");
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

        for (int i = 0; i < scatterList.Count; i++)
        {
            scatterList[i].isDiscover = data.scatterData[i]; //Scatterデータをロードする
        }
        stageAdress = data.recentStageAdress;
    }
    void GetComponent()
    {
        shutterMg = GetComponent<ShutterManager>();
        stageFrameMg = GetComponentInChildren<StageFrameManager>();
    }

    private void Start()
    {
    }

    void Save(SaveData data)
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


    public void UpdateStageInformation(StageInformation stageInfo, int stageAdress)
    {
        this.stageNum = stageInfo.stageNum;
        this.stageLevel = stageInfo.stageLevel;
        this.stageType = (StageType)stageInfo.stageType;
        this.stageAdress = stageAdress;
    }

    public void UpdateCourseNumber(int courseNumber = 0)
    {
        this.courseNum = courseNumber;
        GManager.instance.recentCourseNum = this.courseNum;
        //courseName = (AreaType)Enum.ToObject(typeof(AreaType), courseNumber);
    }

    public void StartGame()
    {
        var sceneName = "Area" + courseNum + stageType + "Stage" + stageNum;
        shutterMg.CloseToStart(sceneName);

        data.recentCourseAdress = courseNum;
        data.recentStageAdress = stageAdress;

        Save(data);

        /*
        if (EditorBuildSettings.scenes.Any(scene => Path.GetFileNameWithoutExtension(scene.path) == sceneName))
        {
            //Debug.Log($"指定ののシーン{sceneName}はビルドに設定されています");
            //シーン遷移
        }
        else
        {
            Debug.Log("指定のシーンはビルドに設定されていません");
            //Debug.Log($"指定のシーン{sceneName}はビルドに設定されていません");
        }
         */

    }

    public void ToTitleScene()
    {
        shutterMg.CloseToHome("Title");
    }

}
