using System.IO;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public SaveData data;     // json�ϊ�����f�[�^�̃N���X
    string filepath;                            // json�t�@�C���̃p�X
    string fileName = "Data.json";              // json�t�@�C����

    void Awake()
    {
#if   UNITY_EDITOR
        Debug.Log("UniryEditor����N��");
        filepath = Application.dataPath + "/" + fileName;// �p�X���擾
#elif UNITY_ANDROID
        Debug.Log("UniryAndroid����N��");
        filepath = Application.persistentDataPath + "/" + fileName;
#endif

        if (!File.Exists(filepath))
        {
            Debug.Log("�t�@�C����������܂��񏉊������܂���");// �t�@�C�����Ȃ��Ƃ��A�t�@�C���쐬  
            ResetData();
            Save(data);
        }

        data = Load(filepath);// �t�@�C����ǂݍ����data�Ɋi�[
    }

    private void Start()
    {
        SetSaveData(this.data);
    }

    // json�Ƃ��ăf�[�^��ۑ�
    void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // json�Ƃ��ĕϊ�
        StreamWriter wr = new StreamWriter(filepath, false);    // �t�@�C���������ݎw��
        wr.WriteLine(json);                                     // json�ϊ�����������������
        //wr.Flush();
        wr.Close();                                             // �t�@�C������
    }

    // json�t�@�C���ǂݍ���
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // �t�@�C���ǂݍ��ݎw��
        string json = rd.ReadToEnd();                           // �t�@�C�����e�S�ēǂݍ���
        rd.Close();                                             // �t�@�C������
        return JsonUtility.FromJson<SaveData>(json);            // json�t�@�C�����^�ɖ߂��ĕԂ�
    }


    public void SetSaveData(SaveData data)
    {
        GManager.instance.courseDate = data.courseDate;
        GManager.instance.isRerease = data.isRerease;
        GManager.instance.maxLifeNum = data.maxLifeNum;
        GManager.instance.recentCourseNum = data.recentCourseNum;
        GManager.instance.recentStageNum = data.recentStageNum;
    }



    public void ResetData() //������
    {
        Debug.Log("������");
        data.courseDate = new int[6];
        for (int i = 0; i < data.courseDate.Length; i++)
        {
            data.courseDate[i] = 0;
        }

        data.isRerease = new bool[37];
        for (int i = 0; i < data.isRerease.Length; i++)
        {
            data.isRerease[i] = false;
        }

        data.maxLifeNum = 3;
        data.recentCourseNum = 0;
        data.recentStageNum = 0;

        SetSaveData(data);
    }


    // �Q�[���I�����ɕۑ�
    void OnDestroy()
    {
        Save(data);
    }
}
