using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using SoundSystem;

public class GrypsController : MonoBehaviour
{
    public GrypsParameter grypsParameter;
    public WheelManager wheelMg;

    [Header("�f�o�b�N���[�h�ؑփ{�^��")] public bool isDebugMode;
    [Header("�T�}�[�\���g�I�����p�x(���x�X���邩)")] public float targetAngle;
    [Header("��")] public float forceMagnitude;
    [Header("�p�x")] public float forceAngle;

    //[Header("JetLevel")] public int[] jetLevel;
    //[Header("1������������̃X���C�v����")] public float distancePerMemory;
    //[Header("BoostLevel")] public int[] boostLevel;
    //[Header("DashLevel")] public int[] dashLevel;
    // [Header("�C���^�[�o��")] public float interval;
    //[Header("�G���W����string")] public string[] engine;
    //[Header("�W�F�b�g��string")] public string[] jet;
    //[Header("������͈�")] public float stepOnRate;
    //[Header("�ő��")] public int maxSpeed;

    [Header("stageCtrl")] public StageCtrl stageCrl;
    [Header("SomersaultButton")] public Button somersaultButton;
    [Header("GroundCheck")] GroundCheck ground;
    [Header("Effect")] EffectManager effectManager;
    [Header("Cinemachine")] CinemachineController cinemachineCtrl;
    [Header("MemoryGage")] public MemoryGageManager memoryGageMg;
    [Header("JetMemory")] public JetManager jetMemoryMg;

    //private Vector2 startPos;
    private Vector3 forceDirection = new Vector3(1f, 1f, 1f);
    public Rigidbody2D rb = null;
    private Animator anim = null;
    public bool isRevup;
    public bool isBrake;
    private bool isForce = false;
    public bool isFreeze = false;
    private bool isGroundPrev = false;
    private bool isGround = false;
    private bool passedIsJet = false;
    private bool isJet = false;
    private string deadAreaTag = "DeadArea";
    private string clearAreaTag = "ClearArea";

    /*
    private bool isInterval;
    private int key = 1;//����
    private int oldKey;//�Ō�ɕς��������
    private int jetcount = 0;
    private int oldGearNum = 0;
    private int gearNum;
    private float intervalTimer = 0.0f;
    private float factorDistance;
     */

    private int somersaultCount = 0;
    private int jetMemory = 0;
    private int oldJetMemory = 0;


    private Quaternion prev;
    private float myAngle;
    private int resultAngle;
    private Vector2 targetVector;
    private Vector2 nowVector;
    public bool isSomersault = false;

    #region//�v���C�x�[�g�ϐ�
    #endregion

    private void Awake()
    {
        AllGetComponent();
    }

    void Start()
    {
        targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//�^�[�Q�b�g�̊p�x���x�N�^�[
        //factorDistance = 1 / distancePerMemory;
        //CalcForceDirection();
    }

