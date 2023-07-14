using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerCheck : MonoBehaviour
{

    [HideInInspector] public bool isOn;//範囲に入っているか
    [HideInInspector] public bool isStay;//範囲中
    [HideInInspector] public bool isExitOnce;//範囲から出たか

    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag && !isOn)
        {
            isOn = true;
            isExitOnce = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == playerTag && isOn)
        {
            isOn = false;
            isExitOnce = true;
            isStay = false;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            isStay = true;
        }
    }


}
