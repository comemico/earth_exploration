using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class LaunchCameraController : MonoBehaviour
{
    [Header("VirtualCamera�z��")] public CinemachineVirtualCamera[] vcamFloor;
    //[Header("�^�[�Q�b�g")] public GrypsController gryps;
    /*
    [Header("DoTween: ScreenX")]
    [Header("ScreenX:��������̋���")] public float screenX;
    [Header("�C�[�W���O�̎��")] public Ease easeType;
    [Header("Screen:�J�ڎ���")] public float time = 1.0f;
     */

    //[Header("fieldOfViewBox")] public int[] fieldOfViewBox;

    [HideInInspector] public CinemachineBrain brain;
    CinemachineFramingTransposer framingTransposer;

    CinemachineVirtualCamera activeVC;
    CinemachineVirtualCamera nearestVC;


    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        vcamFloor = GetComponentsInChildren<CinemachineVirtualCamera>();
    }

    public void AttachFramingTransposer(int floorNum)//Timeline�ŊJ�n�J������Live��Ԃ̎��ɔ��΂�����
    {
        framingTransposer = null;
        framingTransposer = vcamFloor[floorNum].GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ToFloorVcam(int floorNum)
    {
        // AttachFramingTransposer(floorNum);
        //framingTransposer.m_ScreenX = 0.5f - (destinationKey * screenX);
        for (int i = 0; i < vcamFloor.Length; i++)
        {
            vcamFloor[i].Priority = 0;
            //vcamFloor[i].Follow = null;
        }
        vcamFloor[floorNum].Priority = 10;
        // vcamFloor[floorNum].Follow = gryps.transform;
    }

    /*
    public void ChangeDirection(int key)
    {
        DOTween.To(() => framingTransposer.m_ScreenX,
            x => framingTransposer.m_ScreenX = x,
            0.5f - (key * screenX), time)
            .SetEase(easeType);//.SetUpdate(false);
    }

    public void ZoomCamera(int boxNum = 0, float time = 1.0f, Ease type = 0)
    {
        activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (nearestVC != activeVC)
        {
            //Debug.Log("activeVC�ύX");
            nearestVC = activeVC;
        }

        DOTween.To(() => activeVC.m_Lens.FieldOfView,
       x => activeVC.m_Lens.FieldOfView = x,
        fieldOfViewBox[boxNum], time).SetEase(type);
    }
     */

}
