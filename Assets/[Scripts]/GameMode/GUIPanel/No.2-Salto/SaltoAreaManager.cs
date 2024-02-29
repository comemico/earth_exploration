using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SaltoAreaManager : MonoBehaviour
{

    [Header("サルトモード時間")]
    public float flightDuration = 3.0f;

    GrypsController grypsCrl;


    /*
    [Header("入口方向と侵入時の向き")]
    public ENTRANCE_KEY entranceKey;
    public enum ENTRANCE_KEY
    {
        [InspectorName("左：1")] left = 1,
        [InspectorName("右：-1")] right = -1,
    }
     */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            grypsCrl.ForceDash((int)grypsCrl.transform.localScale.x, 0);
            grypsCrl.stageCrl.saltoMg.SaltoStart(flightDuration);

            grypsCrl.effector.trailNormal.emitting = false;
            grypsCrl.effector.trailAlpha.emitting = false;
        }
    }

}
