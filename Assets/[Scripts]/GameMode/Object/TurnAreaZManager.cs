using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class TurnAreaZManager : MonoBehaviour
{
    [Header("�^�[����̈ʒu")]
    public TurnAreaZManager destination;

    [Header("Tween : Sprite.Move")]
    [Range(0f, 10f)] public float easeDuration;
    public Ease easeType;

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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ((int)entranceKey != (int)grypsCrl.transform.localScale.x && !isTurn)//�N�����̌�������ς�����ꍇ
            {
                GetComponentInParent<FloorManager>().TurnFloor(destination.transform.parent);

                distanceHeight = grypsCrl.transform.position.z - destination.transform.parent.position.z;
                Debug.Log(distanceHeight);
                //.col��0 => 0.5 ,sprite�� => -0.5 (0 - 0.5 = -0.5 ) => (col - sprite = distance) => distance�l��sprite�ɓ\��t����
                //.col��0.5 => 0 ,sprite�� => +0.5 (0.5 - 0 = 0.5 )
                //sprite => 0��

                grypsCrl.transform.rotation = Quaternion.Euler(Vector3.zero);
                grypsCrl.transform.position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y, destination.transform.position.z); // new Vector3(grypsCrl.transform.position.x, destination.transform.position.y, grypsCrl.transform.position.z);

                grypsCrl.transform.GetChild(0).position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y, grypsCrl.transform.position.z + distanceHeight);

                grypsCrl.transform.GetChild(0).DOKill(true);
                grypsCrl.transform.GetChild(0).DOLocalMoveZ(0f, easeDuration).SetEase(easeType);

                grypsCrl.sortingGroup.sortingOrder = destination.transform.parent.GetComponent<SortingGroup>().sortingOrder;
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
