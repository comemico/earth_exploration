using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoManager : MonoBehaviour
{
    public SaltoHudManager saltoHudMg;

    public void JugeSaltoMode()
    {
        saltoHudMg.StartUpSaltoHud();
    }

    public void Release()
    {
        saltoHudMg.ShutDownSaltoHud();
    }

}
