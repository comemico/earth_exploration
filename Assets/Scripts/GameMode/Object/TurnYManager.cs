using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class TurnYManager : MonoBehaviour
{
    [Header("ターン後の位置")]
    public TurnYManager destination;

    [Header("Tween : Sprite.Move")]
    [Range(0f, 10f)] public float easeDuration;
    public Ease easeType;

    public enum ENTRANCE_KEY
    {
        [InspectorName("左：1")] left = 1,
        [InspectorName("右：-1")] right = -1,
    }
    ENTRANCE_KEY entranceKey; //侵入時の速度の向き

    public float height = 0.5f;

    GrypsController grypsCrl;
    [HideInInspector] public bool isTurn;
    float distanceHeight;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            if (Enum.IsDefined(typeof(ENTRANCE_KEY), (int)Mathf.Sign(grypsCrl.rb.velocity.x)))// 事前に定義が存在するかどうか確認する
            {
                entranceKey = (ENTRANCE_KEY)(int)Mathf.Sign(grypsCrl.rb.velocity.x);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ((int)entranceKey != (int)grypsCrl.transform.localScale.x && !isTurn)//侵入時の向きから変わった場合
            {
                GetComponentInParent<FloorManager>().TurnFloor(destination.transform.parent);

                distanceHeight = destination.transform.parent.parent.position.y - transform.parent.parent.position.y;

                grypsCrl.transform.rotation = Quaternion.Euler(Vector3.zero);
                grypsCrl.transform.position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y + distanceHeight, grypsCrl.transform.position.z); // new Vector3(grypsCrl.transform.position.x, destination.transform.position.y, grypsCrl.transform.position.z);

                grypsCrl.transform.GetChild(0).position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y - distanceHeight, grypsCrl.transform.position.z);

                grypsCrl.sortingGroup.sortingOrder = 0;
                grypsCrl.transform.GetChild(0).DOKill(true);
                grypsCrl.transform.GetChild(0).DOLocalMoveY(0f, easeDuration).SetEase(easeType)
                    .OnComplete(() => grypsCrl.sortingGroup.sortingOrder = destination.transform.parent.GetComponent<SortingGroup>().sortingOrder + 1);

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
            destination.isTurn = false;
        }
    }

}
