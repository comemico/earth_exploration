using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartTextManager : MonoBehaviour
{
    /*
    private Text heartText;
    private int oldHeartNum = 0;

    [Header("点滅する時間")] public float respawnTime;
    [Header("点滅開始のトリガー")] private bool isContinue;
    [Header("点滅時、0～respawnTimeまで入れる変数")] private float respawnbox;
    [Header("点滅時、0～0.2をサイクルする変数")] private float blinkTime;
    void Start()
    {
        heartText = GetComponent<Text>();
        heartText.text = GManager.instance.maxHeartNum.ToString();
    }

    void Update()
    {
        if (oldHeartNum != GManager.instance.heartNum)
        {
            isContinue = true;
            //heartText.text = GManager.instance.heartNum.ToString();
            //oldHeartNum = GManager.instance.heartNum;
        }

        //点滅
        if (isContinue)
        {
            if (blinkTime > 0.2f)
            {
                heartText.enabled = false; //非表示③→① respawnTime が指定の秒になるまで繰り返し
                blinkTime = 0.0f;
            }
            else if (blinkTime > 0.1f)
            {
                heartText.enabled = true; //表示↑②
            }
            else
            {
                heartText.enabled = false; //非表示↑①
            }


            if (respawnbox > respawnTime) //リセット
            {
                blinkTime = 0f;
                respawnbox = 0f;
                isContinue = false;
                heartText.enabled = true; //表示

                heartText.text = GManager.instance.heartNum.ToString();
                oldHeartNum = GManager.instance.heartNum;
            }
            else
            {
                blinkTime += Time.deltaTime;
                respawnbox += Time.deltaTime;
            }
        }

    }
    */
}
