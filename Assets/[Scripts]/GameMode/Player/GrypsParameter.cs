using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrypsParameter : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(1, 150)] public int[] boostPower;

    [NamedArrayAttribute(new string[] { "ã", "’†", "‹­", "“Á‹­" })]
    [Range(1, 150)] public int[] dashPower;

    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(1, 200)] public int[] jetPower;

    [NamedArrayAttribute(new string[] { "1‰ñ“]", "2‰ñ“]", "3‰ñ“]" })]
    [Range(0.2f, 0.75f)] public float[] saltoTime;

    [NamedArrayAttribute(new string[] { "ã", "’†", "‹­" })]
    [Range(0.5f, 0.15f)] public float[] suctionPower;


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
