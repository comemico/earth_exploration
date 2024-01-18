using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachineManager : MonoBehaviour
{
    [Header("ScreenRange : �i�s�����փJ���������炷�ݒ�")]
    [Range(0.1f, 0.5f)] public float range = 0.15f;
    [Range(0.1f, 0.5f)] public float turnTime = 0.5f;
    public Ease turnType = Ease.OutQuad;
    Tween tween_turn;

    [Header("ScreenZoom : �J�����̃Y�[���ݒ�")]
    [Range(0.25f, 1.0f)] public float zoomTime = 0.35f;
    public Ease zoomType = Ease.Linear;
    [Space(10)]

    [Range(1f, 3f)] public float returnTime = 1.25f;
    public Ease returnType = Ease.Linear;
    public int defaultSize = 10;
    Tween tween_size;

    CinemachineVirtualCamera cinemachineVirtualCamera; //m_Lens.�` �̕ύX�Ɏg�p����
    CinemachineFramingTransposer framingTransposer; //Screen X�̕ύX�Ɏg�p����


    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ChangeDirection(int key)
    {
        //tween_screenX.Kill(true);
        //tween_turn =
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, 0.5f - (key * range), turnTime).SetEase(turnType);
    }

    public void StartDirection(float screenX)
    {
        //tween_turn =
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, screenX, 1.5f).SetEase(Ease.InOutSine);
    }

    public void ClearDirection(float screenX)
    {
        //tween_turn = 
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, screenX, 1.5f).SetEase(Ease.InOutSine);
        //tween_turn = 
        DOTween.To(() => framingTransposer.m_DeadZoneHeight, x => framingTransposer.m_DeadZoneHeight = x, 0f, 1.5f).SetEase(Ease.OutSine);
    }



    public void Zoom(int size)
    {
        tween_size = DOTween.To(() => cinemachineVirtualCamera.m_Lens.OrthographicSize, x => cinemachineVirtualCamera.m_Lens.OrthographicSize = x, size, zoomTime).SetEase(zoomType);

        //tween_fov = DOTween.To(() => cinemachineVirtualCamera.m_Lens.FieldOfView, x => cinemachineVirtualCamera.m_Lens.FieldOfView = x, fov, fovDuration).SetEase(fovType);
        /*
        //tween_fov.Kill(false);
        //cinemachineVirtualCamera = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (nearestVC != cinemachineVirtualCamera)
        {
            //Debug.Log("activeVC�ύX");
            nearestVC = cinemachineVirtualCamera;
        }
         */
    }

    public void DefaultZoom()
    {
        tween_size = DOTween.To(() => cinemachineVirtualCamera.m_Lens.OrthographicSize, x => cinemachineVirtualCamera.m_Lens.OrthographicSize = x, defaultSize, returnTime).SetEase(returnType);
        //tween_fov = DOTween.To(() => cinemachineVirtualCamera.m_Lens.FieldOfView, x => cinemachineVirtualCamera.m_Lens.FieldOfView = x, defaultFov, fovDeDuration).SetEase(fovDeType);
        //tween_fov.Kill(false);
        //cinemachineVirtualCamera = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    Tween t_timeScale;

    public void DOTimeScale(float endValue, float duration)
    {
        t_timeScale.Kill(true);
        t_timeScale = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endValue, duration).SetEase(Ease.OutQuint);
        //Debug.Log("DOTimeScale()");
    }

}
