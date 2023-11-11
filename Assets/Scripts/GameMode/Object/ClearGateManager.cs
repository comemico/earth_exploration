using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearGateManager : MonoBehaviour
{
    public enum GATE_KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("←左")] left = 1,
        [InspectorName("右→")] right = -1,
    }
    GATE_KEY gateKey;//GATE_KEY要素から１つ格納されている


    public enum SUCTION
    {
        [InspectorName("弱")] week = 0,
        [InspectorName("中")] mid,
        [InspectorName("強")] strong
    }
    [Header("吸引力")] public SUCTION suctionPow;

    public SpriteMask right, left;

    [Header("Flag")]
    public Transform mark;
    public SpriteRenderer[] emiSprite;
    public float raiseDuration;
    public Ease raiseType;
    public float fadeDuration;
    public Ease fadeType;

    GrypsController grypsCrl;

    const int DISTANCE_SUCKEDIN = 5;

    private void Start()
    {
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

            if (Enum.IsDefined(typeof(GATE_KEY), gateKey))// 事前に定義が存在するかどうか確認する
            {
                FalseMask((GATE_KEY)gateKey);//速度の正負(1,-1)からenum 型への変換 => 引数に入れる
            }

            grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);

            grypsCrl.rb.velocity = Vector2.zero;

            grypsCrl.transform.DOMoveX(gateKey * DISTANCE_SUCKEDIN, grypsCrl.grypsParameter.suctionPower[(int)suctionPow]).SetUpdate(false).SetRelative(true)
            .OnComplete(() =>
            {
                RaiseFlag();
            });

        }
    }

    public void RaiseFlag()
    {
        SwichBloom(true, fadeDuration);

        Sequence seq_raise = DOTween.Sequence();
        seq_raise.Append(mark.DOLocalRotate(Vector3.zero, raiseDuration, RotateMode.Fast).SetEase(raiseType));
        seq_raise.AppendInterval(0.15f);
        seq_raise.AppendCallback(() => grypsCrl.stageCrl.resultMg.Result(ResultManager.CAUSE.clear));
    }

    public void SwichBloom(bool isEnabled, float fadeTime) //FadeInする場合、PanelAnimeで光源が見えるのを防ぐ目的
    {
        foreach (SpriteRenderer sprite in emiSprite)
        {
            sprite.enabled = isEnabled;
            sprite.DOKill(true);
            sprite.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(fadeType);
        }
    }

    /*
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
     */



}
