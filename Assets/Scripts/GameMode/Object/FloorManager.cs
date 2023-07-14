using System;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("�t���A�z��")] public GameObject[] floor;
    [Header("���ׂĂ�Collider�z��")] public Collider2D[] allCollider;
    [Header("�J�n�t���A")] public GameObject startFloor;

    void Start()
    {
        floor = new GameObject[transform.childCount];
        allCollider = GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < transform.childCount; i++)
        {
            floor[i] = transform.GetChild(i).gameObject;
        }
        ActiveFloor(startFloor);
    }

    public int ActiveFloor(GameObject parentObject)
    {
        AllUnenableCollider();

        int floorNumber = Array.IndexOf(floor, parentObject);

        for (int i = 0; i < floor[floorNumber].transform.childCount; i++)
        {
            floor[floorNumber].transform.GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

        return floorNumber;
    }

    public void AllUnenableCollider()
    {
        foreach (var col in allCollider)
        {
            col.enabled = false;
        }
    }
}