    void CalcForceDirection()
    {
        float rad = forceAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad);
        float y = Mathf.Sin(rad);
        float z = 0f;
        forceDirection = new Vector3(x, y, z);
    }

    void AllGetComponent()
    {
        cinemachineCtrl = Camera.main.GetComponent<CinemachineController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        effectManager = GetComponent<EffectManager>();
        ground = GetComponentInChildren<GroundCheck>();
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//���n����

        if (isGround != isGroundPrev)
        {
            if (isGround == true)
            {
                Land();
                isGroundPrev = true;
            }
            else if (isGround == false)
            {
                TakeOff();
                isGroundPrev = false;
            }
        }

        /*
        var tes = rb.velocity.normalized;
        float ang = Vector2.Angle(rb.velocity, Vector2.right);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, Quaternion.Lerp(transform.localRotation, ang, 0.5f));
         */
        /*
        if (isForce)
        {
            if (gearNum > 1)
            {
                effectManager.JetEffect();
            }
            rb.AddForce(key * transform.right * boostLevel[ConsumptionMemory()]);
            isForce = false;
        }
         */

        if (isBrake)
        {
            rb.velocity *= grypsParameter.breakPower;
            if (Mathf.Abs(rb.velocity.x) <= 0.25f)
            {
                Brake(false);//velocity�� 0.25f�ȉ��ŉ���
            }
        }

        if (isSomersault)
        {
            Vector2 prevvec = prev * Vector2.up;
            Vector2 nowvec = transform.rotation * Vector2.up;
            float angle = Vector2.Angle(prevvec, nowvec);
            myAngle += angle;
            prev = transform.rotation;

            if (myAngle >= resultAngle - 20f)
            {
                //Debug.Log("������");
                rb.angularVelocity *= 0.75f;

                if (myAngle >= resultAngle)
                {
                    somersaultCount++;
                    //memoryGageMg.getMemoryMg.DisplaySomerSaultCount(somersaultCount);
                    //memoryGageMg.DotweenBlueGage(memoryGageMg.memoryGage, memoryGageMg.memoryGage + 1, 0.3f);
                    memoryGageMg.memoryGage++;
                    memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage);
                    myAngle = 0f;
                    rb.angularVelocity = 0f;
                    if (transform.localScale.x == 1)//�E�����Ă���
                    {
                        transform.localEulerAngles = new Vector3(0f, 0f, (360 - targetAngle));//340 : 
                    }
                    else if (transform.localScale.x == -1)//�������Ă���
                    {
                        transform.localEulerAngles = new Vector3(0f, 0f, (360 + targetAngle));//380 : 360����ǂꂾ������Ă���̂��ōl���Ă݂�
                    }
                    isSomersault = false;
                }
            }
        }


    }

    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f))//timescale=0�Ȃ�return
        {
            return;
        }
        if (!isFreeze)
        {
            /*
            if (Input.GetMouseButtonDown(0))
            {
                RevUp();
            }

            if (isGround && !isInterval)//���[�^�[�ύX

            {
                ConsumptionMemory();
            }

            if (Input.GetMouseButtonUp(0) && isGround & !isInterval)
            {
                RevDown();
            }
            if (isGround)//�����ύX
            {
                CheckDirection();
            }
            */
        }
        /*
        if (isInterval)
        {
            if (intervalTimer >= interval)
            {
                isInterval = false;
                intervalTimer = 0.0f;
            }
            else
            {
                intervalTimer += Time.deltaTime;
            }
        }
         */

    }

    /*
    //���������̏���
    private void RevUp()
    {
        startPos = Input.mousePosition;
        if (isGround && !isInterval)
        {
            isRevup = true;
            //speedArrow.CompleteSequence(key);
            //anim.SetFloat("boost", 0.5f);
            //SoundController.instance.PlayEngineSe(engine[0]);
        }
    }
    //���������Ă��鎞�̏���
    private void CheckDirection()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x + 25.0f < startPos.x)
            {
                key = 1;
            }
            if (Input.mousePosition.x - 25.0f > startPos.x)
            {
                key = -1;
            }
        }

        if (oldKey != key)
        {
            if (key == 1)
            {
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            }
            else if (key == -1)
            {
                transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            }
            isBrake = true;
            isInterval = false;
            //speedArrow.CompleteSequence(key);
            cinemachineCtrl.ChangeDirection(key);
            oldKey = key;//�X�V
        }
    }



    //���������Ă��鎞�̏���
    public int ConsumptionMemory()
    {
        if (isFreeze)
        {
            return 0;
        }

        int maxGage;
        int medianValue;

        if (Input.GetMouseButton(0))//�c��̃������ɉ�����maxGage��ύX����
        {
            if (memoryGageMg.memoryGage <= 4)
            {
                maxGage = memoryGageMg.memoryGage;
            }
            else
            {
                maxGage = 5;
            }

            int swipeLength = (int)(Mathf.Abs(this.startPos.x - Input.mousePosition.x));//������Βl�̋���

            swipeLength = Mathf.Clamp(swipeLength, 0, (maxGage * (int)distancePerMemory + 1));

            gearNum = (int)(factorDistance * swipeLength);

            gearNum = Mathf.Clamp(gearNum, 0, maxGage);

            medianValue = swipeLength - (gearNum * (int)distancePerMemory);//100�̂Ƃ����distancePerMemory�ł���Ă��悢����


            //speedArrow.ChargeGear(gearNum, factorDistance, medianValue);
            //speedArrow.ChargeGear(gearNum, medianValue);

            if (oldGearNum != gearNum)//���������ς�����������A�������\���̏������s�Ȃ��Ă��炤
            {
                memoryGageMg.DisplayMemoryGage(memoryGageMg.memoryGage - gearNum);
                //speedArrow.DisplaySpeedArrow(gearNum);
                //animator.set.int(gearNum);
                //anim.SetBool("revup", isRevUp);
                anim.SetFloat("boost", gearNum);
                oldGearNum = gearNum;
            }

        }
        return gearNum;
    }
    //���������̏���
    private void RevDown()
    {
        isForce = true;
        isBrake = false;
        isRevup = false;
        isInterval = true;
        effectManager.Brake(false);
        speedArrow.Release();
        SoundController.instance.StopEngine();
        anim.SetFloat("boost", 0);
        anim.SetTrigger("boostRelease");

        if (isDebugMode)
        {
            return;
        }
        else
        {
            memoryGageMg.memoryGage -= ConsumptionMemory();
            if (memoryGageMg.memoryGage <= 0)
            {
                memoryGageMg.memoryGage = 0;
                rb.velocity = new Vector2(3f, 0f);
                stageCtrl.Lack();
                //�s����ԁ��܂��X�s�[�h�������Ă��Ȃ��A��]�ɂ���ă��������񕜂���
            }
        }
    }
     */
    public void Turn(int key, bool isBrake)
    {
        transform.localScale = new Vector3(key, transform.localScale.y, transform.localScale.z);
        if (isBrake)
        {
            Brake(true);//ControlScreen����Ă΂��
        }
    }

    public void Brake(bool isBrake)
    {
        this.isBrake = isBrake;
        effectManager.Brake(isBrake);
        wheelMg.WheelBlade(isBrake);
    }

    public void ForceBoost(int gearNum)
    {
        Brake(false);//ForceBoost()�Ńu���[�L������
        effectManager.JetEffect();
        Vector2 force = transform.localScale.x * transform.right * grypsParameter.boostPower[gearNum - 1];
        rb.AddForce(force);
    }

    public void ForceDash(int key, int power)
    {
        Brake(false);//ForceDash()�Ńu���[�L������
        if (key != (int)transform.localScale.x)
        {
            rb.velocity = Vector2.zero;
            Turn(key, false);
        }
        stageCrl.controlScreenMg.KeyChange(key);
        Vector2 force = transform.localScale.x * transform.right * grypsParameter.dashPower[power];
        rb.AddForce(force);
    }

    public void ForceJet(int power)
    {
        Brake(false);//ForceJet()�Ńu���[�L������
        Vector2 force = transform.localScale.x * transform.right * grypsParameter.jetPower[power];
        rb.AddForce(force);
    }

    private void Land()
    {
        stageCrl.jetMg.StartUpJetHud();
    }

    private void TakeOff()
    {

        //�T�}�[�\���gHud�N��
    }

    /*
    private void JetCount(int somersaultCount)
    {
        jetMemory += somersaultCount;

        if (somersaultCount >= 5)
        {
            DashA((int)transform.localScale.x, 2);
        }
        else if (somersaultCount >= 3)
        {
            DashA((int)transform.localScale.x, 1);
        }
        else if (somersaultCount >= 1)
        {
            DashA((int)transform.localScale.x, 0);
        }

        if (jetMemory >= 6)
        {
            jetMemory = 6;
            GetUpJet();
        }

        if (oldJetMemory != jetMemory)
        {
            //jetMemoryMg.DotweenJetMemory(oldJetMemory, jetMemory);
            anim.SetFloat("jet", jetMemory);
            oldJetMemory = jetMemory;
        }

        if (jetMemory >= 6)
        {
            cinemachineCtrl.ZoomCamera(boxNum: 3, time: 0.5f, type: Ease.OutQuad);
        }
        else if (jetMemory >= 4)
        {
            cinemachineCtrl.ZoomCamera(boxNum: 2, time: 0.5f, type: Ease.OutQuad);
        }
        else if (jetMemory >= 2)
        {
            cinemachineCtrl.ZoomCamera(boxNum: 1, time: 0.5f, type: Ease.OutQuad);
        }
        else
        {
            cinemachineCtrl.ZoomCamera();
        }

    }

    public void OnRocketButton()
    {
        if (isFreeze)
        {
            return;
        }

        if (isJet)
        {
            rb.velocity = new Vector3(0, 0, 0);
            if (key >= 0)
            {
                this.rb.AddForce(transform.right * jetLevel[2]);
            }
            else if (key < 0)
            {
                this.rb.AddForce(-transform.right * jetLevel[2]);
            }
            GetDownJet();
            effectManager.JetEffect();

            anim.SetFloat("jet", 0);
            anim.SetTrigger("release");

            SoundController.instance.PlayJetSe(jet[3]);
            jetMemory = 0;
            if (oldJetMemory != jetMemory)
            {
                jetMemoryMg.DotweenJetMemory(oldJetMemory, jetMemory);
                oldJetMemory = jetMemory;
            }
            StartCoroutine(DelayMethod(1.0f, () => { cinemachineCtrl.ZoomCamera(); }));
        }
    }
    */


    public void RecIsJet()
    {
        passedIsJet = isJet;
    }

    public void GetUpJet()
    {
        if (isJet)
        {
            return;
        }
        isJet = true;
        //rocketImg.color = new Color(255f, 255f, 255f, 255f);
    }
    public void GetDownJet()
    {
        if (!isJet)
        {
            return;
        }
        isJet = false;
        //rocketImg.color = new Color(255f, 255f, 255f, 0.4f);
        //jetcount = 0;
    }

    /*
    private void Landing()
    {
        //�J�E���g�A�b�v
        JetCount(somersaultCount);
        somersaultCount = 0;

        //�{�^����\��
        somersaultButton.interactable = false;
        memoryGageMg.getMemoryMg.DisAppeearPlusMark();

        //���Z�b�g
        isSomersault = false;
        myAngle = 0f;

        //stageCtrl.state��Lack��ԂŁA���̃���������0�ȏ�ł���΁ARegeneration()�N�����APlay��ԂɈڍs����
        if ((int)stageCrl.state == 2 && memoryGageMg.memoryGage >= 0)
        {
            stageCrl.Regeneration();
            Debug.Log("Regeneration()");
        }
    }
     */



    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }


    public void PausePlayer()
    {
        isFreeze = true;
        rb.velocity = new Vector2(0, 0);
        //speedArrow.CompleteSequence(key);
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);//��]���Z�b�g
    }

    public void SomerSault(float angularChangeInDegrees)
    {
        if (!isSomersault)
        {
            CalcForceDirection();
            Vector3 force = forceMagnitude * forceDirection;
            rb.AddForce(force, ForceMode2D.Impulse);
            //rb.AddForce(force, ForceMode2D.Force);
            //memoryGageMg.getMemoryMg.AppearPlusMark();
            //transform.DOLocalRotate(new Vector3(0, 0, 360f), 0.25f, RotateMode.FastBeyond360);
            nowVector = transform.rotation * Vector2.up;//���̌����̃x�N�^�[
            resultAngle = (int)Vector2.Angle(nowVector, targetVector);
            resultAngle = 360 - resultAngle;

            //Debug.Log(resultAngle);
            float impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;

            rb.AddTorque(transform.localScale.x * impulse, ForceMode2D.Impulse);
            isSomersault = true;

        }
    }


}
