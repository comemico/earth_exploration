using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SaltoAreaManager : MonoBehaviour
{
    public float flightDuration = 3.0f;

    GrypsController grypsCrl;

    /*
    [Header("“üŒû•ûŒü‚ÆN“ü‚ÌŒü‚«")]
    public ENTRANCE_KEY entranceKey;
    public enum ENTRANCE_KEY
    {
        [InspectorName("¶F1")] left = 1,
        [InspectorName("‰EF-1")] right = -1,
    }
     */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            grypsCrl.ForceDash((int)grypsCrl.transform.localScale.x, 0);
            grypsCrl.stageCrl.saltoMg.SaltoStart(flightDuration);
        }
    }

}
