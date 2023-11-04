using System;
using UnityEngine;
using UnityEditor;


public class InformationManager : MonoBehaviour
{
    /*
    [Header("�I���R�[�X")]
    public AreaType courseName;
    public enum AreaType
    {
        [InspectorName("����")] ����,
        [InspectorName("��")] ��,
        [InspectorName("����")] ����,
        [InspectorName("�V���n")] �V���n
    }
    public int maxAreaLevel;[Header("���̃G���A�̃��x�����")]
     */

    public int courseNum;
    public int stageNum;//[Header("�X�e�[�W�ԍ�")]
    public int stageLevel;//[Header("�X�e�[�W���x��")]
    //public int stageLifeNum;//[Header("���C�t��������")]

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
            //Debug.Log($"�w��̂̃V�[��{sceneName}�̓r���h�ɐݒ肳��Ă��܂�");
            //�V�[���J��
        }
        else
        {
            Debug.Log("�w��̃V�[���̓r���h�ɐݒ肳��Ă��܂���");
            //Debug.Log($"�w��̃V�[��{sceneName}�̓r���h�ɐݒ肳��Ă��܂���");
        }
         */

    }

    public void ToTitleScene()
    {
        shutterMg.CloseShutter("Title");
    }

}
