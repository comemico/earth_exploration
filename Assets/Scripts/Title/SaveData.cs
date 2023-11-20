using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Range(1, 40)] public int maxLifeNum;
    public int recentCourseAdress;
    public int recentStageAdress;
    public int[] linearData;
    public bool[] scatterData;
}
