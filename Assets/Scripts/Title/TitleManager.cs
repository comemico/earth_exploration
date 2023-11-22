using System.IO;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string fileName = "Data.json";              // jsonファイル名

    void Awake()
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
            Debug.Log("ファイルが見つかりません初期化しました");// ファイルがないとき、ファイル作成  
            ResetData();
            Save(data);
        }

        data = Load(filepath);// ファイルを読み込んでdataに格納
    }

    private void Start()
    {
        SetSaveData(this.data);
    }

    // jsonとしてデータを保存
    void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        //wr.Flush();
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる
        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }


    public void SetSaveData(SaveData data)
    {
        GManager.instance.courseDate = data.linearData;
        GManager.instance.isRerease = data.scatterClear;
        GManager.instance.maxLifeNum = data.maxLifeNum;
        GManager.instance.recentCourseNum = data.recentCourseAdress;
        GManager.instance.recentStageNum = data.recentStageAdress;
    }



    public void ResetData() //初期化
    {
        Debug.Log("初期化");
        data.maxLifeNum = 3;
        data.recentCourseAdress = 0;
        data.recentStageAdress = 0;

        data.linearData = new int[2]; //コースエリア数
        data.scatterDiscover = new bool[2]; //ScatterStageの数
        data.scatterClear = new bool[2]; //上記の数量に揃える

        for (int i = 0; i < data.linearData.Length; i++) data.linearData[i] = 0;
        for (int i = 0; i < data.scatterDiscover.Length; i++) data.scatterDiscover[i] = false;
        for (int i = 0; i < data.scatterClear.Length; i++) data.scatterClear[i] = false;

        SetSaveData(data);
    }


    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }
}
