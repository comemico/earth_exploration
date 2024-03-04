using System;
using UnityEngine;
using DG.Tweening;

public class GateStopManager : MonoBehaviour
{
    [Header("�z����")]
    public SUCTION suctionPow;
    public enum SUCTION
    {
        [InspectorName("��")] week = 0,
        [InspectorName("��")] mid,
        [InspectorName("��")] strong
    }

    [Header("�ړ��l")]
    public int distance = 6;

    public enum GATE_KEY
    {
        [InspectorName("����")] both = 0,
        [InspectorName("����")] left = 1,
        [InspectorName("�E��")] right = -1,
    }
    //GATE_KEY gateKey;//GATE_KEY�v�f����P�i�[����Ă���

    GrypsController grypsCrl;
    BoxCollider2D boxCol;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }


    /*
    Enum�ɂ��Ă̎Q�l�y�[�W
    .https://takap-tech.com/entry/2020/07/08/015033 �EC#�̒l����񋓌^�̖��O���擾���܂�
    .https://www.techiedelight.com/ja/get-enum-name-from-value-csharp/ �E������␔�l��������Enum�ɕϊ�������@
     */
    //string entranceKey = Enum.GetName(typeof(GATE_KEY), (int)Mathf.Sign(grypsCrl.rb.velocity.x));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();

            int gateKey = (int)Mathf.Sign(grypsCrl.rb.velocity.x);

            Debug.Log("Enter");
            //boxCol.enabled = false; //1�񂵂��Ă΂�Ȃ��Ȃ� �E�����ŐN�������2��ȏ�Ă΂��DOMoveX�����̕����ɂ����s���Ȃ��Ȃ� (Sign(0)��1�ɂȂ邽��).
            grypsCrl.Stop();
            //grypsCrl.transform.DOMoveX((gateKey * distance) + this.transform.position.x, grypsCrl.parameter.suctionPower[(int)suctionPow]).SetUpdate(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            Debug.Log("Exit");
            // boxCol.enabled = true;
        }
    }

}
