using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class WheelManager : MonoBehaviour
{

    [Header("バーナウト値")]
    [NamedArrayAttribute(new string[] { "1段階", "2段階", "3段階", "4段階", "5段階", })]
    [Range(500, 2000)] public int[] burnPower;

    [Header("前輪 ")]
    public Transform frontTransform;
    public float radiusFront; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する
    public Renderer matFront;
    Tween tween_lampFront;

    [Header("後輪 ")]
    public Transform[] wheelBlade;
    public float radiusRear; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する
    public Renderer matRear;
    public SpriteRenderer breakPad;

    [Header("WheelLamp")]
    public float lampDuration;
    public Ease lampType;
    Tween tween_lampRear;
    bool isGain;
    bool oldJudge;

    [Header("ArchedBack : バーナウト時車体を傾ける")]
    public float arched_Value;
    public float arched_Time;
    public float arched_overshoot;

    public float arched_Return_Time;
    public Ease arched_Return_Type;

    [Header("WheelBlade : ブレーキ中タイヤの爪が展開する")]
    public float endValue;
    public float easeDuration;
    public Ease easeType;

    private GrypsController grypsCrl;
    private float factorFront;
    private float factorRear;

    private List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        grypsCrl = GetComponentInParent<GrypsController>();
    }
    private void Start()
    {
        factorFront = 1 / radiusFront;
        factorRear = 1 / radiusRear;
    }

    private void Update()
    {
        //前輪
        if (Mathf.Abs(grypsCrl.rb.velocity.x) >= 0.5f)
        {
            SpinWheel(frontTransform, factorFront);//前輪
        }

        //後輪
        if (grypsCrl.stageCrl.controlScreenMg.gearNum >= 1)
        {
            BurnOutWheel(grypsCrl.stageCrl.controlScreenMg.gearNum);
        }
        else
        {
            if (Mathf.Abs(grypsCrl.rb.velocity.x) >= 0.5f)
            {
                SpinWheel(transform, factorRear);//後輪
            }
        }

    }

    public void SpinWheel(Transform wheel, float factor)
    {
        var translation = grypsCrl.rb.velocity * Time.deltaTime; // 位置の変化量
        var distance = translation.magnitude; // 移動した距離
        var angle = distance * factor; // (distance / circleCollider.radius) 球が回転するべき量 

        if (translation.x > 0)
        {
            wheel.Rotate(0f, 0f, (int)grypsCrl.transform.localScale.x * -1f * angle * Mathf.Rad2Deg);
        }
        else if (translation.x <= 0)
        {
            wheel.Rotate(0f, 0f, (int)grypsCrl.transform.localScale.x * 1f * angle * Mathf.Rad2Deg);
        }
    }


    public void BurnOutWheel(int gearNum)
    {
        Quaternion rot = Quaternion.Euler(0f, 0f, -1 * grypsCrl.transform.localScale.x * burnPower[gearNum - 1] * Time.deltaTime);
        // 現在の自信の回転の情報を取得する。
        Quaternion q = transform.rotation;
        // 合成して、自身に設定
        transform.rotation = rot * q;
        /*
        transform.Rotate(new Vector3(0f, 0f, burnPower[gearNum - 1]) * Time.deltaTime);
        //Quaternion rot = Quaternion.AngleAxis(-1 * burnPower[gearNum - 1] * Time.deltaTime, Vector3.forward);
         */
    }

    public void Judge(int gearNum)
    {
        isGain = (gearNum > 0);

        if (oldJudge != isGain)
        {
            WheelLamp(isGain, false);
            ArchedBack(isGain);
            oldJudge = isGain;
        }
    }

    public void WheelLamp(bool isGain, bool isFront)
    {
        tween_lampRear = DOTween.To(() => matRear.material.GetFloat("_Power"), x => matRear.material.SetFloat("_Power", x), Convert.ToInt32(isGain), lampDuration).SetEase(lampType);
        if (isFront) tween_lampFront = DOTween.To(() => matFront.material.GetFloat("_Power"), x => matFront.material.SetFloat("_Power", x), Convert.ToInt32(isGain), lampDuration).SetEase(lampType);
        //Debug.Log(Convert.ToInt32(isLamp));
    }

    public void ArchedBack(bool isGain)
    {
        grypsCrl.transform.GetChild(0).DOKill(true);

        if (isGain)
        {
            grypsCrl.transform.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, arched_Value), arched_Time).SetEase(Ease.OutBack, arched_overshoot);
        }
        else if (!isGain)
        {
            grypsCrl.transform.GetChild(0).DOLocalRotate(Vector3.zero, arched_Return_Time).SetEase(arched_Return_Type);
        }

        //grypsCrl.transform.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, archedValue * Convert.ToInt32(isGain)), archedDuration).SetEase(Ease.InBack, overshoot);
        //if (isGain)grypsCrl.transform.GetChild(0).DORotate(new Vector3(0f, 0f, 0.25f), 0.05f).SetEase(Ease.OutSine).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetDelay(archedDuration);
    }


    public void WheelBlade(bool isBrake)
    {
        if (isBrake)
        {
            for (int i = 0; i < wheelBlade.Length; i++)
            {
                wheelBlade[i].transform.DOLocalRotate(new Vector3(0f, 0f, endValue), easeDuration).SetEase(easeType);

            }

            breakPad.DOFade(1f, 0.2f);

        }
        else
        {
            for (int i = 0; i < wheelBlade.Length; i++)
            {
                wheelBlade[i].transform.DOLocalRotate(Vector3.zero, easeDuration).SetEase(easeType);
            }

            breakPad.DOFade(0f, 0.2f);

        }

        /*
        for (int i = 0; i < wheelBlade.Length; i++)
        {
            wheelBlade[i].transform.DOComplete();
            if (isBrake)
            {
                wheelBlade[i].transform.DOLocalRotate(new Vector3(0f, 0f, endValue), easeDuration).SetEase(easeType);
            }
            else
            {
                wheelBlade[i].transform.DOLocalRotate(Vector3.zero, easeDuration).SetEase(easeType);
            }
        }
         */
    }


}