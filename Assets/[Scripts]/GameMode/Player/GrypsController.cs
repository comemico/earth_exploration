using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using SoundSystem;

public class GrypsController : MonoBehaviour
{
    //主に物理演算に関わるスクリプト

    public StageCtrl stageCrl;
    public GrypsEffector effector;
    public WheelManager wheelMg;
    public GrypsParameter parameter;

    [HideInInspector] public EffectManager effectManager;
    GroundCheck ground;

    public bool isBrake;

    [HideInInspector] public Rigidbody2D rb = null;

    [Header("ブーストのリロード基準")]
    public float reloadTime = 0.1f;
    [Range(25, 100)] public float reloadVelocity;
    private float time;

    private bool isGround = false;
    private bool isGroundPrev = false;

    /*
    private Vector3 forceDirection = new Vector3(1f, 1f, 1f);
    private bool passedIsJet = false;
    private bool isJet = false;
    private Quaternion prev;
    private float myAngle;
    private int resultAngle;
    private Vector2 targetVector;
    private Vector2 nowVector;
    public bool isSomersault = false;
    [Header("サマーソルト終了時角度(何度傾けるか)")]
    public float targetAngle;
    public float forceMagnitude;
    public float forceAngle;
    private int somersaultCount = 0;
    Tween tween_salto;
     */

    private void Awake()
    {
        AllGetComponent();
        //targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//ターゲットの角度→ベクター
    }

    /*
    void CalcForceDirection()
    {
        float rad = forceAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad);
        float y = Mathf.Sin(rad);
        float z = 0f;
        forceDirection = new Vector3(x, y, z);
    }
     */

    void AllGetComponent()
    {
        rb = GetComponent<Rigidbody2D>();

        effector = GetComponentInChildren<GrypsEffector>();
        parameter = GetComponentInChildren<GrypsParameter>();
        wheelMg = GetComponentInChildren<WheelManager>();
        effectManager = GetComponentInChildren<EffectManager>();
        ground = GetComponentInChildren<GroundCheck>();

        //sortingGroup = GetComponentInChildren<SortingGroup>();
        //capsuleCol = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//着地判定

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

        if (isBrake)
        {
            rb.velocity *= parameter.breakPower;
            if (Mathf.Abs(rb.velocity.x) <= 0.25f) Brake(false);//velocityが 0.25f以下で解除
        }

        if (stageCrl.controlStatus == StageCtrl.ControlStatus.restrictedControl && Mathf.Abs(rb.velocity.x) <= reloadVelocity)
        {
            if (time >= reloadTime)
            {
                stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
                wheelMg.WheelLamp(false, true);
                time = 0.0f;
            }
            else
            {
                time += Time.deltaTime;
            }

        }

        /*
        var tes = rb.velocity.normalized;
        float ang = Vector2.Angle(rb.velocity, Vector2.right);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, Quaternion.Lerp(transform.localRotation, ang, 0.5f));
         */

        /*
    if (isSomersault)
    {
        Vector2 prevvec = prev * Vector2.up;
        Vector2 nowvec = transform.rotation * Vector2.up;
        float angle = Vector2.Angle(prevvec, nowvec);
        myAngle += angle;
        prev = transform.rotation;
        if (myAngle >= resultAngle - 35f)
        {

        }
        if (transform.localScale.x == 1)//右を見ている
        {
            transform.localEulerAngles = new Vector3(0f, 0f, (360 - targetAngle));//340 : 
        }
        else if (transform.localScale.x == -1)//左を見ている
        {
            transform.localEulerAngles = new Vector3(0f, 0f, (360 + targetAngle));//380 : 360からどれだけ離れているのかで考えてみた
        }
        if (myAngle >= resultAngle - 35f)
        {
            //Debug.Log("減速中");
            rb.angularVelocity *= 0.75f;

            if (myAngle >= resultAngle)
            {
                somersaultCount++;
                //memoryGageMg.getMemoryMg.DisplaySomerSaultCount(somersaultCount);
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
         */


        /*
    if (isTurning)
    {
        moveDistance += Mathf.Abs(rb.velocity.x * Time.deltaTime);
        heightFactor = moveDistance * distanceFactor;
        Debug.Log(moveDistance);
        transform.GetChild(0).position = new Vector3(transform.position.x, transform.position.y + (heightFactor * goalHeight), transform.position.z);

        if (goalHeight >= goalHeight * heightFactor)
        {
            isTurning = false;
        }
        if (moveDistance >= 30)
        {
            isTurning = false;
            Debug.Log("stop");
        }
    }
         */

    }


