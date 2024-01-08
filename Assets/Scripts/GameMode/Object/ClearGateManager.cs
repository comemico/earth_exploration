using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class ClearGateManager : MonoBehaviour
{
    public enum GATE_KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("←左")] left = 1,
        [InspectorName("右→")] right = -1,
    }

    //GATE_KEY gateKey;//GATE_KEY要素から１つ格納されている

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
    CinemachineManager cinemachineCrl;
    BoxCollider2D boxCol;
    const int DISTANCE_SUCKEDIN = 6;

    private void Start()
    {
        FalseMask(GATE_KEY.both);
        cinemachineCrl = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
        boxCol = GetComponent<BoxCollider2D>();
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
                cinemachineCrl.ClearDirection(0.43f);
                break;

            case GATE_KEY.right:
                left.enabled = true;
                right.enabled = false;
                cinemachineCrl.ClearDirection(0.08f);
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

            boxCol.enabled = false; //1回しか呼ばれなくなる ・高速で侵入すると2回以上呼ばれてDOMoveXが正の方向にしか行かなくなる (Sign(0)で1になるため)
            transform.GetComponentInParent<SortingGroup>().enabled = false; //SpriteMaskの効果を出すため

            grypsCrl.stageCrl.StageClear();
            grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);

            grypsCrl.rb.velocity = Vector2.zero;
            grypsCrl.rb.constraints = RigidbodyConstraints2D.FreezePositionX;

            grypsCrl.stageCrl.pauseMg.push_Pause.interactable = false;
            grypsCrl.stageCrl.jetMg.limitRingCanGrp.DOFade(0f, 0.25f).SetDelay(0.1f);
            grypsCrl.stageCrl.jetMg.jetGuiMg.ShutDownJetHud();
            grypsCrl.stageCrl.memoryGageMg.DisplayMemoryGage(grypsCrl.stageCrl.data.maxLifeNum);

            grypsCrl.transform.DOMoveX((gateKey * DISTANCE_SUCKEDIN) + this.transform.position.x, grypsCrl.grypsParameter.suctionPower[(int)suctionPow]).SetUpdate(true).OnComplete(() => RaiseFlag());
        }
    }

    public void RaiseFlag()
    {
        SwichBloom(true, fadeDuration);

        Sequence seq_raise = DOTween.Sequence();
        seq_raise.Append(mark.DOLocalRotate(Vector3.zero, raiseDuration, RotateMode.Fast).SetEase(raiseType));
        seq_raise.AppendCallback(() =>
        {
            grypsCrl.stageCrl.resultMg.Result(ResultManager.CAUSE.clear);
        });
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

}
