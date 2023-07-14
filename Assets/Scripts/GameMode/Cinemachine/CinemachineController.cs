using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;

public class CinemachineController : MonoBehaviour
{
    [Header("VirtualCamera配列")] public CinemachineVirtualCamera[] vcamFloor;//編集不可
    [Header("ターゲット")] public GrypsController player;
    [Header("ScreenX:中央からの距離")] public float screenX;
    [Header("Screen:遷移時間")] public float time = 1.0f;
    [Header("イージングの種類")] public Ease easeType;
    [Header("fieldOfViewBox")] public int[] fieldOfViewBox;
    //[Header("field of view : デバック用")] public Text fieldOfView;

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
        //↑負荷が大きいので実装時は消す
    }
    */

    public void AttachFramingTransposer(int floorNum)//Timelineで開始カメラがLive状態の時に発火させる
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
            //Debug.Log("activeVC変更");
            nearestVC = activeVC;
        }

        DOTween.To(() => activeVC.m_Lens.FieldOfView,
       x => activeVC.m_Lens.FieldOfView = x,
        fieldOfViewBox[boxNum], time)
       .SetEase(type);
    }

}
