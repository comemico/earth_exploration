using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SaltoAreaManager : MonoBehaviour
{
    public float flightDuration;

    public float runUpSpeed;

    [Header("Zoom: Camera.FOV")]
    public int fov;

    [Header("入口方向と侵入時の向き")]
    public ENTRANCE_KEY entranceKey;
    public enum ENTRANCE_KEY
    {
        [InspectorName("左：1")] left = 1,
        [InspectorName("右：-1")] right = -1,
    }
    GrypsController grypsCrl;
    bool isSalto;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (grypsCrl == null)
        {
            grypsCrl = collision.gameObject.GetComponent<GrypsController>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl && (int)entranceKey == (int)grypsCrl.transform.localScale.x && !isSalto)//侵入時の向きである場合
            {
                Camera.main.GetComponent<CinemachineManager>().Zoom(fov);
                grypsCrl.stageCrl.saltoMg.JugeSaltoMode(flightDuration);
                isSalto = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isSalto = false;
        }
    }

}
