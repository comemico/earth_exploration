using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultManager : MonoBehaviour
{
    public RESULT result;

    public Text textResult;

    public Color clearColor;
    public Color missColor;

    PopController popCrl;
    public enum RESULT
    {
        [InspectorName("クリア")] clear = 0,
        [InspectorName("ミス: 爆発ゾーン")] missZone = 1,
        [InspectorName("ミス: 燃料切れ")] missLack = 2
    }

    const string clear = "ミッション成功";
    const string miss = "ミッション失敗";

    private void Awake()
    {
        popCrl = GetComponent<PopController>();
    }

    public void Result(RESULT result)
    {
        if ((int)result == 0)
        {
            textResult.text = clear;
            popCrl.backPanel.color = clearColor;
        }
        else
        {
            textResult.text = miss;
            popCrl.backPanel.color = missColor;
        }
        popCrl.OpenPanel();
        textResult.DOFade(1f, 0.25f);
    }


}
