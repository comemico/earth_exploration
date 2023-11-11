using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearGateManager : MonoBehaviour
{
    public enum GATE_KEY
    {
        [InspectorName("����")] both = 0,
        [InspectorName("����")] left = 1,
        [InspectorName("�E��")] right = -1,
    }
    GATE_KEY gateKey;//GATE_KEY�v�f����P�i�[����Ă���


    public enum SUCTION
    {
        [InspectorName("��")] week = 0,
        [InspectorName("��")] mid,
        [InspectorName("��")] strong
    }
    [Header("�z����")] public SUCTION suctionPow;

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

            if (Enum.IsDefined(typeof(GATE_KEY), gateKey))// ���O�ɒ�`�����݂��邩�ǂ����m�F����
            {
                FalseMask((GATE_KEY)gateKey);//���x�̐���(1,-1)����enum �^�ւ̕ϊ� => �����ɓ����
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

    public void SwichBloom(bool isEnabled, float fadeTime) //FadeIn����ꍇ�APanelAnime�Ō�����������̂�h���ړI
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
