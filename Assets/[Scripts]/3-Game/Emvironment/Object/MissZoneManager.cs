using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissZoneManager : MonoBehaviour
{
    GrypsController grypsCrl;

    private void OnTriggerEnter2D(Collider2D collision)//Goal用
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            Debug.Log("fall");
            grypsCrl.stageCrl.resultMg.OpenMissPanel(ResultManager.CAUSE.落下してしまった);
            //            grypsCrl.stageCrl.resultMg.Result(ResultManager.CAUSE.missFall);
            grypsCrl.rb.velocity = Vector2.zero;
            //grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
            if (grypsCrl.stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)
            {
                //grypsCrl.stageCrl.resultMg.Result(ResultManager.CAUSE.missBomb);

                //grypsCrl.stageCrl.pauseMg.push_Pause.interactable = false;
                //stageCrl => result(RESULT.miss)
                //FalseMask(gateKey);
                //gateCollider.enabled = false;
                //grypsCrl.transform.DOMoveX((int)gateKey * DISTANCE_SUCKEDIN, grypsCrl.grypsParameter.suctionPower[(int)suctionPow]).SetUpdate(false).SetRelative(true).OnComplete(() => CloseHole());
            }
        }
    }


}
