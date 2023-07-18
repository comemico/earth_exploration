using System;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("フロア配列")] public GameObject[] floor;
    [Header("開始フロア")] public GameObject startFloor;
    //[Header("すべてのCollider配列")] public Collider2D[] allCollider;

    void Start()
    {
        floor = new GameObject[transform.childCount];
        //allCollider = GetComponentsInChildren<Collider2D>();

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

        for (int i = 0; i < floor[floorNumber].transform.GetChild(0).childCount; i++)
        {
            floor[floorNumber].transform.GetChild(0).GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

        return floorNumber;
    }

    public void AllUnenableCollider()
    {

        for (int x = 0; x < floor.Length; x++)
        {
            for (int i = 0; i < floor[x].transform.GetChild(0).childCount; i++)
            {
                floor[x].transform.GetChild(0).GetChild(i).GetComponent<Collider2D>().enabled = false;
            }
        }

        /*
        foreach (var col in allCollider)
        {
            col.enabled = false;
        }
         */
    }
}
