using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class JetMemoryManager : MonoBehaviour
{
    [Header("�W�F�b�g������")] public Image[] jetMemory;
    [Header("���ڕb��")] public float time;

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
