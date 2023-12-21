using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlScreenManager : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    MovingMaskManager movingMaskMg;
    [HideInInspector] public TutorialManager tutorialMg;
    [HideInInspector] public BowManager bowMg;
    MemoryGageManager memoryGageMg;
    Image crlImage;

    StageCtrl stageCrl;
    GrypsController grypsCrl;
    CinemachineManager cinemachineMg;

    [Header("デバックモード切替ボタン")]
    public bool isDebugMode;
    public int maxGage; //残りメモリ数に応じて上限が変化する(3～1)

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
        cinemachineMg = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
        movingMaskMg = transform.parent.GetComponentInChildren<MovingMaskManager>();
        tutorialMg = transform.parent.GetComponentInChildren<TutorialManager>();
        bowMg = transform.parent.GetComponentInChildren<BowManager>();
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
            bowMg.Release();
        }
    }

    public void ConsumeMemory(int consumeNum)
    {
        memoryGageMg.memoryGage -= consumeNum;

        if (memoryGageMg.memoryGage < maxGage)
        {
            maxGage = memoryGageMg.memoryGage;
        }

        //メモリ切れモード起動
        if (memoryGageMg.memoryGage <= 0)
        {
            memoryGageMg.memoryGage = 0;
            //grypsCrl.rb.velocity = new Vector2(3f, 0f);
            stageCrl.Lack();
            //不足状態→まだスピードが落ちていない、サルトによってメモリが回復する
        }
    }

    public void ProduceMemory(int produceNum)
    {
        if (memoryGageMg.memoryGage >= stageCrl.data.maxLifeNum) return;
        memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage + produceNum);
        memoryGageMg.memoryGage += produceNum;
        memoryGageMg.memoryCountMg.ProduceLamp();

        if (memoryGageMg.memoryGage > maxGage)
        {
            maxGage = Mathf.Clamp(memoryGageMg.memoryGage, 0, 3);
        }
        //メモリ切れモードからの復帰
        stageCrl.Regeneration();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position * screenFactor;
        bowMg.StartDrawBow(key);
        //movingMaskMg.FadeInMovingMask(startPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bowMg.Release();
        //movingMaskMg.FadeOutMovingMask();
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPosition = eventData.position * screenFactor;
        //movingMaskMg.OnDragMovingMask(currentPosition);

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

            bowMg.DrawingBow(gearNum, medianValue);

            if (oldGearNum != gearNum)//メモリが変わった時だけ、メモリ表示の処理を行なってもらう
            {
                bowMg.DisplaySpeedArrow(gearNum);
                memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage - gearNum);
                memoryGageMg.memoryCountMg.ConsumeLamp();
                grypsCrl.wheelMg.Judge(gearNum);
                oldGearNum = gearNum;
            }
        }

    }

    public void KeyChange(int key)
    {
        this.key = key;
        bowMg.StartDrawBow(key);
        cinemachineMg.ChangeDirection(key);
        stageCrl.saltoMg.transform.localScale = new Vector3(key, 1f, 1f);
        oldKey = key;//更新
    }


}
