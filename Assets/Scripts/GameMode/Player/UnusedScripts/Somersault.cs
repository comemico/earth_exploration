using UnityEngine;

public class Somersault : MonoBehaviour
{
    public float targetAngle;
    private Rigidbody2D rb;
    private Quaternion prev;
    private float myAngle;
    private int resultAngle;
    private Vector2 targetVector;
    private Vector2 nowVector;
    private bool isSomersault = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//ターゲットの角度→ベクター
    }

    void FixedUpdate()
    {
        if (isSomersault)
        {
            Vector2 prevvec = prev * Vector2.up;
            Vector2 nowvec = transform.rotation * Vector2.up;
            float angle = Vector2.Angle(prevvec, nowvec);
            myAngle += angle;
            prev = transform.rotation;

            if (myAngle >= resultAngle)
            {
                myAngle = 0f;
                rb.angularVelocity = 0f;
                transform.localEulerAngles = new Vector3(0f, 0f, targetAngle);
                Debug.Log("サマーソルト！");
                isSomersault = false;
            }
        }
    }

    public void SomerSault(float angularChangeInDegrees)
    {
        if (!isSomersault)
        {
            nowVector = transform.rotation * Vector2.up;//今の向きのベクター
            resultAngle = (int)Vector2.Angle(nowVector, targetVector);
            Debug.Log(resultAngle);
            resultAngle = 360 - resultAngle;

            float impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;

            rb.AddTorque(impulse, ForceMode2D.Impulse);

            isSomersault = true;
        }
    }

}
