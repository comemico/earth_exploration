using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;


public class WarpAreaManager : MonoBehaviour
{

    [Header("ì¸å˚å¸Ç´")] public ENTRANCE_KEY key;

    [Header("ÉèÅ[ÉvÇÃà⁄ìÆêÊ")] public GameObject destination;

    GameObject right, left;

    GrypsController grypsCrl;

    public enum ENTRANCE_KEY
    {
        [InspectorName("ç∂")] left = 1,
        [InspectorName("âE")] right = -1,
    }

    private void Start()
    {

        right = transform.GetChild(0).gameObject;
        left = transform.GetChild(1).gameObject;

        switch (key)
        {
            case ENTRANCE_KEY.left:
                right.SetActive(true);
                left.SetActive(false);
                break;
            case ENTRANCE_KEY.right:
                right.SetActive(false);
                left.SetActive(true);
                break;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            Debug.Log("Trigger");
            //grypsCrl.isFreeze = true;
            //grypsCrl.rb.velocity = Vector2.zero;
            grypsCrl.stageCrl.ChangeToUncontrol();
            /*
            grypsCrl.transform.DOMoveX((int)key * 15f, 0.35f).SetRelative(true)
                .OnComplete(() =>
                {
                    Debug.Log("DoneMove");

                    //grypsCrl.EnterWarp(destination.transform);
                    //int floorNum = GetComponentInParent<FloorManager>().ActiveFloor(destination.transform.parent.gameObject);
                    //Camera.main.GetComponent<CinemachineController>().ToFloorVcam(floorNum, destination.transform);
                });
             */
            /*
            StartCoroutine(DelayMethod(0.3f, () =>
            {
            }));
             */
        }
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
