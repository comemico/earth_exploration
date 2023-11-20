using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Range(1, 40)] public int maxLifeNum;
    public int recentCourseNum;
    public int recentStageNum;
    public int[] courseDate;
    public bool[] isRerease;
}
