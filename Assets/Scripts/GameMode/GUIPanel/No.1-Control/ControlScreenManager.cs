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

    [Header("�f�o�b�N���[�h�ؑփ{�^��")]
    public bool isDebugMode;
    public int maxGage; //�c�胁�������ɉ����ď�����ω�����(3�`1)

    [Header("1�������̃X���C�v��(wide=1.0)")]
    public float distancePerMemory;


    float distanceFactor;
    float dragLength; //( 0.0�`1.0 )
    Vector2 screenFactor;
    Vector2 startPosition, currentPosition;

    public int gearNum;
    int oldGearNum;

    private int key = 1;//����
    private int oldKey = 1;//�Ō�ɕς��������

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

    public void ChangeControlLimit(StageCtrl.ControlStatus status)//, bool isInterval)�󒆎��A���[�v���Aetc...
    {
        crlImage.raycastTarget = (status != StageCtrl.ControlStatus.unControl);

        startPosition.x = currentPosition.x;

        if (status == StageCtrl.ControlStatus.unControl)//Uncontrol���ɐؑ֎�
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

        //�������؂ꃂ�[�h�N��
        if (memoryGageMg.memoryGage <= 0)
        {
            memoryGageMg.memoryGage = 0;
            //grypsCrl.rb.velocity = new Vector2(3f, 0f);
            stageCrl.Lack();
            //�s����ԁ��܂��X�s�[�h�������Ă��Ȃ��A�T���g�ɂ���ă��������񕜂���
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
        //�������؂ꃂ�[�h����̕��A
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

            if (oldGearNum != gearNum)//���������ς�����������A�������\���̏������s�Ȃ��Ă��炤
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
        oldKey = key;//�X�V
    }


}
