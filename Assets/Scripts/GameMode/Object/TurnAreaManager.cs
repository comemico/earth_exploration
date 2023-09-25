using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnAreaManager : MonoBehaviour
{
    [Header("ターン後の位置")]
    public TurnAreaManager destination;

    [Header("入口方向と侵入時の向き")]
    public ENTRANCE_KEY entranceKey;

    GrypsController grypsCrl;
    [HideInInspector] public bool isTurn;
    float distanceHeight;
    //public float distanceMoving;

    [Header("Tween : Sprite.Move")]
    [Range(0f, 0.5f)] public float easeDuration;
    public Ease easeType;


    public enum ENTRANCE_KEY
    {
        [InspectorName("左：1")] left = 1,
        [InspectorName("右：-1")] right = -1,
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ((int)entranceKey != (int)grypsCrl.transform.localScale.x && !isTurn)//侵入時の向きから変わった場合
            {
                distanceHeight = grypsCrl.transform.position.y - destination.transform.position.y;
                //grypsCrl.TurnCorner(distanceHeight, distanceMoving);

                grypsCrl.transform.GetChild(0).position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y + distanceHeight, grypsCrl.transform.position.z);
                grypsCrl.transform.GetChild(0).DOLocalMoveY(0f, easeDuration).SetEase(easeType);


                grypsCrl.transform.rotation = Quaternion.Euler(Vector3.zero);
                grypsCrl.transform.position = new Vector3(grypsCrl.transform.position.x, destination.transform.position.y, grypsCrl.transform.position.z);
                GetComponentInParent<FloorManager>().TurnFloor(destination.transform.parent.parent.transform);

                destination.isTurn = true;
                isTurn = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTurn = false;
        }
    }


}
