using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachineManager : MonoBehaviour
{
    [Header("ScreenRange : 進行方向へカメラをずらす設定")]
    [Range(0.1f, 0.5f)] public float range = 0.15f;
    [Range(0.1f, 0.5f)] public float turnTime = 0.5f;
    public Ease turnType = Ease.OutQuad;


    Tween t_lensSize;
    Tween t_timeScale;

    [HideInInspector] public CinemachineVirtualCamera cinemachineVirtualCamera; //m_Lens.〜 の変更に使用する.
    CinemachineFramingTransposer framingTransposer; //Screen Xの変更に使用する.


    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ChangeDirection(int key)
    {
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, 0.5f - (key * range), turnTime).SetEase(turnType);
    }

    public void Foucus(Transform target)
    {
        framingTransposer.m_XDamping = 3;
        framingTransposer.m_YDamping = 3;
        cinemachineVirtualCamera.Follow = target;
        DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, 0.5f, 1.5f).SetEase(Ease.OutSine);
        DOTween.To(() => framingTransposer.m_ScreenY, x => framingTransposer.m_ScreenY = x, 0.5f, 1.5f).SetEase(Ease.OutSine);
    }

    public Tween DOLensSize(float endValue, float duration, Ease ease)
    {
        t_lensSize = DOTween.To(() => cinemachineVirtualCamera.m_Lens.OrthographicSize, x => cinemachineVirtualCamera.m_Lens.OrthographicSize = x, endValue, duration).SetEase(ease);
        return t_lensSize;
    }


    public void DOTimeScale(float endValue, float duration, Ease ease)
    {
        //t_timeScale.Kill(true); ジェットのHud挙動がおかしくなるため、完了できない（ゲージがすぐ満タンになり、溜めることができなくなる）.
        t_timeScale = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endValue, duration).SetEase(ease);
    }

    public void DOPauseTimeScale()
    {
        t_timeScale.Pause();
    }
    public void DOPlayTimeScale()
    {
        t_timeScale.Pause();
    }

}


/*
public void StartDirection(float screenX)
{
    DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, screenX, 1.5f).SetEase(Ease.InOutSine);
}

public void ClearDirection(float screenX)
{
    DOTween.To(() => framingTransposer.m_ScreenX, x => framingTransposer.m_ScreenX = x, screenX, 1.5f).SetEase(Ease.InOutSine);
    DOTween.To(() => framingTransposer.m_DeadZoneHeight, x => framingTransposer.m_DeadZoneHeight = x, 0f, 1.5f).SetEase(Ease.OutSine);
}
 */