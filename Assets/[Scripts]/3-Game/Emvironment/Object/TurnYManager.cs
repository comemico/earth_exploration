using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class TurnYManager : MonoBehaviour
{
    [Header("�ړI�ʒu")]
    public TurnYManager destination;

    ENTRANCE_KEY entranceKey; //�N�����̑��x�̌���
    public enum ENTRANCE_KEY
    {
        [InspectorName("���F1")] left = 1,
        [InspectorName("�E�F-1")] right = -1,
    }

    float distanceHeight; // ( �ړI�ʒu - ���݈ʒu )�̋���
    [HideInInspector] public bool isTurn;

    GrypsController grypsCrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();

            if (Enum.IsDefined(typeof(ENTRANCE_KEY), (int)Mathf.Sign(grypsCrl.rb.velocity.x))) //���O�ɒ�`�����݂��邩�ǂ����m�F���� (ENTRANCE_KEY��1��-1�����邩�ǂ���)
            {
                entranceKey = (ENTRANCE_KEY)(int)Mathf.Sign(grypsCrl.rb.velocity.x);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ((int)entranceKey != (int)grypsCrl.transform.localScale.x && !isTurn) //�N�����̌�������ς�����ꍇ && Turn�͏o��܂ň��݂̂ɂ���
            {
                GetComponentInParent<FloorManager>().TurnFloor(destination.transform.parent);

                distanceHeight = destination.transform.parent.parent.position.y - transform.parent.parent.position.y;

                grypsCrl.transform.rotation = Quaternion.Euler(Vector3.zero);
                grypsCrl.transform.position = new Vector3(grypsCrl.transform.position.x, grypsCrl.transform.position.y + distanceHeight, grypsCrl.transform.position.z);

                grypsCrl.effector.sortingGroup.sortingOrder = destination.transform.parent.GetComponent<SortingGroup>().sortingOrder + 1;
                grypsCrl.effector.Turn(distanceHeight);

                isTurn = true; //���񂵂Ă��^�[���ł���悤�ɂ��邽��
                destination.isTurn = true; //�ړI�ʒu����TurnYManager��Turn����Ȃ��悤��
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTurn = false; //���񂵂Ă��^�[���ł���悤�ɂ��邽��
            destination.isTurn = false; //�ړI�ʒu����TurnYManager��Turn����Ȃ��悤��
        }
    }

}
