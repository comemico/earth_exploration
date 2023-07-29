using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrypsParameter : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "1段階", "2段階", "3段階", "4段階", "5段階", })]
    [Range(5000, 30000)] public int[] boostPower;

    [NamedArrayAttribute(new string[] { "弱", "中", "大", "特大" })]
    [Range(5000, 30000)] public int[] dashPower;

    [NamedArrayAttribute(new string[] { "1段階", "2段階", "3段階" })]
    [Range(5000, 20000)] public int[] jetPower;

    [NamedArrayAttribute(new string[] { "弱", "中", "強" })]
    [Range(0.5f, 0.15f)] public float[] suctionPower;


    [Range(0.5f, 1.0f)] public float breakPower;

    [Range(1000, 10000)] public int somersoultPower;
    /*
    public ControlLimit controlLimitB;
    public enum ControlLimit
    {
        [InspectorName("操作不能")] 操作不能b,
        [InspectorName("操作可能")] 操作可能b,
        [InspectorName("操作一部可能")] 操作一部可能b
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