    public void ForceBoost(int gearNum)
    {
        Brake(false);//ForceBoost()でブレーキを解除
        Vector2 force = transform.localScale.x * transform.right * parameter.boostPower[gearNum - 1];
        rb.AddForce(force, ForceMode2D.Impulse);
        //effectManager.JetEffect();
        wheelMg.ArchedBack(false);
    }

    public void Turn(int key, bool isBrake)
    {
        transform.localScale = new Vector3(key, transform.localScale.y, transform.localScale.z);
        if (isBrake) Brake(true);//ControlScreenから呼ばれる
    }

    public void Return()
    {
        rb.DORotate(0f, 0.5f).SetEase(Ease.OutSine);
    }

    public void ForceDash(int key, int power)
    {
        Brake(false);//ForceDash()でブレーキを解除
        if (key != (int)transform.localScale.x)
        {
            rb.velocity = Vector2.zero;
            Turn(key, false);
        }
        wheelMg.WheelLamp(true, true);
        stageCrl.controlScreenMg.KeyChange(key);
        Vector2 force = transform.localScale.x * transform.right * parameter.dashPower[power];
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Brake(bool isBrake)
    {
        this.isBrake = isBrake;
        effectManager.Brake(isBrake);
        wheelMg.WheelBlade(isBrake);
    }

    public void ForceJet(int power)
    {
        Brake(false);//ForceJet()でブレーキを解除
        Vector2 force = transform.localScale.x * transform.right * parameter.jetPower[power];
        rb.AddForce(force, ForceMode2D.Impulse);
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.restrictedControl);
    }

    private void TakeOff()
    {
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
        Brake(false);//TakeOff()でブレーキを解除
        //rb.DORotate(-1 * (int)transform.localScale.x * 10f, 0.5f).SetEase(Ease.OutSine);
    }

    private void Land()
    {
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
        stageCrl.saltoMg.Release(); //Salto中着地した場合SaltoHudをShutdownさせるために呼ぶ
        wheelMg.WheelLamp(false, true);
    }



    /*
    public void SomerSault(float angularChangeInDegrees)
    {
        if (!isSomersault)
        {
            CalcForceDirection();
            Vector3 force = forceMagnitude * forceDirection;
            nowVector = transform.rotation * Vector2.up;//今の向きのベクター
            resultAngle = (int)Vector2.Angle(nowVector, targetVector);
            resultAngle = 360 - resultAngle;
            //Debug.Log(resultAngle);
            float impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;
            rb.AddTorque(transform.localScale.x * impulse, ForceMode2D.Impulse);
            isSomersault = true;

            rb.AddForce(force, ForceMode2D.Impulse);
            //rb.AddForce(force, ForceMode2D.Force);
        }
    }
     */


    /*
    public void TurnCorner(float distanceHeight, float distanceMoving)
    {
        turnPoint = transform.position.x;
        distanceFactor = 1 / distanceMoving;
        goalHeight = distanceHeight;
        transform.GetChild(0).position = new Vector3(transform.position.x, transform.position.y + distanceHeight, transform.position.z);
        isTurning = true;
    }
     */

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
    public void RecIsJet()
    {
        passedIsJet = isJet;
    }

    public void GetUpJet()
    {
        if (isJet) return;
        isJet = true;
        //rocketImg.color = new Color(255f, 255f, 255f, 255f);
    }
    public void GetDownJet()
    {
        if (!isJet) return;
        isJet = false;
        //rocketImg.color = new Color(255f, 255f, 255f, 0.4f);
        //jetcount = 0;
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    */
    /*
    private void Landing()
    {
        //カウントアップ
        JetCount(somersaultCount);
        somersaultCount = 0;

        //ボタン非表示
        somersaultButton.interactable = false;
        memoryGageMg.getMemoryMg.DisAppeearPlusMark();

        //リセット
        isSomersault = false;
        myAngle = 0f;

        //stageCtrl.stateがLack状態で、今のメモリ数が0以上であれば、Regeneration()起動し、Play状態に移行する
        if ((int)stageCrl.state == 2 && memoryGageMg.memoryGage >= 0)
        {
            stageCrl.Regeneration();
            Debug.Log("Regeneration()");
        }
    }
     */

    /*
    public void PausePlayer()
    {
        rb.velocity = new Vector2(0, 0);
        //speedArrow.CompleteSequence(key);
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);//回転リセット
    }
     */




}
