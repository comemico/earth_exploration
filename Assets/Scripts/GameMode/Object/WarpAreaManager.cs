using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;


public class WarpAreaManager : MonoBehaviour
{

    [Header("ÉèÅ[ÉvÇÃà⁄ìÆêÊ")] public GameObject destination;

    //GrypsManager grypsMg;
    GrypsController grypsCrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            if (!grypsCrl.isFreeze)
            {
                grypsCrl.isFreeze = true;
                grypsCrl.transform.DOMoveX(this.gameObject.transform.position.x, 0.35f)
                    .OnComplete(() =>
                    {
                        grypsCrl.EnterWarp(destination.transform);
                        int floorNum = GetComponentInParent<FloorManager>().ActiveFloor(destination.transform.parent.gameObject);
                        Camera.main.GetComponent<CinemachineController>().ToFloorVcam(floorNum, destination.transform);
                    });
                /*
                StartCoroutine(DelayMethod(0.3f, () =>
                {
                }));
                 */
            }

        }
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
