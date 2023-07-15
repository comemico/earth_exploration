using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using SoundSystem;

public class GrypsController : MonoBehaviour
{
    public GrypsParameter grypsParameter;

    #region//インスペクターで設定する

    [Header("デバックモード切替ボタン")] public bool isDebugMode;
    [Header("1メモリ当たりのスワイプ距離")] public float distancePerMemory;
    //[Header("BoostLevel")] public int[] boostLevel;
    //[Header("DashLevel")] public int[] dashLevel;
    [Header("JetLevel")] public int[] jetLevel;
    // [Header("インターバル")] public float interval;
    [Header("サマーソルト終了時角度(何度傾けるか)")] public float targetAngle;
    [Header("力")] public float forceMagnitude;
    [Header("角度")] public float forceAngle;


    //[Header("当たり範囲")] public float stepOnRate;
    //[Header("最大力")] public int maxSpeed;
    [Header("エンジン音string")] public string[] engine;
    [Header("ジェット音string")] public string[] jet;
    #endregion

    [Header("stageCtrl")] public StageCtrl stageCrl;
    //[Header("JetButton")] public Image rocketImg;
    [Header("SomersaultButton")] public Button somersaultButton;

    [Header("GroundCheck")] GroundCheck ground;
    [Header("Effect")] EffectManager effectManager;
    [Header("Cinemachine")] CinemachineController cinemachineCtrl;

    //[Header("BoostArrow")] public SpeedArrowManager speedArrow;
    [Header("MemoryGage")] public MemoryGageManager memoryGageMg;
    [Header("JetMemory")] public JetMemoryManager jetMemoryMg;

    #region//プライベート変数
    private Vector2 startPos;
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
    //private bool isInterval;
    private string deadAreaTag = "DeadArea";
    private string clearAreaTag = "ClearArea";
    private int oldGearNum = 0;
    /*
    private int key = 1;//向き
    private int oldKey;//最後に変わった向き
     */

    private int somersaultCount = 0;
    private int jetMemory = 0;
    private int oldJetMemory = 0;

    //private int jetcount = 0;
    private int gearNum;
    private float intervalTimer = 0.0f;
    private float factorDistance;

    private Quaternion prev;
    private float myAngle;
    private int resultAngle;
    private Vector2 targetVector;
    private Vector2 nowVector;
    public bool isSomersault = false;

    #endregion

    private void Awake()
    {
    }

