using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GrypsEffector : MonoBehaviour
{
    //主にスプライトの動きに関わるスクリプト

    // アニメーション、エフェクト、ランプ(目、ジェット)、ソート切り替えの管理を行なう場所

    [Header("HeadLamp")]
    public SpriteRenderer headLamp;
    public Light2D underLamp;
    [Space(10)]

    [Range(0.1f, 1f)] public float onTime = 0.5f;
    public Ease onType = Ease.OutSine;
    [Range(0.1f, 1f)] public float offTime = 0.5f;
    public Ease offType = Ease.OutSine;
    [NamedArrayAttribute(new string[] { "Ready", "Play", "Lack" })]
    public Color[] lampColor;


    [Header("Salto")]
    public Animator animatorSalto;
    [Space(10)]
    [NamedArrayAttribute(new string[] { "1回転", "2回転", "3回転" })]
    [Range(0.2f, 0.75f)] public float[] saltoTime;
    public Ease saltoType = Ease.OutSine;


    [Header("Jet")]
    public Animator animatorJet;
    public Renderer jetLamp;


    [Header("Turn")]
    [Range(0f, 1f)] public float turnTime = 0.25f;
    public Ease turnType = Ease.OutSine;


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

    public void PowerOnLamp()
    {
        headLamp.DOFade(1f, onTime).SetEase(onType);
        DOTween.To(() => underLamp.intensity, x => underLamp.intensity = x, 1f, onTime).SetEase(onType);
    }

    public void PowerOffLamp()
    {
        headLamp.DOFade(0f, offTime).SetEase(offType);
        DOTween.To(() => underLamp.intensity, x => underLamp.intensity = x, 0f, offTime).SetEase(offType);
    }


    public void Turn(float distanceHeight)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - distanceHeight, transform.position.z);
        transform.DOKill(true);
        transform.DOLocalMoveY(0f, turnTime).SetEase(turnType);
    }


    public void Salto(int saltoNum)
    {
        transform.DOKill(true);

        transform.DOLocalRotate(new Vector3(0, 0, 360f), saltoTime[saltoNum], RotateMode.FastBeyond360).SetEase(saltoType).OnComplete(() =>
        {
            grypsCrl.stageCrl.saltoMg.SaltoComplete();
        });

        SoundManager.Instance.PlaySE(SESoundData.SE.Force_Salto);
        SoundManager.Instance.seAudioSource.pitch = 2.5f;
    }


}
