using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachineManager : MonoBehaviour
{
    [Header("ScreenRange : 進行方向へカメラをずらす設定")]
    [Range(0.1f, 0.5f)] public float range = 0.15f;
    [Range(0.1f, 0.5f)] public float turnTime = 0.5f;
    public Ease turnType = Ease.OutQuad;
    Tween tween_turn;

    [Header("ScreenZoom : カメラのズーム設定")]
    [Range(0.25f, 1.0f)] public float zoomTime = 0.35f;
    public Ease zoomType = Ease.Linear;
    [Space(10)]

    [Range(1f, 3f)] public float returnTime = 1.25f;
    public Ease returnType = Ease.Linear;
    public int defaultSize = 10;
    Tween tween_size;

    CinemachineVirtualCamera cinemachineVirtualCamera; //m_Lens.～ の変更に使用する
    CinemachineFramingTransposer framingTransposer; //Screen Xの変更に使用する


    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void Start()
    {

        //framingTransposer.m_ScreenX = 0.5f - (-1 * range);
        //  brain = GetComponent<CinemachineBrain>();
        //  vcamFloor = GetComponentsInChildren<CinemachineVirtualCamera>();
        //  framingTransposer = vcamFloor[1].GetCinemachineComponent<CinemachineFramingTransposer>();
        //  activeVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    /*
    public void AttachFramingTransposer(int floorNum)//Timelineで開始カメラがLive状態の時に発火させる
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
     */

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
            //Debug.Log("activeVC変更");
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

}
