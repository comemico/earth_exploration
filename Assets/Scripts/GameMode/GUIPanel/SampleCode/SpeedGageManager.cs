using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGageManager : MonoBehaviour
{
    [Header("スピードメモリ")] public SpriteRenderer[] speedGageMemory;

    public int speedMemory = 0;
    private float oldSpeedMemory = 0;

    private void Start()
    {
        DisplayGageMemory(0);
    }

    void Update()
    {
        if (oldSpeedMemory != speedMemory)
        {
            DisplayGageMemory(speedMemory);
            oldSpeedMemory = speedMemory;
        }

    }
    public void DisplayGageMemory(int speedMemoryNumber)
    {
        for (int i = 0; i < speedGageMemory.Length; i++)
        {
            speedGageMemory[i].enabled = (speedMemoryNumber > i);
        }

    }
}
