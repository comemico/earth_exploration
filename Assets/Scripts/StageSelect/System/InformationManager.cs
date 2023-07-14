using System;
using UnityEngine;
using UnityEditor;
/*
#if UNITY_EDITOR
#endif
 */

public class InformationManager : MonoBehaviour
{
    const float PreHeader = 20f;// ヘッダー前のスペース

    [Space(PreHeader)]
    [Header("確認")]
    [Header("-----------------------------")]
    [Header("選択コース")]
    public AreaType courseName;
    public enum AreaType
    {
        [InspectorName("入門")] 入門,
        [InspectorName("峠")] 峠,
        [InspectorName("屋上")] 屋上,
        [InspectorName("遊園地")] 遊園地
    }
    public int courseNum;

    [Header("このエリアのレベル上限")]
    public int maxAreaLevel;

    [Header("ステージ番号")]
    public int stageNum;
    [Header("ステージレベル")]
    public int stageLevel;
    [Header("ライフメモリ数")]
    public int lifeNum;


    [HideInInspector] public StageFrameManager stageFrameMg;
    ShutterManager shutterMg;
    SceneTransitionManager sceneTransitionMg;

    private void Awake()
    {
        shutterMg = GetComponent<ShutterManager>();
        sceneTransitionMg = GetComponent<SceneTransitionManager>();
        stageFrameMg = GetComponentInChildren<StageFrameManager>();
    }


    public void UpdateStageInformation(StageInformation stageInfo)
    {
        this.stageNum = (int)stageInfo.stageNumber;
        this.lifeNum = stageInfo.lifeNumber;
        this.stageLevel = (int)stageInfo.stageLevel;

    }

    public void UpdateCourseNumber(int courseNumber = 0, int maxAreaLevel = 0)
    {
        GManager.instance.recentCourseNum = courseNumber;
        this.courseNum = courseNumber;
        this.maxAreaLevel = maxAreaLevel;
        stageFrameMg.DisplayMaxLevel(maxAreaLevel);
        stageFrameMg.ChangeTarget(this.stageLevel);

        courseName = (AreaType)Enum.ToObject(typeof(AreaType), courseNumber);
    }


    public void StartGame()
    {
        var sceneName = "area" + courseNum + "stage" + stageNum;
        FadeCanvasManager.instance.LoadScene(sceneName);
        shutterMg.ShutterClose(false);

        //sceneTransitionMg.SceneTo(sceneName);
        /*
        if (EditorBuildSettings.scenes.Any(scene => Path.GetFileNameWithoutExtension(scene.path) == sceneName))
        {
            //Debug.Log($"指定ののシーン{sceneName}はビルドに設定されています");
            //シーン遷移
        }
        else
        {
            Debug.Log($"指定のシーン{sceneName}はビルドに設定されていません");
        }
         */
    }

    public void ToTitleScene()
    {
        FadeCanvasManager.instance.LoadFade("Title");
    }

}
