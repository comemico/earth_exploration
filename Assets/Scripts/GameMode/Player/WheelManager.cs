using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WheelManager : MonoBehaviour
{

    [Header("タイヤをバーンナウトさせるか")] public bool isBurnOut;
    [Header("タイヤの直径")] public float radius; //CircleCollider2Dコンポーネントをアタッチして、Radiusを確認する
    [Header("回転力")] public float fac;
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
        if (Mathf.Abs(rb.velocity.x) >= 1f)
        {
            BoostWheel();
        }

    }

    private void BurnOutWheel(int boostMemory)
    {
        float intensity = boostMemory * fac;
        transform.Rotate(0f, 0f, -1 * intensity * Time.deltaTime);
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