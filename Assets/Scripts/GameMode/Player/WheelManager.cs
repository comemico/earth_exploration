using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WheelManager : MonoBehaviour
{

    [Header("回転力")]
    [NamedArrayAttribute(new string[] { "0段階", "1段階", "2段階", "3段階", "4段階", "5段階", })]
    [Range(0, 2000)] public int[] burnPower;

    [Header("タイヤをバーンナウトさせるか")] public bool isBurnOut;

    [Header("タイヤの直径")] public float radius; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する

    private GrypsController player;
    private Rigidbody2D rb;

    private float factor;
    //public Light2D light2d;

    private void Start()
    {
        player = GetComponentInParent<GrypsController>();
        rb = player.GetComponent<Rigidbody2D>();
        //light2d = GetComponentInChildren<Light2D>();
        factor = 1 / radius;
    }

    private void Update()
    {
        /*
        if (player.isRevup && isBurnOut && Mathf.Abs(rb.velocity.x) <= 20f)
        {
            BurnOutWheel(player.ConsumptionMemory());
        }
        else if (Mathf.Abs(rb.velocity.x) >= 1f)
        {

        }
         */
        if (isBurnOut && Mathf.Abs(rb.velocity.x) <= 20f)
        {
            BurnOutWheel(player.stageCrl.controlScreenMg.gearNum);
        }
        if (Mathf.Abs(rb.velocity.x) >= 1f)
        {
            BoostWheel();
        }
    }

    public void BurnOutWheel(int gearNum)
    {
        //float intensity = boostMemory * fac;
        transform.Rotate(0f, 0f, -1 * burnPower[gearNum] * Time.deltaTime);
        //light
    }

    public void BoostWheel()
    {
        var translation = rb.velocity * Time.deltaTime; // 位置の変化量
        var distance = translation.magnitude; // 移動した距離
        var angle = distance * factor; // (distance / circleCollider.radius) 球が回転するべき量 
        transform.Rotate(0f, 0f, -1 * angle * Mathf.Rad2Deg);
    }

}