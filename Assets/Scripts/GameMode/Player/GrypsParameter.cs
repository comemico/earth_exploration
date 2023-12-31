using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrypsParameter : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "1�i�K", "2�i�K", "3�i�K" })]
    [Range(1, 150)] public int[] boostPower;

    [NamedArrayAttribute(new string[] { "��", "��", "��", "����" })]
    [Range(1, 150)] public int[] dashPower;

    [NamedArrayAttribute(new string[] { "1�i�K", "2�i�K", "3�i�K" })]
    [Range(1, 200)] public int[] jetPower;

    [NamedArrayAttribute(new string[] { "1��]", "2��]", "3��]" })]
    [Range(0.2f, 0.75f)] public float[] saltoTime;

    [NamedArrayAttribute(new string[] { "��", "��", "��" })]
    [Range(0.5f, 0.15f)] public float[] suctionPower;


    [Range(0.5f, 1.0f)] public float breakPower;

    [Range(1000, 10000)] public int somersoultPower;
    /*
    public ControlLimit controlLimitB;
    public enum ControlLimit
    {
        [InspectorName("����s�\")] ����s�\b,
        [InspectorName("����\")] ����\b,
        [InspectorName("����ꕔ�\")] ����ꕔ�\b
    }

    public Status status;
    public enum Status
    {
        Ready,
        Play,
        Lack,
        GameOver,
        Clear
    }
     */


}
