using System;
using System.IO;
using UnityEngine;


public class InformationManager : MonoBehaviour
{
    public SaveData data;     // json�ϊ�����f�[�^�̃N���X
    string filepath;                            // json�t�@�C���̃p�X
    string fileName = "Data.json";              // json�t�@�C����

    public int courseNum;
    public int stageNum;
    public int stageLevel;

    [HideInInspector] public StageFrameManager stageFrameMg;
    ShutterManager shutterMg;

    private void Awake()
    {
        GetComponent();

#if   UNITY_EDITOR
        Debug.Log("UniryEditor����N��");
        filepath = Application.dataPath + "/" + fileName;// �p�X���擾
#elif UNITY_ANDROID
        Debug.Log("UniryAndroid����N��");
        filepath = Application.persistentDataPath + "/" + fileName;
#endif

        if (!File.Exists(filepath))
        {
            Save(data);
            Debug.Log("�t�@�C����������܂���");// �t�@�C�����Ȃ��Ƃ��A�t�@�C���쐬  
        }
        data = Load(filepath); // �t�@�C����ǂݍ����data�Ɋi�[
    }

    void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // json�Ƃ��ĕϊ�
        StreamWriter wr = new StreamWriter(filepath, false);    // �t�@�C���������ݎw�� �J��
        wr.WriteLine(json);                                     // json�ϊ�����������������
        wr.Close();                                             // �t�@�C������
    }

    SaveData Load(string path)// json�t�@�C���ǂݍ���
    {
        StreamReader rd = new StreamReader(path);               // �t�@�C���ǂݍ��ݎw��@�J��
        string json = rd.ReadToEnd();                           // �t�@�C�����e�S�ēǂݍ���
        rd.Close();                                             // �t�@�C������
        return JsonUtility.FromJson<SaveData>(json);            // json�t�@�C�����^�ɖ߂��ĕԂ�
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
        shutterMg.CloseToHome("Title");
    }

}
