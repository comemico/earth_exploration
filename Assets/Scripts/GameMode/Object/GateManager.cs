using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateManager : MonoBehaviour
{
    [Header("出入口")] public GATE_KEY gateKey;

    [Header("吸引力")] public SUCTION suctionPow;

    [Header("角度")] [Range(0, 45)] public int angle;

    [Header("---イージング: 鍵穴----")]
    [Header("終了値")]
    public float endValue;
    [Header("種類")]
    public Ease easeTypeOpen, easeTypeClose;
    [Header("時間")]
    public float easeDuration;


    public enum GATE_KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("←左")] left = 1,
        [InspectorName("右→")] right = -1,
    }
    public enum SUCTION
    {
        [InspectorName("弱")] week = 0,
        [InspectorName("中")] mid,
        [InspectorName("強")] strong
    }

    SpriteMask right, left;
    Transform rock;
    GrypsController grypsCrl;
    Collider2D gateCollider;
    FloorManager floorMg;

    const int DISTANCE_SUCKEDIN = 5;
    const int DISTANCE_GATE = 3;
    const float APPEARENCE_HEIGHT = 0f;

    private void Awake()
    {
        floorMg = GetComponentInParent<FloorManager>();
        gateCollider = GetComponent<Collider2D>();
        right = transform.GetChild(0).GetComponent<SpriteMask>();
        left = transform.GetChild(1).GetComponent<SpriteMask>();
        rock = transform.GetChild(2).GetComponent<Transform>();
        FalseMask(GATE_KEY.both);
    }

    public void FalseMask(GATE_KEY entranceKey)
    {
        switch (entranceKey)
        {
            case GATE_KEY.both:
                right.enabled = false;
                left.enabled = false;
                break;
            case GATE_KEY.left:
                left.enabled = false;
                right.enabled = true;
                break;
            case GATE_KEY.right:
                left.enabled = true;
                right.enabled = false;
                break;
        }
    }

    public void SetStartPosition(GameObject gryps)
    {
        FalseMask(gateKey);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        rock.transform.localScale = Vector3.zero;
        gryps.transform.position = new Vector3(transform.position.x + (DISTANCE_GATE * (int)gateKey), transform.position.y - APPEARENCE_HEIGHT, transform.position.z);
        gryps.transform.localRotation = Quaternion.Euler(0f, 0f, -1 * (int)gateKey * angle);
        gryps.transform.localScale = new Vector3(-1 * (int)gateKey, transform.localScale.y, transform.localScale.z);
        floorMg.ActiveFloor(transform.parent.transform, -1 * (int)gateKey);
    }

    private void OnTriggerExit2D(Collider2D collision)//start用　
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl)
            {
                gateCollider.enabled = false;
                FalseMask(GATE_KEY.both);
                CloseHole();
                //grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);//着地後起動するようにする
                grypsCrl.stageCrl.curtainMg.Hide();
                grypsCrl.stageCrl.pauseMg.push_Pause.interactable = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//Goal用
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null)
            {
                grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            }

            if (grypsCrl.stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)
            {
                FalseMask(gateKey);
                gateCollider.enabled = false;

                grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
                //grypsCrl.stageCrl.ResultA(ResultManager.RESULT.clear);

                //grypsCrl.stageCrl.pauseMg.push_Pause.interactable = false;
                grypsCrl.rb.velocity = Vector2.zero;
                grypsCrl.transform.DOMoveX((int)gateKey * DISTANCE_SUCKEDIN, grypsCrl.grypsParameter.suctionPower[(int)suctionPow]).SetUpdate(false).SetRelative(true)
                    .OnComplete(() =>
                    {
                        CloseHole();
                        grypsCrl.stageCrl.resultMg.Result(ResultManager.RESULT.clear);

                    });
            }
        }
    }

    public Tween OpenHole()
    {
        rock.DOComplete();
        Tween t = rock.DOScale(endValue, easeDuration).SetEase(easeTypeOpen).SetUpdate(false);
        return t;
    }

    public void CloseHole()
    {
        rock.DOComplete();
        rock.DOScale(Vector3.zero, easeDuration).SetEase(easeTypeClose);
    }

}
