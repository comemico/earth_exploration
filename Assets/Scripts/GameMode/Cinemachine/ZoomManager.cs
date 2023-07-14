using UnityEngine;
using DG.Tweening;

public class ZoomManager : MonoBehaviour
{
    [Header("FieldOfView")] public int boxNum;
    [Header("Time")] public float time;
    [Header("イージングの種類")] public Ease easeType;

    private CinemachineManager cinemachineMg;

    void Start()
    {
        cinemachineMg = Camera.main.GetComponent<CinemachineManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cinemachineMg.ZoomCamera(boxNum, time, easeType);
        }

    }
}
