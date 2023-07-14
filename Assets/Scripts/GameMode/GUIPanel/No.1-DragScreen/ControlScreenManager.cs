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

    [Header("�f�o�b�N���[�h�ؑփ{�^��")]
    public bool isDebugMode;

    [Header("1�������̃X���C�v��(wide=1.0)")]
    public float distancePerMemory;

    public bool isInterval;

    float distanceFactor;
    float dragLength; //( 0.0�`1.0 )
    Vector2 screenFactor;
    Vector2 startPosition;

    int maxGage;
    int gearNum;
    int oldGearNum;

    private int key = 1;//����
    private int oldKey = 1;//�Ō�ɕς��������

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

    public void ChangeControlLimit(bool isRaycast)//, bool isInterval)�󒆎��A���[�v���Aetc...
    {
        crlImage.raycastTarget = isRaycast;
        //this.isInterval = isInterval;
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
        if (gearNum >= 1)
        {
            grypsCrl.Boost(gearNum, key);
            stageCrl.ChangeToRestrictedControl();
            //isInterval = true;
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
                    //�s����ԁ��܂��X�s�[�h�������Ă��Ȃ��A��]�ɂ���ă��������񕜂���
                }
            }
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
            //isInterval = false;
            stageCrl.ChangeToControl();
            speedPowerMg.GetReadyCharge(key);
            cinemachineCrl.ChangeDirection(key);
            grypsCrl.Turn(key);
            oldKey = key;//�X�V
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

            //Debug.Log("��������:" + gearNum + "���Ԓl:" + medianValue);

            if (oldGearNum != gearNum)//���������ς�����������A�������\���̏������s�Ȃ��Ă��炤
            {
                speedPowerMg.DisplaySpeedArrow(gearNum);
                memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage - gearNum);
                oldGearNum = gearNum;
            }

        }

    }

}
