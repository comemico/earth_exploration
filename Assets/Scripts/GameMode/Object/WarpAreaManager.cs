using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;


public class WarpAreaManager : MonoBehaviour
{

    [Header("ワープの移動先")] public WarpAreaManager destination;

    [Header("入口向き")] public ENTRANCE_KEY key;

    [Header("飛び出る力")] [Range(0, 2)] public int dashPower;

    SpriteMask right, left;

    GrypsController grypsCrl;

    Collider2D warpCollider;

    public enum ENTRANCE_KEY
    {
        [InspectorName("左")] left = 1,
        [InspectorName("右")] right = -1,
    }

    private void Start()
    {
        warpCollider = GetComponent<Collider2D>();
        right = transform.GetChild(0).GetComponent<SpriteMask>();
        left = transform.GetChild(1).GetComponent<SpriteMask>();

        switch (key)
        {
            case ENTRANCE_KEY.left:
                right.enabled = true;
                left.enabled = false;
                break;
            case ENTRANCE_KEY.right:
                right.enabled = false;
                left.enabled = true;
                break;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl)
            {
                grypsCrl.stageCrl.ChangeToControl();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }
            if (grypsCrl.stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)
            {
                warpCollider.enabled = false;
                grypsCrl.stageCrl.ChangeToUncontrol();
                grypsCrl.rb.velocity = Vector2.zero;
                grypsCrl.transform.DOMoveX((int)key * 17, 0.35f).SetRelative(true)
                       .OnComplete(() =>
                       {
                           grypsCrl.transform.position = new Vector3(destination.transform.position.x + (10 * (int)destination.key), destination.transform.position.y + 2.5f, destination.transform.position.z);
                           grypsCrl.transform.localScale = new Vector3(-1 * (int)destination.key, transform.localScale.y, transform.localScale.z);
                           int floorNum = GetComponentInParent<FloorManager>().ActiveFloor(destination.transform.parent.parent.gameObject);
                           Camera.main.GetComponent<CinemachineController>().dashPower = destination.dashPower;
                           Camera.main.GetComponent<CinemachineController>().ToFloorVcam(floorNum, (int)destination.key * (-1));
                       });

            }


            /*
           StartCoroutine(DelayMethod(0.3f, () =>
           { }));
             */

        }
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
