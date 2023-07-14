using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GasolineGauge : MonoBehaviour
{
    [Header("このステージの最大燃料値")] [SerializeField] public float maxGasoline;

    [Header("ガソリンゲージimage（手前）")] [SerializeField] public Image GreenGauge;
    [Header("ガソリンゲージimage（奥）")] [SerializeField] public Image RedGauge;

    [HideInInspector] public float gasolineValue = 0f;
    private float oldGasolineValue = 0f;
    private Tween redGaugeTween;

    private float timer;
    public bool isGetItem;
    [Header("最終燃料値(保存用)")] [SerializeField] public float gasolineAmount;
    [Header("ボーナス燃料値(保存用)")] public float bonusAmount;
    private int bonusCount;//連続何個ゲットしたか


    private void Start()
    {
        gasolineValue = maxGasoline;
        oldGasolineValue = maxGasoline;
        //GManager.instance.maxGasoline = maxGasoline;
        //GManager.instance.gasolineValue = maxGasoline;
    }
    void Update()
    {
        if (oldGasolineValue != gasolineValue)
        {
            //GreenGauge.fillAmount = gasolineValue / maxGasoline;
            GaugeReduction(0.7f);
            oldGasolineValue = gasolineValue;
        }

        if (isGetItem)
        {
            if (timer >= 1f)
            {
                timer = 0f;
                isGetItem = false;
                bonusCount = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }

        }

    }
    public void GaugeReduction(float time = 1f)
    {
        var valueFrom = oldGasolineValue / maxGasoline;
        var valueTo = gasolineValue / maxGasoline;


        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
            Debug.Log("Tween.Kill");
        }

        //奥ゲージ減少
        redGaugeTween = DOTween.To(() => valueFrom,
            x =>
            {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
            );

        //手前ゲージ減少
        //GreenGauge.fillAmount = valueTo;

    }

    public void GetGasolineItem(float gasolineAmount, float bonusAmount)
    {
        if (isGetItem && bonusCount >= 2)
        {
            this.gasolineAmount = gasolineAmount + bonusAmount;
            //色を変える処理
        }
        else if (!isGetItem)
        {
            this.gasolineAmount = gasolineAmount;
        }
        //GManager.instance.AddHeartNum(this.gasolineAmount);
        gasolineValue += this.gasolineAmount;
        if (gasolineValue > maxGasoline)
        {
            gasolineValue = maxGasoline;
        }

        isGetItem = true;
        timer = 0f;
        bonusCount++;

    }

}
