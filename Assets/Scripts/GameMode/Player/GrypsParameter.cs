using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrypsParameter : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK", "4’iŠK", "5’iŠK", })]
    [Range(5000, 30000)] public int[] boostPower;

    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(10000, 30000)] public int[] dashPower;

    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(50000, 100000)] public int[] jetPower;

    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(0.15f, 0.5f)] public float[] warpTime;


    [Range(0.5f, 1.0f)] public float breakPower;

    [Range(1000, 10000)] public int somersoultPower;
    /*
    public ControlLimit controlLimitB;
    public enum ControlLimit
    {
        [InspectorName("‘€ì•s”\")] ‘€ì•s”\b,
        [InspectorName("‘€ì‰Â”\")] ‘€ì‰Â”\b,
        [InspectorName("‘€ìˆê•”‰Â”\")] ‘€ìˆê•”‰Â”\b
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
