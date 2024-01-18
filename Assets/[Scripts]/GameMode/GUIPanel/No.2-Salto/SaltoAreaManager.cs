using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SaltoAreaManager : MonoBehaviour
{
    public float flightDuration;

    public float runUpSpeed;

    [Header("Zoom: Camera.Size")]
    public int size;

    [Header("“üŒû•ûŒü‚ÆN“ü‚ÌŒü‚«")]
    public ENTRANCE_KEY entranceKey;
    public enum ENTRANCE_KEY
    {
        [InspectorName("¶F1")] left = 1,
        [InspectorName("‰EF-1")] right = -1,
    }
    GrypsController grypsCrl;
    bool isSalto;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            grypsCrl.stageCrl.saltoMg.SaltoStart(flightDuration);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        {
            // isSalto = true;
            // if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl && (int)entranceKey == (int)grypsCrl.transform.localScale.x && !isSalto)//N“ü‚ÌŒü‚«‚Å‚ ‚éê‡
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // isSalto = false;
        }
    }

}
