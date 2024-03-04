using System;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    /// <summary>
    /// フロアの配置方法 : 奥行きはフロアオブジェクト全体のY値を上下させる( SpriteShape.Hight=>0.5f の場合、高さは1.25f ).
    /// </summary>

    [Header("フロア配列")]
    public Transform[] colBox;
    CinemachineManager cinemachineMg;

    private void Start()
    {
        cinemachineMg = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
    }

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

        cinemachineMg.ChangeDirection(key);

        for (int i = 0; i < colBox[floorNumber].childCount; i++)
        {
            colBox[floorNumber].GetChild(i).GetComponent<Collider2D>().enabled = true;
        }

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
