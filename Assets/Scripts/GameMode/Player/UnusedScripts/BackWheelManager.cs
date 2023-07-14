using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWheelManager : MonoBehaviour
{
    [Header("前輪の位置(回転)情報")] public GameObject frontWheel;


    private void Update()
    {
        transform.rotation = frontWheel.transform.rotation;
    }
}
