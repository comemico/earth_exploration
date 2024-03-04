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

    [NamedArrayAttribute(new string[] { "��", "��", "��" })]
    [Range(0.5f, 0.15f)] public float[] suctionPower;


    [Range(0.5f, 1.0f)] public float breakPower;


}
