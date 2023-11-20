using System;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("フロア配列")]
    public Transform[] colBox;
    CinemachineController cinemachineCrl;

    private void Start()
    {
        cinemachineCrl = Camera.main.GetComponent<CinemachineController>();
        /*
        floor = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            floor[i] = transform.GetChild(i);
        }
         */
    }
    /*
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
     */

    public void FalseCollider()
    {
        for (int x = 0; x < colBox.Length; x++)
        {
            for (int i = 0; i < colBox[x].childCount; i++)
            {
                colBox[x].GetChild(i).GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public int ActiveFloor(Transform gateObject, int key)
    {
        FalseCollider();
        int floorNumber = Array.IndexOf(colBox, gateObject);

        for (int i = 0; i < colBox[floorNumber].childCount; i++)
        {
            colBox[floorNumber].GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

        cinemachineCrl.ToFloorVcam(floorNumber, key);//不完全

        return floorNumber;
    }

    public int TurnFloor(Transform gateObject)
    {
        FalseCollider();
        int floorNumber = Array.IndexOf(colBox, gateObject);

        for (int i = 0; i < colBox[floorNumber].childCount; i++)
        {
            colBox[floorNumber].GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

        return floorNumber;
    }

    //allCollider = GetComponentsInChildren<Collider2D>();配列取得方法
}
