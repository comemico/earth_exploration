using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveData data;          // json変換するデータのクラス
    string filepath;               // jsonファイルのパス
    string fileName = "Data.json"; // jsonファイル名

    void Awake()
    {
#if   UNITY_EDITOR
        Debug.Log("UniryEditorから起動");
        filepath = Application.dataPath + "/" + fileName;// パス名取得
#elif UNITY_ANDROID
        Debug.Log("UniryAndroidから起動");
        filepath = Application.persistentDataPath + "/" + fileName;
#endif
        Application.targetFrameRate = 50;

        if (!File.Exists(filepath))
        {
            Debug.Log("ファイルが見つかりません初期化しました");// ファイルがないとき、ファイル作成  
            DeleteAll();
            Save(data);
        }

        data = Load(filepath);// ファイルを読み込んでdataに格納
    }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる
        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }

    // jsonとしてデータを保存
    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        //wr.Flush();
        wr.Close();                                             // ファイル閉じる
    }

    public void DeleteAll() //初期化
    {
        Debug.Log("初期化");
        data.maxLifeNum = 15;
        data.recentCourseAdress = 0;
        data.recentStageAdress = 0;

        data.linearData = new int[3]; //コースエリア総数
        data.scatterDiscover = new bool[2]; //ScatterStageの総数
        data.scatterClear = new bool[2]; //上記の数量に揃える

        data.isGuide = false;

        for (int i = 0; i < data.linearData.Length; i++) data.linearData[i] = 0;
        for (int i = 0; i < data.scatterDiscover.Length; i++) data.scatterDiscover[i] = false;
        for (int i = 0; i < data.scatterClear.Length; i++) data.scatterClear[i] = false;
    }

    // ゲーム終了時に保存
    void OnDestroy() => Save(data);

}
