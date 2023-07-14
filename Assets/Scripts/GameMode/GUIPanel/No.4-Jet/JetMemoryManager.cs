using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class JetMemoryManager : MonoBehaviour
{
    [Header("ジェットメモリ")] public Image[] jetMemory;
    [Header("推移秒間")] public float time;

    private Tween decreaseTween = null;

    private void Start()
    {
        DisplayJetMemory(0);
    }
    public void DotweenJetMemory(int oldJetMemory, int jetMemory)//,float time
    {
        DOTween.Kill(decreaseTween);
        decreaseTween = DOTween.To(() => oldJetMemory, (val) =>
        {

            DisplayJetMemory(val);

        }, jetMemory, time);

    }

    private void DisplayJetMemory(int memory)
    {

        for (int i = 0; i < jetMemory.Length; i++)
        {
            jetMemory[i].enabled = (memory > i);
        }

    }
}
