using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownCheck : MonoBehaviour
{
    [Header("ダウン判定時間")] public float downTimeRange;
    private float timer;
    //private GrypsManager gryps;

    private string groundTag = "Ground"; //ただの床
    private string platformTag = "GroundPlatform";//すり抜ける床
    private string moveFloorTag = "MoveFloor";//動く床
    private string fallFloorTag = "FallFloor";//落ちる床
    private string elevatorTag = "Elevator";//エレベーター


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag || collision.tag == elevatorTag)
        {
            Debug.Log("stay中");
            if (timer >= downTimeRange)
            {
                //gryps = GetComponentInParent<GrypsManager>();
                //gryps.Down();

                timer = 0.0f;
            }
            else
            {
                timer += Time.deltaTime;
            }

        }

    }
}
