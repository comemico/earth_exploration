using UnityEngine;
using UnityEngine.UI;

public class AddForce : MonoBehaviour
{

    [Header("—Í")] [Range(0, 1500)] public float power;

    Rigidbody2D rb;
    public Button buttonForce;
    public Slider sliderAngle;

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, (int)sliderAngle.value);
        rb = GetComponent<Rigidbody2D>();
        sliderAngle.onValueChanged.AddListener(Hoge);
    }

    public void AddForceD()
    {
        buttonForce.interactable = false;

        float rad = sliderAngle.value * Mathf.Deg2Rad;
        Vector2 directionM = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector2 diractionR = transform.right;

        Debug.Log(directionM);
        Debug.Log(diractionR);

        Vector2 force = transform.localScale.x * directionM * power;

        rb.AddForce(force);
    }

    public void ResetPosition()
    {
        buttonForce.interactable = true;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = Vector3.zero;
        sliderAngle.value = 0f;
    }

    void Hoge(float value)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, (int)value);
    }

}
