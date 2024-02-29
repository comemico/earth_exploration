using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;


public class InformationManager : MonoBehaviour
{
    [Header("�I�����ꂽ�X�e�[�W���")]
    public int courseNum;
    public StageType stageType;
    public int stageNum;
    [Range(1, 37)] public int stageLevel;


    [Header("�S�G���A��ScatterType�����Ă���")]
    public List<StageInformation> scatterList; //�R�[�X�G���A���܂����őS�������iScatterType�X�e�[�W�j
    public int stageAdress;//�Ō�ɑI�������X�e�[�W�̏��i�j
    public enum StageType
    {
        Linear = 0,
        Scatter
    }

    [Header("�Z�[�u�f�[�^")]
    public SaveData data;     // json�ϊ�����f�[�^�̃N���X
    string filepath;                            // json�t�@�C���̃p�X
    string fileName = "Data.json";              // json�t�@�C����


    [HideInInspector] public StageFrameManager stageFrameMg;
    ShutterManager shutterMg;
    MemoryGageManager memoryGageMg;

    private void Awake()
    {
#if   UNITY_EDITOR
        //Debug.Log("UniryEditor����N��");
        filepath = Application.dataPath + "/" + fileName;// �p�X���擾
#elif UNITY_ANDROID
        //Debug.Log("UniryAndroid����N��");
        filepath = Application.persistentDataPath + "/" + fileName;
#endif
        if (!File.Exists(filepath))
        {
            Save(data);
            Debug.Log("�t�@�C����������܂���");// �t�@�C�����Ȃ��Ƃ��A�t�@�C���쐬  
        }
        data = Load(filepath); // �t�@�C����ǂݍ����data�Ɋi�[

        GetComponent();

        for (int i = 0; i < scatterList.Count; i++)
        {
            scatterList[i].isDiscover = data.scatterDiscover[i]; //�SScatterStage��isDiacover�����[�h����
            scatterList[i].isClear = data.scatterClear[i]; //�SScatterStage��isClear�����[�h����
        }
        memoryGageMg.InitializeMemoryGage(data.maxLifeNum);
        stageAdress = data.recentStageAdress;
    }

    void GetComponent()
    {
        shutterMg = GetComponent<ShutterManager>();
        stageFrameMg = GetComponentInChildren<StageFrameManager>();
        memoryGageMg = GetComponentInChildren<MemoryGageManager>();
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
        //courseName = (AreaType)Enum.ToObject(typeof(AreaType), courseNumber);
    }

    public void StartGame()
    {
        var sceneName = "Area[" + courseNum + "]" + stageType + "[" + stageNum + "]";
        shutterMg.StartGame(sceneName);

        data.recentCourseAdress = courseNum;
        data.recentStageAdress = stageAdress;

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
        shutterMg.BackHome("Title");
    }

}
