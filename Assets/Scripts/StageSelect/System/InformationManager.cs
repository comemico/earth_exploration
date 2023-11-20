using System;
using System.IO;
using UnityEngine;


public class InformationManager : MonoBehaviour
{
    public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string fileName = "Data.json";              // jsonファイル名

    public int courseNum;
    public int stageNum;
    public int stageLevel;

    [HideInInspector] public StageFrameManager stageFrameMg;
    ShutterManager shutterMg;

    private void Awake()
    {
        GetComponent();

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

    void GetComponent()
    {
        shutterMg = GetComponent<ShutterManager>();
        stageFrameMg = GetComponentInChildren<StageFrameManager>();
    }

    public void UpdateStageInformation(StageInformation stageInfo)
    {
        this.stageNum = stageInfo.stageNum;
        GManager.instance.recentStageNum = this.stageNum;
        this.stageLevel = stageInfo.stageLevel;
    }

    public void UpdateCourseNumber(int courseNumber = 0)
    {
        this.courseNum = courseNumber;
        GManager.instance.recentCourseNum = this.courseNum;
        //courseName = (AreaType)Enum.ToObject(typeof(AreaType), courseNumber);
    }

    public void StartGame()
    {
        var sceneName = "area" + courseNum + "stage" + stageNum;
        shutterMg.CloseToStart(sceneName);

        data.recentCourseNum = courseNum;
        data.recentStageNum = stageNum;

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
