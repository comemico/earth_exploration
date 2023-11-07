using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissZoneManager : MonoBehaviour
{
    GrypsController grypsCrl;

    private void OnTriggerEnter2D(Collider2D collision)//Goal—p
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            if (grypsCrl.stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)
            {
                grypsCrl.rb.velocity = Vector2.zero;
                grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
                grypsCrl.stageCrl.resultMg.Result(ResultManager.CAUSE.missZone);
                //grypsCrl.stageCrl.pauseMg.push_Pause.interactable = false;
                //stageCrl => result(RESULT.miss)
                //FalseMask(gateKey);
                //gateCollider.enabled = false;
                //grypsCrl.transform.DOMoveX((int)gateKey * DISTANCE_SUCKEDIN, grypsCrl.grypsParameter.suctionPower[(int)suctionPow]).SetUpdate(false).SetRelative(true).OnComplete(() => CloseHole());
            }
        }
    }


}
