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
        GManager.instance.courseDate = data.linearData;
        GManager.instance.isRerease = data.scatterClear;
        GManager.instance.maxLifeNum = data.maxLifeNum;
        GManager.instance.recentCourseNum = data.recentCourseAdress;
        GManager.instance.recentStageNum = data.recentStageAdress;
    }



    public void ResetData() //������
    {
        Debug.Log("������");
        data.maxLifeNum = 3;
        data.recentCourseAdress = 0;
        data.recentStageAdress = 0;

        data.linearData = new int[2]; //�R�[�X�G���A��
        data.scatterDiscover = new bool[2]; //ScatterStage�̐�
        data.scatterClear = new bool[2]; //��L�̐��ʂɑ�����

        for (int i = 0; i < data.linearData.Length; i++) data.linearData[i] = 0;
        for (int i = 0; i < data.scatterDiscover.Length; i++) data.scatterDiscover[i] = false;
        for (int i = 0; i < data.scatterClear.Length; i++) data.scatterClear[i] = false;

        SetSaveData(data);
    }


    // �Q�[���I�����ɕۑ�
    void OnDestroy()
    {
        Save(data);
    }
}
