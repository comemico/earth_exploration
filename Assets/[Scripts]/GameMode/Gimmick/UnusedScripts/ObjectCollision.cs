using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{

    /// <summary>
    /// このオブジェクトをプレイヤーが踏んだかどうか
    /// </summary>
    [HideInInspector] public bool playerStepOn;

    private string fallFloorTag = "FallFloor";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool fallFloor = (collision.collider.tag == fallFloorTag);

        if (fallFloor)
        {

        }
    }
}
