using UnityEngine;

public class AccelerationAreaManager : MonoBehaviour
{
    [Header("ダッシュレベル")] public DashPower dashPower;
    [Header("方向制限の有無")] public bool directionLimit;

    public enum DashPower
    {
        [InspectorName("1段階")] one = 0,
        [InspectorName("2段階")] two,
        [InspectorName("3段階")] three
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<GrypsController>().Dash((int)dashPower, directionLimit, (int)transform.localScale.x);
        }
    }
}
