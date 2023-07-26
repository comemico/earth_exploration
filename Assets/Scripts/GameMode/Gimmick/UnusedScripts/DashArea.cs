using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashArea : MonoBehaviour
{
    [Header("パワー")] public int force;
    [Header("ローテーションリミッタースイッチ")] public bool isLimitter;
    //[Header("スクリプト")] public GrypsManager player;
    [Header("スクリプト")] public PlayerTriggerCheck playerTriggerCheck;
    [Header("ローテーションリミッター")] public RotationLimitter rotationLimitter;

    private bool on = false;//二重防止

    private void Update()
    {
        if (playerTriggerCheck.isOn && !on)
        {
            //player.DashPanel(force);
            if (rotationLimitter != null)
            {
                rotationLimitter.enabled = isLimitter;
            }

        }
    }

}
