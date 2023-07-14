using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private PlayableDirector playableDirector;

    [Header(" テープ_シークエンス")] [SerializeField] PlayableAsset[] sequence;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void TapeChangeUI(int tapeNum)
    {
        playableDirector.playableAsset = sequence[tapeNum];
    }
    public void PlayTimeline()
    {
        playableDirector.Play();
    }

    /*
    //操作するタイムラインの住所
    [Header(" タイムライン")] [SerializeField] PlayableDirector[] playableDirector;

    //タイムラインで再生するテープのようなもの
    [Header(" タイムライン_Player")] [SerializeField] PlayableAsset[] player;
    [Header(" タイムライン_Object")] [SerializeField] PlayableAsset[] ob;
     


    /// <summary>
    /// それぞれのタイムラインのテープを交換する
    /// </summary>
    /// <param name="TapeNum"></param>
    public void TapeChangeUI(int TapeNum)
    {
        playableDirector[0].playableAsset = ui[TapeNum];
    }
    public void TapeChangePlayer(int TapeNum)
    {
        playableDirector[1].playableAsset = player[TapeNum];
    }
    public void TapeChangeObject(int TapeNum)
    {
        playableDirector[2].playableAsset = ob[TapeNum];
    }

    /// <summary>
    /// タイムラインを指定して再生/停止/一時停止
    /// </summary>
    /// <param name="timelineNum"></param>
    //再生する
    public void PlayTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Play();
    }
    //一時停止する
    public void PauseTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Pause();
    }
    //一時停止を再開する
    public void ResumeTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Resume();
    }
    //停止する
    public void StopTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Stop();
    }

    */
}
