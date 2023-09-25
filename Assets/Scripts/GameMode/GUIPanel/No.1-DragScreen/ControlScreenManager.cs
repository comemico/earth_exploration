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
    public int maxGage = 3;

    [Header("1メモリのスワイプ量(wide=1.0)")]
    public float distancePerMemory;


    float distanceFactor;
    float dragLength; //( 0.0～1.0 )
    Vector2 screenFactor;
    Vector2 startPosition, currentPosition;

    public int gearNum;
    int oldGearNum;

    private int key = 1;//向き
    private int oldKey = 1;//最後に変わった向き

    private void Start()
    {
        GetComponent();
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);
        distanceFactor = 1f / distancePerMemory;
        ConsumeMemory(0);
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

    public void ChangeControlLimit(StageCtrl.ControlStatus status)//, bool isInterval)空中時、ワープ中、etc...
    {
        crlImage.raycastTarget = (status != StageCtrl.ControlStatus.unControl);

        startPosition.x = currentPosition.x;

        if (status == StageCtrl.ControlStatus.unControl)//Uncontrol時に切替時
        {
            gearNum = 0;
            speedPowerMg.Release(gearNum);
        }
    }

    public void ConsumeMemory(int consumeNum)
    {
        memoryGageMg.memoryGage -= consumeNum;

        if (memoryGageMg.memoryGage < maxGage)
        {
            maxGage = memoryGageMg.memoryGage;
        }
        /*
        else
        {
            maxGage = 5;
        }
          */

        if (memoryGageMg.memoryGage <= 0)
        {
            memoryGageMg.memoryGage = 0;
            //grypsCrl.rb.velocity = new Vector2(3f, 0f);
            //stageCtrl.Lack();
            //不足状態→まだスピードが落ちていない、回転によってメモリが回復する
        }
    }

    public void ProduceMemory(int produceNum)
    {
        memoryGageMg.memoryGage += produceNum;

        if (memoryGageMg.memoryGage < maxGage)
        {
            maxGage = memoryGageMg.memoryGage;
        }
        /*
        else
        {
            maxGage = 5;
        }
         */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position * screenFactor;
        movingMaskMg.FadeInMovingMask(startPosition);
        speedPowerMg.StartDrawBow(key);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        movingMaskMg.FadeOutMovingMask();
        speedPowerMg.Release(gearNum);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (stageCrl.controlStatus == StageCtrl.ControlStatus.control)
        {
            if (gearNum >= 1)
            {
                grypsCrl.ForceBoost(gearNum);
                stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.restrictedControl);
                if (!isDebugMode) ConsumeMemory(gearNum);
            }
        }
        gearNum = 0;
        memoryGageMg.DownStatus(gearNum);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPosition = eventData.position * screenFactor;
        movingMaskMg.OnDragMovingMask(currentPosition);

        if (stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)
        {
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
                stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
                grypsCrl.Turn(key, true);
                KeyChange(key);
            }
        }

        if (stageCrl.controlStatus == StageCtrl.ControlStatus.control)
        {
            dragLength = Mathf.Abs(startPosition.x - currentPosition.x);

            float power = Mathf.Clamp(dragLength * distanceFactor, 0, maxGage);

            gearNum = (int)power;

            float medianValue = power - gearNum;

            speedPowerMg.DrawingBow(gearNum, medianValue);

            if (oldGearNum != gearNum)//メモリが変わった時だけ、メモリ表示の処理を行なってもらう
            {
                speedPowerMg.DisplaySpeedArrow(gearNum);
                memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage - gearNum);
                memoryGageMg.DownStatus(gearNum);
                grypsCrl.wheelMg.Judge(gearNum);
                oldGearNum = gearNum;
            }
        }

    }

    public void KeyChange(int key)
    {
        this.key = key;
        speedPowerMg.StartDrawBow(key);
        cinemachineCrl.ChangeDirection(key);
        stageCrl.saltoMg.transform.localScale = new Vector3(key, 1f, 1f);
        oldKey = key;//更新
    }


}
