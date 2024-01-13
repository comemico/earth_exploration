using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopFloor : MonoBehaviour
{
    [Header("trueにするオブジェクト")] public EdgeCollider2D trueEdgeCollider2D;
    [Header("falseにするオブジェクト")] public EdgeCollider2D falseEdgeCollider2D;
    [Header("逆走用スイッチ")] public BoxCollider2D otherBoxCollider2D;
    private BoxCollider2D myBoxCollider2D;

    private void Start()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            trueEdgeCollider2D.enabled = true;
            falseEdgeCollider2D.enabled = false;
            myBoxCollider2D.enabled = false;
            otherBoxCollider2D.enabled = true;
        }
    }
}
