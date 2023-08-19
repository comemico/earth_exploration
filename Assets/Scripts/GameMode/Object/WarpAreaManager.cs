using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class WarpAreaManager : MonoBehaviour
{

    [Header("ワープの移動先")] public WarpAreaManager destination;

    [Header("入口向き")] public ENTRANCE_KEY entranceKey;

    [Header("吸引力")] public SUCTION suctionPower;

    [Header("飛び出る力")] [Range(0, 2)] public int dashPower;

    public enum ENTRANCE_KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("左")] left = 1,
        [InspectorName("右")] right = -1,
    }
    public enum SUCTION
    {
        [InspectorName("弱")] zero = 0,
        [InspectorName("中")] one,
        [InspectorName("強")] two
    }

    SpriteMask right, left;

    GrypsController grypsCrl;

    Collider2D warpCollider;

    const int DISTANCE_SUCKEDIN = 15;
    const int DISTANCE_WARP = 10;
    private void Awake()
    {
        warpCollider = GetComponent<Collider2D>();
        right = transform.GetChild(0).GetComponent<SpriteMask>();
        left = transform.GetChild(1).GetComponent<SpriteMask>();
        FalseMask(ENTRANCE_KEY.both);
    }

    public void FalseMask(ENTRANCE_KEY entranceKey)
    {
        switch (entranceKey)
        {
            case ENTRANCE_KEY.both:
                right.enabled = false;
                left.enabled = false;
                break;
            case ENTRANCE_KEY.left:
                left.enabled = false;
                right.enabled = true;
                break;
            case ENTRANCE_KEY.right:
                left.enabled = true;
                right.enabled = false;
                break;
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
                FalseMask(entranceKey);
                warpCollider.enabled = false;
                grypsCrl.rb.velocity = Vector2.zero;
                //grypsCrl.stageCrl.pauseMg.push_Pause.interactable = false;
                grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
                grypsCrl.transform.DOMoveX((int)entranceKey * DISTANCE_SUCKEDIN, grypsCrl.grypsParameter.suctionPower[(int)suctionPower]).SetRelative(true).SetUpdate(false)
                       .OnComplete(() =>
                       {
                           destination.FalseMask(destination.entranceKey);
                           grypsCrl.transform.position = new Vector3(destination.transform.position.x + (DISTANCE_WARP * (int)destination.entranceKey), destination.transform.position.y + 2.5f, destination.transform.position.z);
                           grypsCrl.transform.localScale = new Vector3(-1 * (int)destination.entranceKey, transform.localScale.y, transform.localScale.z);

                           var floorNum = GetComponentInParent<FloorManager>().ActiveFloor(destination.transform.parent.parent.transform, -1 * (int)destination.entranceKey);
                           //Camera.main.GetComponent<CinemachineController>().ToFloorVcam(floorNum, (int)destination.entranceKey * (-1));

                           DOVirtual.DelayedCall(Camera.main.GetComponent<CinemachineController>().brain.m_CustomBlends.m_CustomBlends[floorNum].m_Blend.m_Time, () =>
                           {
                               grypsCrl.ForceDash((int)grypsCrl.transform.localScale.x, destination.dashPower);
                           }, false);
                       });
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl)
            {
                //grypsCrl.stageCrl.pauseMg.push_Pause.interactable = true;
                grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
                FalseMask(ENTRANCE_KEY.both);
            }
        }
    }

}
