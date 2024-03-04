using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrypsParameter : MonoBehaviour
{
    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(1, 150)] public int[] boostPower;

    [NamedArrayAttribute(new string[] { "Žã", "’†", "‹­", "“Á‹­" })]
    [Range(1, 150)] public int[] dashPower;

    [NamedArrayAttribute(new string[] { "1’iŠK", "2’iŠK", "3’iŠK" })]
    [Range(1, 200)] public int[] jetPower;

    [NamedArrayAttribute(new string[] { "Žã", "’†", "‹­" })]
    [Range(0.5f, 0.15f)] public float[] suctionPower;


    [Range(0.5f, 1.0f)] public float breakPower;


}
