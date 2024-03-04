using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class SpeedometerManager : MonoBehaviour
{
    public GrypsManager player;
    [HideInInspector] public Slider speedSlider;
    [Header("スピードメモリ")] public Image[] speedGageMemory;

    //[HideInInspector] 
    public float speedValue = 0;
    private float oldSpeedValue = 0;
    public int speedMemory = 0;
    private float oldSpeedMemory = 0;

    private void Start()
    {
        speedSlider = GetComponent<Slider>();
        //speedSlider.maxValue = player.maxSpeed;
        DisplayGageMemory(0);
    }

    void Update()
    {
        if (oldSpeedMemory != speedMemory)
        {
            DisplayGageMemory(speedMemory);
            oldSpeedMemory = speedMemory;
        }
        /*if (oldSpeedValue != speedValue)
        {
            speedSlider.value = speedValue;

            oldSpeedValue = speedValue;
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

        */