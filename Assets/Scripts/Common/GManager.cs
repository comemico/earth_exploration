using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    const float PreHeader = 20f;// ヘッダー前のスペース

    public static GManager instance = null;

    private AudioSource audioSource = null;


    [Space(PreHeader)]
    [Header("保存する変数")]
    [Header("-----------------------------")]
    [Header("エリアの到達値")] public int[] courseDate;
    [Header("最大ライフ数")] public int maxLifeNum;
    [Header("直近の選択エリア番号")] public int recentCourseNum;

    /*
    [Space(PreHeader)]
    [Header("確認用")]
    [Header("-----------------------------")]
    [Header("ステージ番号")] public int stageNum;
    [Header("ステージごとに設定された設定メモリ数")] public int lifeNum;
     */


    /*
    [Header("ステージ記録（ステージセレクト用）")] public string[] stageRecord;
    [Header("ステージ番号（ステージセレクト用）")] public string loadStageNum = "stageNum";
    */

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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //PlayerPrefs.SetFloat("RecordTime", 999.99f);
        // PlayerPrefs.DeleteKey("RecordTime");
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

    /*
    public void AddMemoryGage(int value)
    {
        lifeMemoryGage += value;
        if (lifeMemoryGage > maxLifeMemoryGage)
        {
            lifeMemoryGage = maxLifeMemoryGage;
        }
    }

    public void SubLifeMemoryGage(int value)
    {
        if (lifeMemoryGage > 0)
        {
            lifeMemoryGage -= value;
            if (lifeMemoryGage <= 0)
            {
                lifeMemoryGage = 0;
            }
        }
    }

    public void RetryGame() //ゲームオーバー or ゲームクリア → リトライする際の処理
    {
        isGameOver = false;
        isStageClear = false;
    }

    public void NextStage()
    {
        isGameOver = false;
        isStageClear = false;
    }
    */
}
