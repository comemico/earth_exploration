using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class WheelManager : MonoBehaviour
{

    [Header("バーナウト値")]
    [NamedArrayAttribute(new string[] { "1段階", "2段階", "3段階", "4段階", "5段階", })]
    [Range(500, 2000)] public int[] burnPower;

    [Header("前輪 :参照")] public Transform frontTransform;
    [Header("前輪 :円周")] public float radiusFront; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する

    [Header("後輪 :参照")] public Transform[] wheelBlade;
    [Header("後輪 :円周")] public float radiusRear; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する

    [Header("---イージング----")]
    [Header("終了値")]
    public float endValue;
    [Header("種類")]
    public Ease easeType;
    [Header("時間")]
    public float easeDuration;

    private GrypsController grypsCrl;
    private float factorFront;
    private float factorRear;
    //public Light2D light2d;

    private List<Tween> tweenList = new List<Tween>();
    private void Awake()
    {
        grypsCrl = GetComponentInParent<GrypsController>();
        //light2d = GetComponentInChildren<Light2D>();        
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
                BurnOutWheel(grypsCrl.stageCrl.controlScreenMg.gearNum - 1);
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
        transform.Rotate(0f, 0f, -1 * burnPower[gearNum] * Time.deltaTime);
        //light
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