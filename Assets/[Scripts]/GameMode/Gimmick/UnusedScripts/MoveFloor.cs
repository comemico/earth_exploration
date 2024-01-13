using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [Header("移動経路")] public Transform[] movePoint;
    [Header("速さ")] public float speed = 1.0f;
    [Header("待ち時間（秒）")] public float min;

    private Rigidbody2D rb;
    private int nowPoint = 0;
    private bool returnPoint;
    private Vector2 oldPosition = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;
    private float waitTime = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = movePoint[0].transform.position;
        oldPosition = rb.position;
    }

    public Vector2 GetVelocity()
    {
        return myVelocity;
    }



    private void FixedUpdate()
    {

        if (!returnPoint)
        {
            int nextPoint = nowPoint + 1;

            if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
            {
                Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);
                rb.MovePosition(toVector);
            }
            else
            {
                rb.MovePosition(movePoint[nextPoint].transform.position);
                if (waitTime >= min)
                {
                    nowPoint++;
                    waitTime = 0.0f;
                }
                else
                {
                    waitTime += Time.deltaTime;
                }


                if (nowPoint + 1 >= movePoint.Length)
                {
                    returnPoint = true;
                }
            }
        }
        else
        {
            int nextPoint = nowPoint - 1;

            if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
            {
                Vector2 toVector = Vector2.MoveTowards(rb.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);
                rb.MovePosition(toVector);
            }
            else
            {
                rb.MovePosition(movePoint[nextPoint].transform.position);
                if (waitTime >= min)
                {
                    --nowPoint;
                    waitTime = 0.0f;
                }
                else
                {
                    waitTime += Time.deltaTime;
                }

                if (nowPoint <= 0)
                {
                    returnPoint = false;
                }
            }
        }
        myVelocity = (rb.position - oldPosition) / Time.deltaTime;
        oldPosition = rb.position;

    }
}
