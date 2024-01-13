using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using SoundSystem;

public class GrypsController : MonoBehaviour
{
    public StageCtrl stageCrl;
    public MemoryGageManager memoryGageMg;
    [HideInInspector] public GrypsParameter grypsParameter;
    [HideInInspector] public WheelManager wheelMg;
    [HideInInspector] public EffectManager effectManager;
    GroundCheck ground;

    public bool isBrake;

    [Header("サマーソルト終了時角度(何度傾けるか)")]
    public float targetAngle;
    public float forceMagnitude;
    public float forceAngle;
    private int somersaultCount = 0;
    Tween tween_salto;

    [HideInInspector] public Rigidbody2D rb = null;
    public Animator jetAnimator;
    public Renderer jetLamp;

    public Animator saltoAnimator;

    public SortingGroup sortingGroup;
    CapsuleCollider2D capsuleCol;

    [Header("ブーストのリロード基準")]
    public float reloadTime = 0.1f;
    [Range(25, 100)] public float reloadVelocity;
    private float time;



    private Vector3 forceDirection = new Vector3(1f, 1f, 1f);
    private bool isGroundPrev = false;
    private bool isGround = false;
    private bool passedIsJet = false;
    private bool isJet = false;
    private Quaternion prev;
    private float myAngle;
    private int resultAngle;
    private Vector2 targetVector;
    private Vector2 nowVector;
    public bool isSomersault = false;

    /*
    private bool isTurning;
    private float distanceFactor;
    private float heightFactor;
    private float goalHeight;
    private float moveDistance;
    private float turnPoint;
    private bool isForce = false;
    //[Header("JetLevel")] public int[] jetLevel;
    //[Header("1メモリ当たりのスワイプ距離")] public float distancePerMemory;
    //[Header("BoostLevel")] public int[] boostLevel;
    //[Header("DashLevel")] public int[] dashLevel;
    //[Header("エンジン音string")] public string[] engine;
    //[Header("ジェット音string")] public string[] jet;
    //[Header("当たり範囲")] public float stepOnRate;
    //[Header("最大力")] public int maxSpeed;
    //[Header("Cinemachine")] CinemachineController cinemachineCtrl;
    //[Header("JetMemory")] public JetManager jetMemoryMg;
    //public bool isRevup;
    //public bool isFreeze = false;
    //private Vector2 startPos;
    private int key = 1;//向き
    private int oldKey;//最後に変わった向き
    private int jetcount = 0;
    private int oldGearNum = 0;
    private int gearNum;
    private float factorDistance;
     */


    private void Awake()
    {
        AllGetComponent();
        targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//ターゲットの角度→ベクター
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
        rb = GetComponent<Rigidbody2D>();
        sortingGroup = GetComponentInChildren<SortingGroup>();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        grypsParameter = GetComponent<GrypsParameter>();
        wheelMg = GetComponentInChildren<WheelManager>();
        effectManager = GetComponentInChildren<EffectManager>();
        ground = GetComponentInChildren<GroundCheck>();
        jetLamp = jetAnimator.GetComponent<Renderer>();
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

        /*
        var tes = rb.velocity.normalized;
        float ang = Vector2.Angle(rb.velocity, Vector2.right);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, Quaternion.Lerp(transform.localRotation, ang, 0.5f));
         */

        if (isBrake)
        {
            rb.velocity *= grypsParameter.breakPower;
            if (Mathf.Abs(rb.velocity.x) <= 0.25f) Brake(false);//velocityが 0.25f以下で解除
        }


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
            /*
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
             */
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


    /*
        if (Mathf.Approximately(Time.timeScale, 0f))//timescale=0ならreturn
        {
            return;
        }
    //押した時の処理
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
    //押し続けている時の処理
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
            oldKey = key;//更新
        }
    }



    //押し続けている時の処理
    public int ConsumptionMemory()
    {
        if (isFreeze)
        {
            return 0;
        }

        int maxGage;
        int medianValue;

        if (Input.GetMouseButton(0))//残りのメモリに応じてmaxGageを変更する
        {
            if (memoryGageMg.memoryGage <= 4)
            {
                maxGage = memoryGageMg.memoryGage;
            }
            else
            {
                maxGage = 5;
            }

            int swipeLength = (int)(Mathf.Abs(this.startPos.x - Input.mousePosition.x));//整数絶対値の距離

            swipeLength = Mathf.Clamp(swipeLength, 0, (maxGage * (int)distancePerMemory + 1));

            gearNum = (int)(factorDistance * swipeLength);

            gearNum = Mathf.Clamp(gearNum, 0, maxGage);

            medianValue = swipeLength - (gearNum * (int)distancePerMemory);//100のところをdistancePerMemoryでいれてもよいかも


            //speedArrow.ChargeGear(gearNum, factorDistance, medianValue);
            //speedArrow.ChargeGear(gearNum, medianValue);

            if (oldGearNum != gearNum)//メモリが変わった時だけ、メモリ表示の処理を行なってもらう
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
    //離した時の処理
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
                //不足状態→まだスピードが落ちていない、回転によってメモリが回復する
            }
        }
    }
     */
    public void ForceBoost(int gearNum)
    {
        Brake(false);//ForceBoost()でブレーキを解除
        Vector2 force = transform.localScale.x * transform.right * grypsParameter.boostPower[gearNum - 1];
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
        /*
        float inclination = transform.rotation.z;
        transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, inclination);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);//回転リセット
        transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, (int)transform.localScale.x * 0f), 0.3f, RotateMode.Fast).SetEase(Ease.OutSine);
         */
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
        Vector2 force = transform.localScale.x * transform.right * grypsParameter.dashPower[power];
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
        Vector2 force = transform.localScale.x * transform.right * grypsParameter.jetPower[power];
        rb.AddForce(force, ForceMode2D.Impulse);
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.restrictedControl);
    }

    public void Salto()
    {
        tween_salto.Kill(true);
        tween_salto = transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 360f), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.OutSine).OnComplete(stageCrl.saltoMg.SaltoComplete);
        /*
        if ((int)transform.localScale.x == 1)//右を見ている
        {
            tween_salto.Kill(true);
            tween_salto = transform.DORotate(new Vector3(0, 0, 360f - targetAngle), 0.25f).SetRelative(true).SetUpdate(false).SetEase(Ease.OutSine);
        }
        else if ((int)transform.localScale.x == -1)//左を見ている
        {
            tween_salto.Kill(true);
            tween_salto = transform.DOLocalRotate(new Vector3(0, 0, 360f + targetAngle), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.OutSine);
        }
        */
    }

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

    private void Land()
    {
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
        stageCrl.saltoMg.Release(); //Salto中着地した場合SaltoHudをShutdownさせるために呼ぶ
        wheelMg.WheelLamp(false, true);
    }

    private void TakeOff()
    {
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
        Brake(false);//TakeOff()でブレーキを解除
        //rb.DORotate(-1 * (int)transform.localScale.x * 10f, 0.5f).SetEase(Ease.OutSine);
    }

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
    */

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
