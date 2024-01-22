using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachineManager : MonoBehaviour
{
    [Header("ScreenRange : �i�s�����փJ���������炷�ݒ�")]
    [Range(0.1f, 0.5f)] public float range = 0.15f;
    [Range(0.1f, 0.5f)] public float turnTime = 0.5f;
    public Ease turnType = Ease.OutQuad;


    /*
    [Header("ScreenZoom : �J�����̃Y�[���ݒ�")]
    [Range(0.25f, 1.0f)] public float zoomTime = 0.35f;
    public Ease zoomType = Ease.Linear;
    [Space(10)]

    [Range(1f, 3f)] public float returnTime = 1.25f;
    public Ease returnType = Ease.Linear;
    public int defaultSize = 10;
     */

    Tween t_LensSize;
    Tween t_timeScale;

    CinemachineVirtualCamera cinemachineVirtualCamera; //m_Lens.�` �̕ύX�Ɏg�p����
    CinemachineFramingTransposer framingTransposer; //Screen X�̕ύX�Ɏg�p����


    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ChangeDirection(int key)
    {
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, 0.5f - (key * range), turnTime).SetEase(turnType);
    }

    public void StartDirection(float screenX)
    {
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, screenX, 1.5f).SetEase(Ease.InOutSine);
    }

    public void ClearDirection(float screenX)
    {
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, screenX, 1.5f).SetEase(Ease.InOutSine);
        DOTween.To(() => framingTransposer.m_DeadZoneHeight, x => framingTransposer.m_DeadZoneHeight = x, 0f, 1.5f).SetEase(Ease.OutSine);
    }


    public void DOLensSize(float endValue, float duration, Ease ease)
    {
        t_LensSize = DOTween.To(() => cinemachineVirtualCamera.m_Lens.OrthographicSize, x => cinemachineVirtualCamera.m_Lens.OrthographicSize = x, endValue, duration).SetEase(ease);
    }


    public void DOTimeScale(float endValue, float duration, Ease ease)
    {
        t_timeScale = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endValue, duration).SetEase(ease);
    }


}
