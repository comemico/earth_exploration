using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateManager : MonoBehaviour
{
    [Header("�o����")] public GATE_KEY gateKey;

    [Header("�z����")] public SUCTION suctionPow;

    [Header("�p�x")] [Range(0, 90)] public int angle;

    [Header("---�C�[�W���O: ����----")]
    [Header("�I���l")]
    public float endValue;
    [Header("���")]
    public Ease easeTypeOpen, easeTypeClose;
    [Header("����")]
    public float easeDuration;

    public enum GATE_KEY
    {
        [InspectorName("����")] both = 0,
        [InspectorName("����")] left = 1,
        [InspectorName("�E��")] right = -1,
    }
    public enum SUCTION
    {
        [InspectorName("��")] week = 0,
        [InspectorName("��")] mid,
        [InspectorName("��")] strong
    }

    SpriteMask right, left;
    Transform rock;
    GrypsController grypsCrl;
    Collider2D gateCollider;

    const int DISTANCE_SUCKEDIN = 15;
    const int DISTANCE_GATE = 10;
    const float APPEARENCE_HEIGHT = 1f;

    private void Start()
    {
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
        Camera.main.GetComponent<CinemachineController>().ChangeDirection(-1 * (int)gateKey);
        rock.transform.localScale = Vector3.zero;
        gryps.transform.position = new Vector3(transform.position.x + (DISTANCE_GATE * (int)gateKey), transform.position.y - APPEARENCE_HEIGHT, transform.position.z);
        gryps.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        gryps.transform.localScale = new Vector3(-1 * (int)gateKey, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerExit2D(Collider2D collision)//start�p�@
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
                grypsCrl.stageCrl.ChangeToControl();//���n��N������悤�ɂ���
                FalseMask(GATE_KEY.both);
                CloseHole();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//Goal�p
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
                grypsCrl.stageCrl.ChangeToUncontrol();
                grypsCrl.rb.velocity = Vector2.zero;
                grypsCrl.transform.DOMoveX((int)gateKey * DISTANCE_SUCKEDIN, grypsCrl.grypsParameter.suctionPower[(int)suctionPow]).SetRelative(true).OnComplete(() => CloseHole());
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
        //.OnComplete(() => this.gameObject.SetActive(false));
    }

}