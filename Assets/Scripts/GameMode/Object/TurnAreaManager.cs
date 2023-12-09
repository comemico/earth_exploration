using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnAreaManager : MonoBehaviour
{
    [Header("�^�[����̈ʒu")]
    public TurnAreaManager destination;

    [Header("Tween : Sprite.Move")]
    [Range(0f, 0.5f)] public float easeDuration = 0.2f;
    public Ease easeType = Ease.OutSine;

    GrypsController grypsCrl;
    [HideInInspector] public bool isTurn;
    float distanceHeight;

    public enum ENTRANCE_KEY
    {
        [InspectorName("���F1")] left = 1,
        [InspectorName("�E�F-1")] right = -1,
    }
    [Header("�N�����̑��x�̌���")]
    public ENTRANCE_KEY entranceKey;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            if (Enum.IsDefined(typeof(ENTRANCE_KEY), (int)Mathf.Sign(grypsCrl.rb.velocity.x)))// ���O�ɒ�`�����݂��邩�ǂ����m�F����
            {
                entranceKey = (ENTRANCE_KEY)(int)Mathf.Sign(grypsCrl.rb.velocity.x);
            }
        }
        /*
         */
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ((int)entranceKey != (int)grypsCrl.transform.localScale.x && !isTurn)//�N�����̌�������ς�����ꍇ
            {
                distanceHeight = grypsCrl.transform.position.y - destination.transform.position.y;
                //grypsCrl.TurnCorner(distanceHeight, distanceMoving);

                grypsCrl.transform.GetChild(0).position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y + distanceHeight, grypsCrl.transform.position.z);
                grypsCrl.transform.GetChild(0).DOLocalMoveY(0f, easeDuration).SetEase(easeType);


                grypsCrl.transform.rotation = Quaternion.Euler(Vector3.zero);
                grypsCrl.transform.position = new Vector3(grypsCrl.transform.position.x, destination.transform.position.y, grypsCrl.transform.position.z);
                GetComponentInParent<FloorManager>().TurnFloor(destination.transform.parent);

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
