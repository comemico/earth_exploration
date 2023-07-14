using System;
using UnityEngine;
using UnityEditor;
/*
#if UNITY_EDITOR
#endif
 */

public class InformationManager : MonoBehaviour
{
    const float PreHeader = 20f;// �w�b�_�[�O�̃X�y�[�X

    [Space(PreHeader)]
    [Header("�m�F")]
    [Header("-----------------------------")]
    [Header("�I���R�[�X")]
    public AreaType courseName;
    public enum AreaType
    {
        [InspectorName("����")] ����,
        [InspectorName("��")] ��,
        [InspectorName("����")] ����,
        [InspectorName("�V���n")] �V���n
    }
    public int courseNum;

    [Header("���̃G���A�̃��x�����")]
    public int maxAreaLevel;

    [Header("�X�e�[�W�ԍ�")]
    public int stageNum;
    [Header("�X�e�[�W���x��")]
    public int stageLevel;
    [Header("���C�t��������")]
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
            //Debug.Log($"�w��̂̃V�[��{sceneName}�̓r���h�ɐݒ肳��Ă��܂�");
            //�V�[���J��
        }
        else
        {
            Debug.Log($"�w��̃V�[��{sceneName}�̓r���h�ɐݒ肳��Ă��܂���");
        }
         */
    }

    public void ToTitleScene()
    {
        FadeCanvasManager.instance.LoadFade("Title");
    }

}
