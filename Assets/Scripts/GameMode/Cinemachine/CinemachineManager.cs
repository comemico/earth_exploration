using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class CinemachineManager : MonoBehaviour
{
    [Header("VirtualCamera�z��")] public CinemachineVirtualCamera[] vcamFloor;//�ҏW�s��
    [Header("�^�[�Q�b�g")] public Transform player;
    [Header("ScreenX:��������̋���")] public float screenX;
    [Header("Screen:�J�ڎ���")] public float time = 1.0f;
    [Header("�C�[�W���O�̎��")] public Ease easeType;
    [Header("field of view")] public Text fieldOfView;
    [Header("fieldOfViewBox")] public int[] fieldOfViewBox;

    private CinemachineBrain brain;
    private CinemachineVirtualCamera vcamTransition;
    private CinemachineFramingTransposer framingTransposer;
    private CinemachineFramingTransposer framingTransposerTransition;
    private int vcamNum;
    private int vcamKey;

    void Start()
    {
        brain = GetComponent<CinemachineBrain>();
        vcamFloor = GetComponentsInChildren<CinemachineVirtualCamera>();
        vcamTransition = transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();

        framingTransposer = vcamFloor[1].GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposerTransition = vcamTransition.GetCinemachineComponent<CinemachineFramingTransposer>();

    }

    void FixedUpdate()
    {
        if (brain.IsLive(vcamTransition))//�J�ڃJ������Live�ɂȂ�����
        {
            if (brain.ActiveBlend.BlendWeight >= 0.55f)
            {
                ToFloorVcam(vcamNum);
                //player.GetComponent<GrypsManager>().ExitWarp(1);
            }
        }

        //fieldOfView.text = "field_of_view : " + (int)brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        //�����ׂ��傫���̂Ŏ������͏���
    }

    public void AttachFramingTransposer(int floorNum)//Timeline�ŊJ�n�J������Live��Ԃ̎��ɔ��΂�����
    {
        framingTransposer = null;
        framingTransposer = vcamFloor[floorNum].GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ToTransitionVcam(int floorNum, Transform destination)
    {
        brain.ActiveVirtualCamera.Follow = null;
        brain.ActiveVirtualCamera.Priority = 0;
        vcamTransition.Follow = player;
        vcamTransition.Priority = 10;
        vcamNum = floorNum;
        vcamKey = (int)destination.localScale.x;//�o���̌���
        framingTransposerTransition.m_ScreenX = 0.5f - (vcamKey * screenX);
    }

    private void ToFloorVcam(int floorNum)
    {
        AttachFramingTransposer(floorNum);
        framingTransposer.m_ScreenX = 0.5f - (vcamKey * screenX);

        brain.ActiveVirtualCamera.Follow = null;
        brain.ActiveVirtualCamera.Priority = 0;
        vcamFloor[floorNum].Follow = player;
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
        CinemachineVirtualCamera activeVC;
        activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        DOTween.To(() => activeVC.m_Lens.FieldOfView,
       x => activeVC.m_Lens.FieldOfView = x,
        fieldOfViewBox[boxNum], time)
       .SetEase(type);
    }


}
