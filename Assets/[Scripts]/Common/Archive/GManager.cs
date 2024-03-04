using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    //private AudioSource audioSource = null;

    [Header("保存する変数")]

    [Header("エリアごとのの最大到達値")]
    public int[] courseDate;
    [Header("最大ライフ数")]
    [Range(1, 40)]
    public int maxLifeNum;
    [Header("直近の選択エリア番号")]
    public int recentCourseNum;
    [Header("直近の選択ステージ番号")]
    public int recentStageNum;
    [Header("ExceadStage")]
    public bool[] isRerease;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /*
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }
     */

}
