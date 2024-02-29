using System;
using UnityEngine;
using DG.Tweening;

public class GateStopManager : MonoBehaviour
{
    [Header("吸引力")]
    public SUCTION suctionPow;
    public enum SUCTION
    {
        [InspectorName("弱")] week = 0,
        [InspectorName("中")] mid,
        [InspectorName("強")] strong
    }

    [Header("移動値")]
    public int distance = 6;

    public enum GATE_KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("←左")] left = 1,
        [InspectorName("右→")] right = -1,
    }
    //GATE_KEY gateKey;//GATE_KEY要素から１つ格納されている

    GrypsController grypsCrl;
    BoxCollider2D boxCol;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }


    /*
    Enumについての参考ページ
    .https://takap-tech.com/entry/2020/07/08/015033 ・C#の値から列挙型の名前を取得します
    .https://www.techiedelight.com/ja/get-enum-name-from-value-csharp/ ・文字列や数値から特定のEnumに変換する方法
     */
    //string entranceKey = Enum.GetName(typeof(GATE_KEY), (int)Mathf.Sign(grypsCrl.rb.velocity.x));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();

            int gateKey = (int)Mathf.Sign(grypsCrl.rb.velocity.x);

            Debug.Log("Enter");
            //boxCol.enabled = false; //1回しか呼ばれなくなる ・高速で侵入すると2回以上呼ばれてDOMoveXが正の方向にしか行かなくなる (Sign(0)で1になるため).
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
