using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class CinemachineController : MonoBehaviour
{
    [Header("VirtualCamera�z��")] public CinemachineVirtualCamera[] vcamFloor;//�ҏW�s��
    [Header("�^�[�Q�b�g")] public GrypsController player;
    [Header("ScreenX:��������̋���")] public float screenX;
    [Header("Screen:�J�ڎ���")] public float time = 1.0f;
    [Header("�C�[�W���O�̎��")] public Ease easeType;
    [Header("fieldOfViewBox")] public int[] fieldOfViewBox;
    //[Header("field of view : �f�o�b�N�p")] public Text fieldOfView;

    private CinemachineBrain brain;
    private CinemachineFramingTransposer framingTransposer;
    [HideInInspector] public bool isRock = true;
    CinemachineVirtualCamera activeVC;
    CinemachineVirtualCamera nearestVC;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        vcamFloor = GetComponentsInChildren<CinemachineVirtualCamera>();
        framingTransposer = vcamFloor[1].GetCinemachineComponent<CinemachineFramingTransposer>();
        //activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    public void ActivatedEvent()
    {
        if (!isRock)
        {
            DOVirtual.DelayedCall(0.65f, () =>
            {
                player.ExitWarp(1);
            }, false).OnComplete(() =>
             {
                 player.isFreeze = false;
             });
        }
    }
    /*
    void FixedUpdate()
    {
        fieldOfView.text = "field_of_view : " + (int)brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        //�����ׂ��傫���̂Ŏ������͏���
    }
    */

    public void AttachFramingTransposer(int floorNum)//Timeline�ŊJ�n�J������Live��Ԃ̎��ɔ��΂�����
    {
        framingTransposer = null;
        framingTransposer = vcamFloor[floorNum].GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ToFloorVcam(int floorNum, Transform destination)
    {
        AttachFramingTransposer(floorNum);
        framingTransposer.m_ScreenX = 0.5f - ((int)destination.localScale.x * screenX);

        brain.ActiveVirtualCamera.Follow = null;
        brain.ActiveVirtualCamera.Priority = 0;
        vcamFloor[floorNum].Follow = player.transform;
        vcamFloor[floorNum].Priority = 10;
    }

    public void ChangeDirection(int key)
    {
        DOTween.To(() => framingTransposer.m_ScreenX,
            x => framingTransposer.m_ScreenX = x,
            0.5f - (key * screenX), time)
            .SetEase(easeType);
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
        fieldOfViewBox[boxNum], time)
       .SetEase(type);
    }

}
