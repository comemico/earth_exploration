using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private PlayableDirector playableDirector;

    [Header(" �e�[�v_�V�[�N�G���X")] [SerializeField] PlayableAsset[] sequence;

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
    //���삷��^�C�����C���̏Z��
    [Header(" �^�C�����C��")] [SerializeField] PlayableDirector[] playableDirector;

    //�^�C�����C���ōĐ�����e�[�v�̂悤�Ȃ���
    [Header(" �^�C�����C��_Player")] [SerializeField] PlayableAsset[] player;
    [Header(" �^�C�����C��_Object")] [SerializeField] PlayableAsset[] ob;
     


    /// <summary>
    /// ���ꂼ��̃^�C�����C���̃e�[�v����������
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
    /// �^�C�����C�����w�肵�čĐ�/��~/�ꎞ��~
    /// </summary>
    /// <param name="timelineNum"></param>
    //�Đ�����
    public void PlayTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Play();
    }
    //�ꎞ��~����
    public void PauseTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Pause();
    }
    //�ꎞ��~���ĊJ����
    public void ResumeTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Resume();
    }
    //��~����
    public void StopTimeline(int timelineNum)
    {
        playableDirector[timelineNum].Stop();
    }

    */
}
