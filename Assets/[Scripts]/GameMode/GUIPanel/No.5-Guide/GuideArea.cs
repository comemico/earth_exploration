using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArea : MonoBehaviour
{

    [Header("ƒKƒCƒh‚Ì‡”Ô")]
    [Range(0, 10)] public int guideNumber;

    [Header("‹@‘Ì‚Ì“®‚«‚ğ~‚ß‚é‚©")]
    public bool isStop = true;

    GrypsController grypsCrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            grypsCrl.stageCrl.guideMg.OpenGuide(guideNumber);
            if (isStop) grypsCrl.Stop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }

}
