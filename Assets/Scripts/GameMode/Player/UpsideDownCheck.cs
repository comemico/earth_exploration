using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownCheck : MonoBehaviour
{
    [Header("ダウン判定時間")] public float downTimeRange;
    private float timer;
    private GrypsController grypsCrl;

    private string groundTag = "Ground"; //ただの床
    private string platformTag = "GroundPlatform";//すり抜ける床
    private string moveFloorTag = "MoveFloor";//動く床
    private string fallFloorTag = "FallFloor";//落ちる床
    private string elevatorTag = "Elevator";//エレベーター

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timer = 0.0f;
        if (grypsCrl == null)
        {
            grypsCrl = GetComponentInParent<GrypsController>();
        }
        //0.5秒後に起動させれば継続してStay2Dが呼ばれ続ける
        /*
        if (grypsCrl.rb.IsSleeping())
        {
            grypsCrl.rb.WakeUp();
            Debug.Log("wakeUp");
        }
         */
    }

    private void OnTriggerStay2D(Collider2D collision)//Time To Sleep = 0.5f Sleep状態までの時間
    {
        if (collision.tag == groundTag || collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag || collision.tag == elevatorTag)
        {
            if (timer >= downTimeRange)
            {
                timer = 0.0f;
                grypsCrl.Return();
                //grypsCrl.rb.velocity = Vector2.zero;
                //grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
                //grypsCrl.stageCrl.resultMg.Result(ResultManager.RESULT.missZone);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
}
