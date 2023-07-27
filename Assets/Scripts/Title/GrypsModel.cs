using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DG.Tweening;
using System;
using UnityEngine.UI;

public class GrypsModel : MonoBehaviour
{
    public GrypsParameter grypsParameter;
    public WheelManager wheelMg;

    #region//インスペクターで設定する
    [Header("デバックモード切替ボタン")] public bool isDebugMode;
    [Header("サマーソルト終了時角度(何度傾けるか)")] public float targetAngle;
    [Header("力")] public float forceMagnitude;
    [Header("角度")] public float forceAngle;
    #endregion

    [Header("stageCtrl")] public StageCtrl stageCrl;
    [Header("SomersaultButton")] public Button somersaultButton;
    [Header("GroundCheck")] GroundCheck ground;
    [Header("Effect")] EffectManager effectManager;
    [Header("Cinemachine")] CinemachineController cinemachineCtrl;
    [Header("MemoryGage")] public MemoryGageManager memoryGageMg;
    [Header("JetMemory")] public JetMemoryManager jetMemoryMg;

    #region//プライベート変数
    private Vector3 forceDirection = new Vector3(1f, 1f, 1f);
    public Rigidbody2D rb = null;
    private Animator anim = null;
    public bool isBrake;
    private bool isGroundPrev = false;
    private bool isGround = false;
    private bool passedIsJet = false;
    private bool isJet = false;
    private int somersaultCount = 0;
    private string deadAreaTag = "DeadArea";
    private string clearAreaTag = "ClearArea";
    private int jetMemory = 0;
    private int oldJetMemory = 0;
    private Quaternion prev;
    private float myAngle;
    private int resultAngle;
    private Vector2 targetVector;
    private Vector2 nowVector;
    public bool isSomersault = false;
    #endregion

    private void Awake()
    {
        AllGetComponent();
    }

    void Start()
    {
        targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//ターゲットの角度→ベクター
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
        isGround = ground.IsGround();//着地判定
        var tes = rb.velocity.normalized;
        float ang = Vector2.Angle(rb.velocity, Vector2.right);
        //transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, Quaternion.Lerp(transform.localRotation, ang, 0.5f));

        /*
        if (isGround != isGroundPrev)
        {
            if (isGround == true)
            {
                Landing();
                isGroundPrev = true;
            }
            else if (isGround == false)
            {
                TakeOff();
                isGroundPrev = false;
            }
        }
         */

        if (isBrake)
        {
            rb.velocity *= grypsParameter.breakPower;
            if (Mathf.Abs(rb.velocity.x) <= 0.25f)
            {
                Brake(false);
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
                //Debug.Log("減速中");
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
                    if (transform.localScale.x == 1)//右を見ている
                    {
                        transform.localEulerAngles = new Vector3(0f, 0f, (360 - targetAngle));//340 : 
                    }
                    else if (transform.localScale.x == -1)//左を見ている
                    {
                        transform.localEulerAngles = new Vector3(0f, 0f, (360 + targetAngle));//380 : 360からどれだけ離れているのかで考えてみた
                    }
                    isSomersault = false;
                }
            }
        }


    }

    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f)) return;

    }


    public void Turn(int key, bool isBrake)
    {
        transform.localScale = new Vector3(key, transform.localScale.y, transform.localScale.z);
        if (isBrake)
        {
            Brake(true);
        }
        //effectManager.Brake(true);
        //isBrake = true;
    }

    public void Brake(bool isBrake)
    {
        this.isBrake = isBrake;
        effectManager.Brake(isBrake);
        wheelMg.WheelBlade(isBrake);
        //BrakeEffect();
    }

    public void Boost(int gearNum)//, int key)
    {
        Brake(false);//掛かっているブレーキを解除する念のため
        rb.AddForce(transform.localScale.x * transform.right * grypsParameter.boostPower[gearNum - 1]);
        effectManager.JetEffect();
    }

    public void DashA(int key, int power)
    {
        if (key != transform.localScale.x)
        {
            rb.velocity = Vector2.zero;
            Turn(key, false);
            Debug.Log("Turn");
            //transform.localScale = new Vector3(key, transform.localScale.y, transform.localScale.z);
        }
        stageCrl.controlScreenMg.KeyChange(key);

        Vector2 force = transform.localScale.x * transform.right * grypsParameter.dashPower[power];
        rb.AddForce(force);

    }

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
            nowVector = transform.rotation * Vector2.up;//今の向きのベクター
            resultAngle = (int)Vector2.Angle(nowVector, targetVector);
            resultAngle = 360 - resultAngle;

            //Debug.Log(resultAngle);
            float impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;

            rb.AddTorque(transform.localScale.x * impulse, ForceMode2D.Impulse);
            isSomersault = true;

        }
    }
    /*
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    private void TakeOff()
    {
        somersaultButton.interactable = true; //ボタン表示
    }
     */

    /*それぞれのオブジェクトに役割を与える
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == deadAreaTag)
        {
            Down();
        }
        else if (collision.tag == clearAreaTag)
        {
            isFreeze = true;
            transform.DOMove(collision.transform.position, 0.35f)
                .OnComplete(() =>
                {
                    Clear();
                });
        }
    }
     */

}
