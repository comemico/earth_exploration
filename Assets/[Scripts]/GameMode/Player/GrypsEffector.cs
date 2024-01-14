using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class GrypsEffector : MonoBehaviour
{
    //��ɃX�v���C�g�̓����Ɋւ��X�N���v�g

    // �A�j���[�V�����A�G�t�F�N�g�A�����v(�ځA�W�F�b�g)�A�\�[�g�؂�ւ��̊Ǘ����s�Ȃ��ꏊ

    [Header("HeadLamp")]
    public Renderer headLamp;

    [Header("Turn")]
    [Range(0f, 1f)] public float turnTime = 0.25f;
    public Ease turnType = Ease.OutSine;

    [Header("Jet")]
    public Animator animatorJet;
    public Renderer jetLamp;

    [Header("Salto")]
    public Animator animatorSalto;
    [Space(10)]

    [Range(0f, 1f)] public float saltoTime = 0.3f;
    public Ease saltoType = Ease.OutSine;


    [Header("Sorting")]
    [HideInInspector] public SortingGroup sortingGroup;
    private GrypsController grypsCrl;


    private void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
        grypsCrl = GetComponentInParent<GrypsController>();
    }

    public void HeadLamp()
    {

    }

    public void Turn(float distanceHeight)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - distanceHeight, transform.position.z);
        transform.DOKill(true);
        transform.DOLocalMoveY(0f, turnTime).SetEase(turnType);
    }

    public void Salto()
    {
        transform.DOKill(true);
        transform.DOLocalRotate(new Vector3(0, 0, 360f), saltoTime, RotateMode.FastBeyond360).SetEase(saltoType).OnComplete(grypsCrl.stageCrl.saltoMg.SaltoComplete);

        /*
        if ((int)transform.localScale.x == 1)//�E�����Ă���
        {
            tween_salto.Kill(true);
            tween_salto = transform.DORotate(new Vector3(0, 0, 360f - targetAngle), 0.25f).SetRelative(true).SetUpdate(false).SetEase(Ease.OutSine);
        }
        else if ((int)transform.localScale.x == -1)//�������Ă���
        {
            tween_salto.Kill(true);
            tween_salto = transform.DOLocalRotate(new Vector3(0, 0, 360f + targetAngle), 0.25f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.OutSine);
        }
        */
    }


}
