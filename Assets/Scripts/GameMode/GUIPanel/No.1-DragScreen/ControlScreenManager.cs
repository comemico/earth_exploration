using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;
using SoundSystem;
using UnityEngine.SceneManagement;

public class ControlScreenManager : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    MovingMaskManager movingMaskMg;
    SpeedPowerManager speedPowerMg;
    MemoryGageManager memoryGageMg;
    Image crlImage;

    StageCtrl stageCrl;
    GrypsController grypsCrl;
    CinemachineController cinemachineCrl;

    [Header("デバックモード切替ボタン")]
    public bool isDebugMode;

    [Header("1メモリのスワイプ量(wide=1.0)")]
    public float distancePerMemory;


    float distanceFactor;
    float dragLength; //( 0.0～1.0 )
    Vector2 screenFactor;
    Vector2 startPosition;

    int maxGage;
    public int gearNum;
    int oldGearNum;

    private int key = 1;//向き
    private int oldKey = 1;//最後に変わった向き

    private void Start()
    {
        GetComponent();
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);
        distanceFactor = 1f / distancePerMemory;


    }

    private void GetComponent()
    {
        stageCrl = transform.root.GetComponent<StageCtrl>();
        grypsCrl = transform.root.GetComponent<StageCtrl>().grypsCrl;
        cinemachineCrl = Camera.main.GetComponent<CinemachineController>();
        movingMaskMg = transform.parent.GetComponentInChildren<MovingMaskManager>();
        speedPowerMg = transform.parent.GetComponentInChildren<SpeedPowerManager>();
        memoryGageMg = transform.root.GetComponentInChildren<MemoryGageManager>();
        crlImage = GetComponent<Image>();
    }

    public void ChangeControlLimit(bool isRaycast)//, bool isInterval)空中時、ワープ中、etc...
    {
        crlImage.raycastTarget = isRaycast;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position * screenFactor;

        movingMaskMg.FadeInMovingMask(startPosition);
        speedPowerMg.GetReadyCharge(key);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        movingMaskMg.FadeOutMovingMask();
        speedPowerMg.Release();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (stageCrl.controlStatus == StageCtrl.ControlStatus.control)
        {

            if (gearNum >= 1)
            {
                grypsCrl.Boost(gearNum, key);
                stageCrl.ChangeToRestrictedControl();
                gearNum = 0;
                if (isDebugMode)
                {
                    return;
                }
                else
                {
                    memoryGageMg.memoryGage -= gearNum;
                    if (memoryGageMg.memoryGage <= 0)
                    {
                        memoryGageMg.memoryGage = 0;
                        //grypsCrl.rb.velocity = new Vector2(3f, 0f);
                        //stageCtrl.Lack();
                        //不足状態→まだスピードが落ちていない、回転によってメモリが回復する
                    }
                }
            }
        }
        else
        {
            gearNum = 0;
            return;
        }


    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition = eventData.position * screenFactor;

        movingMaskMg.OnDragMovingMask(currentPosition);

        if (currentPosition.x + 0.005f < startPosition.x)
        {
            key = 1;
        }
        else if (currentPosition.x - 0.005f > startPosition.x)
        {
            key = -1;
        }

        if (oldKey != key)
        {
            stageCrl.ChangeToControl();
            grypsCrl.Turn(key, true);
            KeyChange(key);

        }


        if (memoryGageMg.memoryGage <= 4)
        {
            maxGage = memoryGageMg.memoryGage;
        }
        else
        {
            maxGage = 5;
        }


        if (stageCrl.controlStatus == StageCtrl.ControlStatus.control)
        {
            dragLength = Mathf.Abs(startPosition.x - currentPosition.x);

            float power = Mathf.Clamp(dragLength * distanceFactor, 0, maxGage);

            gearNum = (int)power;

            float medianValue = power - gearNum;

            speedPowerMg.ChargeGear(gearNum, medianValue);


            if (oldGearNum != gearNum)//メモリが変わった時だけ、メモリ表示の処理を行なってもらう
            {
                speedPowerMg.DisplaySpeedArrow(gearNum);
                memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage - gearNum);
                oldGearNum = gearNum;
            }
        }

    }

    public void KeyChange(int key)
    {
        this.key = key;
        speedPowerMg.GetReadyCharge(key);
        cinemachineCrl.ChangeDirection(key);
        oldKey = key;//更新
    }

}
