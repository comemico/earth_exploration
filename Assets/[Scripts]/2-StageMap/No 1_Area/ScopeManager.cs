using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScopeManager : MonoBehaviour
{
    [Header("Scope")]
    public RectTransform scopeOut;
    public RectTransform scopeIn;
    Image scopeImg;
    [Space(10)]

    public float initialScale = 6.5f;
    [Range(0.1f, 0.5f)] public float scaleDuration = 0.3f;
    public Ease scaleType = Ease.OutSine;
    [Range(0.1f, 0.5f)] public float fadeDuration = 0.25f;
    public Ease fadeType = Ease.Linear;
    [Range(1f, 10f)] public float loopDuration = 6.5f;

    Sequence S_Scope;
    Tween scopeLoop;

    private void OnDestroy() => scopeLoop.Kill(false);

    private void Start()
    {
        scopeImg = scopeOut.transform.GetComponent<Image>();
        scopeLoop = scopeIn.DOLocalRotate(new Vector3(0, 0, 360f), loopDuration, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true);
        InitializedScope();
    }
    private void Update()
    {
        scopeOut.transform.rotation = scopeIn.transform.rotation;
    }

    private void InitializedScope()
    {
        scopeImg.color = new Color(1, 1, 1, 0);
        scopeOut.transform.localScale = new Vector3(initialScale, initialScale, 1f);
        //scopeOut.transform.localEulerAngles = new Vector3(0f, 0f, -1 * (Mathf.Abs(scopeIn.localEulerAngles.z) + 65f));
    }

    public void CompleteScope()
    {
        S_Scope.Kill(true);
        InitializedScope();
    }

    public Sequence TargetScope()
    {
        S_Scope = DOTween.Sequence();
        S_Scope.Append(scopeOut.transform.DOScale(1f, scaleDuration).SetEase(scaleType));
        S_Scope.Join(scopeImg.DOFade(1f, fadeDuration).SetEase(fadeType));
        //s_scope.Append(scopeOut.transform.DOLocalRotate(new Vector3(0f, 0f, focusAmount), focusDuration, RotateMode.LocalAxisAdd).SetEase(focusType).SetRelative(true));
        return S_Scope;
    }



}
