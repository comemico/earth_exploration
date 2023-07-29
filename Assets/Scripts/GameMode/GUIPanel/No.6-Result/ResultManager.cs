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
        [InspectorName("�N���A")] clear = 0,
        [InspectorName("�~�X: �����]�[��")] missZone = 1,
        [InspectorName("�~�X: �R���؂�")] missLack = 2
    }

    const string clear = "�~�b�V��������";
    const string miss = "�~�b�V�������s";

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
