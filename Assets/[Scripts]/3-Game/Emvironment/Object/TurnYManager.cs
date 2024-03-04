using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class TurnYManager : MonoBehaviour
{
    [Header("目的位置")]
    public TurnYManager destination;

    ENTRANCE_KEY entranceKey; //侵入時の速度の向き
    public enum ENTRANCE_KEY
    {
        [InspectorName("左：1")] left = 1,
        [InspectorName("右：-1")] right = -1,
    }

    float distanceHeight; // ( 目的位置 - 現在位置 )の距離
    [HideInInspector] public bool isTurn;

    GrypsController grypsCrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();

            if (Enum.IsDefined(typeof(ENTRANCE_KEY), (int)Mathf.Sign(grypsCrl.rb.velocity.x))) //事前に定義が存在するかどうか確認する (ENTRANCE_KEYに1か-1があるかどうか)
            {
                entranceKey = (ENTRANCE_KEY)(int)Mathf.Sign(grypsCrl.rb.velocity.x);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ((int)entranceKey != (int)grypsCrl.transform.localScale.x && !isTurn) //侵入時の向きから変わった場合 && Turnは出るまで一回のみにする
            {
                GetComponentInParent<FloorManager>().TurnFloor(destination.transform.parent);

                distanceHeight = destination.transform.parent.parent.position.y - transform.parent.parent.position.y;

                grypsCrl.transform.rotation = Quaternion.Euler(Vector3.zero);
                grypsCrl.transform.position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y + distanceHeight, grypsCrl.transform.position.z);

                grypsCrl.effector.sortingGroup.sortingOrder = destination.transform.parent.GetComponent<SortingGroup>().sortingOrder + 1;
                grypsCrl.effector.Turn(distanceHeight);

                isTurn = true; //周回してもターンできるようにするため
                destination.isTurn = true; //目的位置側のTurnYManagerでTurnされないように
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTurn = false; //周回してもターンできるようにするため
            destination.isTurn = false; //目的位置側のTurnYManagerでTurnされないように
        }
    }

}
