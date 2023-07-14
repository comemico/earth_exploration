using UnityEngine;

public class AccelerationAreaManager : MonoBehaviour
{
    [Header("�_�b�V�����x��")] public DashPower dashPower;
    [Header("���������̗L��")] public bool directionLimit;

    public enum DashPower
    {
        [InspectorName("1�i�K")] one = 0,
        [InspectorName("2�i�K")] two,
        [InspectorName("3�i�K")] three
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<GrypsController>().Dash((int)dashPower, directionLimit, (int)transform.localScale.x);
        }
    }
}
