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
    public int powerLevel;

    [Header("前輪 ")]
    public Transform frontTransform;
    public float radiusFront; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する
    public Renderer matFront;
    Tween tween_lampFront;

    [Header("後輪 ")]
    public Transform[] wheelBlade;
    public float radiusRear; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する
    public Renderer matRear;
    Tween tween_lampRear;
    bool isLamp;
    bool oldJudge;

    [Header("Lamp")]
    public float startValue_Lamp;
    public float easeDuration_Lamp;
    public Ease easeType_Lamp;

    [Header("WheelBlade")]
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
        if (Mathf.Abs(grypsCrl.rb.velocity.x) >= 1f)
        {
            SpinWheel(frontTransform, factorFront);//前輪
            SpinWheel(transform, factorRear);//後輪
        }

        //後輪のみ
        if (Mathf.Abs(grypsCrl.rb.velocity.x) <= 20f)
        {
            if (grypsCrl.stageCrl.controlScreenMg.gearNum >= 1)
            {
                BurnOutWheel(grypsCrl.stageCrl.controlScreenMg.gearNum);
                /*
                if (powerLevel != grypsCrl.stageCrl.controlScreenMg.gearNum)
                {
                    TurnOnLamp();
                    powerLevel = grypsCrl.stageCrl.controlScreenMg.gearNum;
                }
                 */

            }
        }
    }

    public void SpinWheel(Transform wheel, float factor)
    {
        var translation = grypsCrl.rb.velocity * Time.deltaTime; // 位置の変化量
        var distance = translation.magnitude; // 移動した距離
        var angle = distance * factor; // (distance / circleCollider.radius) 球が回転するべき量 
        wheel.Rotate(0f, 0f, -1 * angle * Mathf.Rad2Deg);
    }


    public void BurnOutWheel(int gearNum)
    {
        transform.Rotate(0f, 0f, -1 * burnPower[gearNum - 1] * Time.deltaTime);
    }

    public void Judge(int gearNum)
    {
        isLamp = (gearNum > 0);

        if (oldJudge != isLamp)
        {
            TurnLamp(isLamp, false);
            oldJudge = isLamp;
        }
    }

    public void TurnLamp(bool isLamp, bool isFront)
    {
        tween_lampRear = DOTween.To(() => matRear.material.GetFloat("_Power"), x => matRear.material.SetFloat("_Power", x), Convert.ToInt32(isLamp), easeDuration_Lamp).SetEase(easeType_Lamp);
        if (isFront) tween_lampFront = DOTween.To(() => matFront.material.GetFloat("_Power"), x => matFront.material.SetFloat("_Power", x), Convert.ToInt32(isLamp), easeDuration_Lamp).SetEase(easeType_Lamp);
        //Debug.Log(Convert.ToInt32(isLamp));
    }


    public void WheelBlade(bool isBrake)
    {
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
    }


}