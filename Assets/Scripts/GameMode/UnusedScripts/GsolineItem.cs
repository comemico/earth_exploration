using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GsolineItem : MonoBehaviour
{
    [Header("加算する燃料")] public float gasolineAmount;
    [Header("加算するボーナス燃料値")] public float bonusAmount;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("ガソリンゲージ")] public GasolineGauge gasolineGauge;
    [Header("UI用スクリプト")] public EnergyballManager energyballManager;
    [Header("通常エフェクト")] public GameObject normal;
    [Header("パワーアップエフェクト")] public GameObject powerUp;
    [Header("消滅エフェクト")] public GameObject disAppear;

    private bool isChangeItem;
    private float combo_itmer;

    void Update()
    {

        if (playerCheck.isOn)
        {
            if (GManager.instance != null)
            {
                gasolineGauge.GetGasolineItem(gasolineAmount, bonusAmount);
                energyballManager.energyballCurrent++;
                normal.SetActive(false);
                disAppear.SetActive(true);
                Destroy(this.gameObject, 1f);

            }

        }
    }
    public void PowerUpEnergy()
    {

    }
}
