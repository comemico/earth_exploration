using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class StartGateManager : MonoBehaviour
{
    public enum GATE_KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("←左")] left = 1,
        [InspectorName("右→")] right = -1,
    }
    [Header("発進方向")] public GATE_KEY gateKey;//GATE_KEY要素から１つ格納されている
    [Range(0, 2)] public int dashLevel;

    public SpriteMask left, right;

    [Header("Flag")]
    public Transform mark;
    public SpriteRenderer[] emiSprite;
    public float raiseDuration;
    public Ease raiseType;
    public float fadeDuration;
    public Ease fadeType;

    CinemachineController cinemachineCrl;
    BoxCollider2D boxCol;
    FloorManager floorMg;
    GrypsController grypsCrl;

    const int DISTANCE_GATE = 4;
    const int APPEARENCE_HEIGHT = 1;

    private void Awake()
    {
        cinemachineCrl = Camera.main.GetComponent<CinemachineController>();
        FalseMask(gateKey);
        floorMg = GetComponentInParent<FloorManager>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    public void SetStartPosition(GameObject gryps) //StageCrlから最初に呼ばれる・位置おき
    {
        grypsCrl = gryps.GetComponent<GrypsController>();
        floorMg.ActiveFloor(transform.parent.GetChild(0), -1 * (int)gateKey);
        gryps.transform.localScale = new Vector3(-1 * (int)gateKey, transform.localScale.y, transform.localScale.z);
        gryps.transform.position = new Vector3(transform.position.x + (DISTANCE_GATE * (int)gateKey), (transform.position.y - 5) + APPEARENCE_HEIGHT, transform.position.z);
        grypsCrl.rb.constraints = RigidbodyConstraints2D.None;
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
                cinemachineCrl.StartDirection(0.75f);
                break;

            case GATE_KEY.right:
                left.enabled = true;
                right.enabled = false;
                cinemachineCrl.StartDirection(0.1f);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//start用　
    {
        if (collision.tag == "Player")
        {
            boxCol.enabled = false;
            FalseMask(GATE_KEY.both);
            grypsCrl.stageCrl.pauseMg.push_Pause.interactable = true;

            grypsCrl.stageCrl.curtainMg.HideNameInfo();
        }
    }

    public void RaiseFlag()
    {
        SwichBloom(true, fadeDuration);

        Sequence seq_raise = DOTween.Sequence();
        seq_raise.Append(mark.DOLocalRotate(Vector3.zero, raiseDuration, RotateMode.Fast).SetEase(raiseType));
        seq_raise.AppendInterval(0.35f);
        seq_raise.AppendCallback(() =>
        {
            grypsCrl.ForceDash((int)grypsCrl.transform.localScale.x, dashLevel);
            grypsCrl.stageCrl.controlScreenMg.bowMg.canvasGroup.alpha = 0f;
        });
        seq_raise.AppendInterval(1.5f);
        seq_raise.AppendCallback(() => LowerFlag());
    }

    public void LowerFlag()
    {
        SwichBloom(false, fadeDuration);

        Sequence seq_lower = DOTween.Sequence();
        seq_lower.Append(mark.DOLocalRotate(new Vector3(0, 0, -90), fadeDuration, RotateMode.Fast).SetEase(fadeType));
    }

    public void SwichBloom(bool isEnabled, float fadeTime) //FadeInする場合、PanelAnimeで光源が見えるのを防ぐ目的
    {
        foreach (SpriteRenderer sprite in emiSprite)
        {
            //sprite.enabled = isEnabled;
            sprite.DOKill(true);
            sprite.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(fadeType);
        }
    }

}