    void Start()
    {
        AllGetComponent();

        targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//ターゲットの角度→ベクター
        factorDistance = 1 / distancePerMemory;
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
        //speedArrow = stageCtrl.speedArrow.GetComponent<SpeedArrowManager>();
        memoryGageMg = stageCrl.memoryGageMg.GetComponent<MemoryGageManager>();
        jetMemoryMg = stageCrl.jetMemoryMg.GetComponent<JetMemoryManager>();

        if (cinemachineCtrl == null || effectManager == null || ground == null || memoryGageMg == null)
        {
            Debug.Log("GrypsManager.cs: warning : スクリプトが正しくアタッチされていません");
        }
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//着地判定
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
            //effectManager.Brake(true);
            if (Mathf.Abs(rb.velocity.x) <= 0.25f)
            {
                effectManager.Brake(false);
                isBrake = false;
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
                    memoryGageMg.getMemoryMg.DisplaySomerSaultCount(somersaultCount);
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
        if (Mathf.Approximately(Time.timeScale, 0f))
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

            if (isGround && !isInterval)//メーター変更

            {
                ConsumptionMemory();
            }

            if (Input.GetMouseButtonUp(0) && isGround & !isInterval)
            {
                RevDown();
            }
            if (isGround)//向き変更
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
    public void Turn(int key)
    {
        transform.localScale = new Vector3(key, transform.localScale.y, transform.localScale.z);

        effectManager.Brake(true);
        isBrake = true;
    }

    public void Brake()
    {
        //WheelBlade();
        //BrakeEffect();
    }

    public void Boost(int gearNum, int key)
    {
        effectManager.JetEffect();
        rb.AddForce(key * transform.right * grypsParameter.boostPower[gearNum - 1]);

        effectManager.Brake(false);
        isBrake = false;//掛かっているブレーキを解除する念のため
    }

    public void Dash(int dashPower, bool directionLimit = false, int DestinationLocalScaleX = 1)
    {

        if (directionLimit)//方向制限の有無...(AccelerationArea)側で操作
        {
            if (transform.localScale.x != DestinationLocalScaleX)//反対方向の場合...(Player)と(AccelerationArea)の向きを比べる
            {
                transform.localScale = new Vector3(DestinationLocalScaleX, transform.localScale.y, transform.localScale.z);
                rb.velocity = new Vector2((0.2f * DestinationLocalScaleX), 0);//keyが変わり、velocityが0.25以下になるまでブレーキ(isBrake=true)が掛かり続けてしまう
                isBrake = false;//掛かっているブレーキを解除する念のため
            }
        }

        rb.AddForce(transform.localScale.x * transform.right * grypsParameter.dashPower[dashPower]);
        effectManager.JetEffect();
        stageCrl.ChangeToRestrictedControl();
        //SoundController.instance.PlayJetSe(jet[3]);
        //isInterval = true;
        //intervalTimer = 0.0f;
        //stageCrl => 操作一部不能へ
    }

    public void DashA(int key, int power)
    {
        //rb.velocity = Vector2.zero;
        stageCrl.controlScreenMg.KeyChange(key);
        rb.AddForce(transform.localScale.x * transform.right * grypsParameter.dashPower[power]);
    }


    private void JetCount(int somersaultCount)
    {
        jetMemory += somersaultCount;

        if (somersaultCount >= 5)
        {
            Dash(2);
        }
        else if (somersaultCount >= 3)
        {
            Dash(1);
        }
        else if (somersaultCount >= 1)
        {
            Dash(0);
        }

        if (jetMemory >= 6)
        {
            jetMemory = 6;
            GetUpJet();
        }

        if (oldJetMemory != jetMemory)
        {
            jetMemoryMg.DotweenJetMemory(oldJetMemory, jetMemory);
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


    /*
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

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    /*
    public void DashPanel(int levelNum = 0, bool directionLimit = false, int direction = 1)
    {

        if (directionLimit)//方向制限の有無...(AccelerationArea)側で操作
        {
            if (key != direction)//反対方向の場合...(Player)と(AccelerationArea)の向きを比べる
            {
                key = direction;
                rb.velocity = new Vector2((0.2f * direction), 0);//keyが変わり、velocityが0.25以下になるまでブレーキ(isBrake=true)が掛かり続けてしまう
                isBrake = false;//掛かっているブレーキを解除する念のため
            }
        }

        if (key >= 0)
        {
            this.rb.AddForce(transform.right * dashLevel[levelNum]);
        }
        else if (key < 0)
        {
            this.rb.AddForce(-transform.right * dashLevel[levelNum]);
        }

        effectManager.JetEffect();
        SoundController.instance.PlayJetSe(jet[3]);
        intervalTimer = 0.0f;
        isInterval = true;

    }
     */

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

    private void TakeOff()
    {
        somersaultButton.interactable = true; //ボタン表示
    }

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

    public void Down()
    {
        PausePlayer();
        stageCrl.GameOver();
    }

    public void Clear()
    {
        PausePlayer();
        stageCrl.StageClear();
    }

    public void PausePlayer()
    {
        isFreeze = true;
        rb.velocity = new Vector2(0, 0);
        //speedArrow.CompleteSequence(key);
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);//回転リセット
    }

    public void SuckedIn()
    {
        //対象の位置へ移動する
        //操作不能にする
    }

    public void EjectFrom()
    {
        //向きを出口の方向に向かせる
        //指定のForceを加える
        //操作可能にする
    }

    public void EnterWarp(Transform transferDestination)
    {
        PausePlayer();
        transform.position = transferDestination.position;
        transform.localScale = new Vector3((int)transferDestination.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void ExitWarp(int forceNum)
    {
        rb.velocity = new Vector2(3f, 0f);
        // rb.AddForce(transform.localScale.x * transform.right * dashLevel[forceNum]);
    }

    public void SomerSault(float angularChangeInDegrees)
    {
        if (!isSomersault)
        {
            CalcForceDirection();
            Vector3 force = forceMagnitude * forceDirection;
            rb.AddForce(force, ForceMode2D.Impulse);
            //rb.AddForce(force, ForceMode2D.Force);
            memoryGageMg.getMemoryMg.AppearPlusMark();
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


}
