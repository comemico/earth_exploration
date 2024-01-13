using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyballManager : MonoBehaviour
{
    [Header("このステージのエナジーボールの数")] public int energyball;
    [Header("エナジーボール数の表示")] public Text energyText;

    private int oldenergyball = 0;
    public int energyballCurrent = 0;

    void Start()
    {
        energyText.text = oldenergyball + "/" + energyball;
    }

    void Update()
    {
        if (oldenergyball != energyballCurrent)
        {
            energyText.text = energyballCurrent + "/" + energyball;
            oldenergyball = energyballCurrent;

        }

    }
}
