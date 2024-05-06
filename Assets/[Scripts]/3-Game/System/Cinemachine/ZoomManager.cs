using UnityEngine;
using DG.Tweening;

public class ZoomManager : MonoBehaviour
{
    [Header("Lens")]
    [Range(5f, 16f)] public float enterValue = 10f;
    [Range(0.25f, 3f)] public float enterTime = 1f;
    [Space(10)]

    [Range(6f, 15f)] public float exitValue = 10f;
    [Range(0.5f, 3f)] public float exitTime = 1f;

    public Ease lensType = Ease.OutQuad;


    CinemachineManager cinemachineMg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (cinemachineMg == null) cinemachineMg = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
            cinemachineMg.DOLensSize(enterValue, enterTime, lensType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (cinemachineMg == null)
            {
                Debug.Log("cinemachineMg��null�ł��B");
                //cinemachineMg = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
            }
            cinemachineMg.DOLensSize(exitValue, exitTime, lensType);
        }
    }

}
/*
 * �u�[�X�g
 * �^�[�{
 * �W�F�b�g
 * �_�b�V��
 */