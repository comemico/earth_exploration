using System;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("�t���A�z��")] public Transform[] floor;
    CinemachineController camCrl;
    private void Awake()
    {
        floor = new Transform[transform.childCount];
        camCrl = Camera.main.GetComponent<CinemachineController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            floor[i] = transform.GetChild(i);
        }
    }

    public void AllUnenableCollider()
    {
        for (int x = 0; x < floor.Length; x++)
        {
            for (int i = 0; i < floor[x].GetChild(0).childCount; i++)
            {
                floor[x].GetChild(0).GetChild(i).GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public int ActiveFloor(Transform gateObject, int key)
    {
        AllUnenableCollider();
        int floorNumber = Array.IndexOf(floor, gateObject);

        for (int i = 0; i < floor[floorNumber].GetChild(0).childCount; i++)
        {
            floor[floorNumber].GetChild(0).GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

        camCrl.ToFloorVcam(floorNumber, key);//�s���S

        return floorNumber;
        // return floorNumber;
    }

    public int TurnFloor(Transform gateObject)
    {
        AllUnenableCollider();
        int floorNumber = Array.IndexOf(floor, gateObject);

        for (int i = 0; i < floor[floorNumber].GetChild(0).childCount; i++)
        {
            floor[floorNumber].GetChild(0).GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

        return floorNumber;
    }

    //allCollider = GetComponentsInChildren<Collider2D>();�z��擾���@
}
