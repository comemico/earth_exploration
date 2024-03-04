using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGuide : MonoBehaviour
{

    [Header("ÉKÉCÉhÇÃèáî‘")]
    [Range(0, 10)] public int guideNumber;

    GrypsController grypsCrl;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            grypsCrl.stageCrl.guideMg.OpenGuide(guideNumber);
            grypsCrl.stageCrl.controlScreenMg.ProduceMemory(grypsCrl.stageCrl.data.maxLifeNum - grypsCrl.stageCrl.memoryGageMg.memoryGage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
        }
    }

}
