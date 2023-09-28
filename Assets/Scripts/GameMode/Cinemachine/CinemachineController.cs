using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class CinemachineController : MonoBehaviour
{
    [Header("VirtualCamera�z��")] public CinemachineVirtualCamera[] vcamFloor;//�ҏW�s��
    [Header("�^�[�Q�b�g")] public Transform grypsSprite;

    [Header("Direction")]
    [Range(0.1f, 0.5f)] public float range;
    public float turnDuration = 0.5f;
    public Ease turnType;
    Tween tween_turn;

    [Header("FOV")]
    [Range(0.25f, 1.0f)] public float fovDuration;
    public Ease fovType;
    public int defaultFov;
    [Range(1f, 3f)] public float fovDeDuration;
    public Ease fovDeType;

    Tween tween_fov;


    [HideInInspector] public CinemachineBrain brain;
    CinemachineFramingTransposer framingTransposer;
    [HideInInspector] public CinemachineVirtualCamera activeVC;
    CinemachineVirtualCamera nearestVC;


    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        vcamFloor = GetComponentsInChildren<CinemachineVirtualCamera>();
        //framingTransposer = vcamFloor[1].GetCinemachineComponent<CinemachineFramingTransposer>();
        //activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    public void AttachFramingTransposer(int floorNum)//Timeline�ŊJ�n�J������Live��Ԃ̎��ɔ��΂�����
    {
        framingTransposer = null;
        framingTransposer = vcamFloor[floorNum].GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ToFloorVcam(int floorNum, int destinationKey)
    {
        AttachFramingTransposer(floorNum);
        framingTransposer.m_ScreenX = 0.5f - (destinationKey * range);

        for (int i = 0; i < vcamFloor.Length; i++)
        {
            vcamFloor[i].Priority = 0;
            vcamFloor[i].Follow = null;
        }
        vcamFloor[floorNum].Priority = 10;
        vcamFloor[floorNum].Follow = grypsSprite;
    }

    public void ChangeDirection(int key)
    {
        //tween_screenX.Kill(true);
        tween_turn = DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, 0.5f - (key * range), turnDuration).SetEase(turnType);
    }

    public void Zoom(int fov)
    {
        //tween_fov.Kill(false);
        activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (nearestVC != activeVC)
        {
            //Debug.Log("activeVC�ύX");
            nearestVC = activeVC;
        }
        tween_fov = DOTween.To(() => activeVC.m_Lens.FieldOfView, x => activeVC.m_Lens.FieldOfView = x, fov, fovDuration).SetEase(fovType);
    }

    public void DefaultZoom()
    {
        //tween_fov.Kill(false);
        activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        tween_fov = DOTween.To(() => activeVC.m_Lens.FieldOfView, x => activeVC.m_Lens.FieldOfView = x, defaultFov, fovDeDuration).SetEase(fovDeType);
    }

    /*
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

    /*
    public void ActivatedEvent()
    {
        DOVirtual.DelayedCall(1.5f, () =>//�J�����ړ����ҋ@
        {
            player.DashA((int)player.transform.localScale.x, dashPower);
        }, false);
    }
    void FixedUpdate()
    {
        fieldOfView.text = "field_of_view : " + (int)brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        //�����ׂ��傫���̂Ŏ������͏���
    }
    */

}
