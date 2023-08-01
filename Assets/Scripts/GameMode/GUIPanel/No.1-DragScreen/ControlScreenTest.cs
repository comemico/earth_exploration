using UnityEngine;
using UnityEngine.EventSystems;

public class ControlScreenTest : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    MovingMaskManager movingMaskMg;
    SpeedPowerManager speedPowerMg;


    [Header("1メモリのスワイプ量(wide=1.0)")]
    public float distancePerMemory;


    float distanceFactor;
    float dragLength; //( 0.0～1.0 )
    Vector2 screenFactor;
    Vector2 startPosition, currentPosition;

    int maxGage = 5;
    public int gearNum;
    int oldGearNum;

    private int key = 1;//向き
    private int oldKey = 1;//最後に変わった向き


    private void Awake()
    {
        GetComponent();
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);
        distanceFactor = 1f / distancePerMemory;
    }

    private void GetComponent()
    {
        movingMaskMg = transform.parent.GetComponentInChildren<MovingMaskManager>();
        speedPowerMg = transform.parent.GetComponentInChildren<SpeedPowerManager>();
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
        gearNum = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPosition = eventData.position * screenFactor;
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
            KeyChange(key);
        }


        dragLength = Mathf.Abs(startPosition.x - currentPosition.x);

        float power = Mathf.Clamp(dragLength * distanceFactor, 0, maxGage);

        gearNum = (int)power;

        float medianValue = power - gearNum;

        speedPowerMg.ChargeGear(gearNum, medianValue);

        if (oldGearNum != gearNum)//メモリが変わった時だけ、メモリ表示の処理を行なってもらう
        {
            speedPowerMg.DisplaySpeedArrow(gearNum);
            oldGearNum = gearNum;
        }
    }
    public void KeyChange(int key)
    {
        this.key = key;
        speedPowerMg.GetReadyCharge(key);
        oldKey = key;//更新
    }

}
