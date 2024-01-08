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
        [InspectorName("����")] both = 0,
        [InspectorName("����")] left = 1,
        [InspectorName("�E��")] right = -1,
    }

    //GATE_KEY gateKey;//GATE_KEY�v�f����P�i�[����Ă���

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

            boxCol.enabled = false; //1�񂵂��Ă΂�Ȃ��Ȃ� �E�����ŐN�������2��ȏ�Ă΂��DOMoveX�����̕����ɂ����s���Ȃ��Ȃ� (Sign(0)��1�ɂȂ邽��)
            transform.GetComponentInParent<SortingGroup>().enabled = false; //SpriteMask�̌��ʂ��o������

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

    public void SwichBloom(bool isEnabled, float fadeTime) //FadeIn����ꍇ�APanelAnime�Ō�����������̂�h���ړI
    {
        foreach (SpriteRenderer sprite in emiSprite)
        {
            sprite.enabled = isEnabled;
            sprite.DOKill(true);
            sprite.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(fadeType);
        }
    }

}
