using System;
using UnityEngine;
using UnityEditor;


public class InformationManager : MonoBehaviour
{
    /*
    [Header("選択コース")]
    public AreaType courseName;
    public enum AreaType
    {
        [InspectorName("入門")] 入門,
        [InspectorName("峠")] 峠,
        [InspectorName("屋上")] 屋上,
        [InspectorName("遊園地")] 遊園地
    }
    public int maxAreaLevel;[Header("このエリアのレベル上限")]
     */

    public int courseNum;
    public int stageNum;//[Header("ステージ番号")]
    public int stageLevel;//[Header("ステージレベル")]
    //public int stageLifeNum;//[Header("ライフメモリ数")]

    [HideInInspector] public StageFrameManager stageFrameMg;
    ShutterManager shutterMg;

    private void Awake()
    {
        GetComponent();
    }

    void GetComponent()
    {
        shutterMg = GetComponent<ShutterManager>();
        stageFrameMg = GetComponentInChildren<StageFrameManager>();
    }

    public void UpdateStageInformation(StageInformation stageInfo)
    {
        this.stageNum = (int)stageInfo.stageNumber;
        GManager.instance.recentStageNum = this.stageNum;
        this.stageLevel = (int)stageInfo.stageLevel;
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
        shutterMg.CloseShutter(sceneName);

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
        shutterMg.CloseShutter("Title");
    }

}
