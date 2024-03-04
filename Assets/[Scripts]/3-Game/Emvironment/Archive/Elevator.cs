using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("移動経路")] public Transform[] movePoint;
    [Header("速さ")] public float speed = 1.0f;
    [Header("待ち時間（秒）")] public float min;
    [Header("入り判定")] public PlayerTriggerCheck playerTriggerCheck;
    [Header("プレイヤー重力操作")] public Rigidbody2D carRb;

    private Rigidbody2D rb;
    private int nowPoint = 0;
    private bool returnPoint;
    private Vector2 oldPosition = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;
    private float waitTime = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = movePoint[0].transform.position;
        oldPosition = rb.position;
        playerTriggerCheck.isExitOnce = true;
    }

    public Vector2 GetVelocity()
    {
        return myVelocity;
    }



    private void FixedUpdate()
    {
        if (playerTriggerCheck.isOn && !returnPoint && playerTriggerCheck.isExitOnce)
        {
            Debug.Log("起動");

            NextPointStage();
        }


        if (playerTriggerCheck.isOn && returnPoint && playerTriggerCheck.isExitOnce)
        {
            ReturnPointStage();
        }

        myVelocity = (rb.position - oldPosition) / Time.deltaTime;
        oldPosition = rb.position;


        //Debug.Log(waitTime);
    }
    void NextPointStage()
    {
        if (waitTime >= 2f)
        {
            carRb.GetComponent<Rigidbody2D>().gravityScale = 30;
            int nextPoint = nowPoint + 1;

            if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
            {
                Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);
                rb.MovePosition(toVector);
            }
            else
            {

                rb.MovePosition(movePoint[nextPoint].transform.position);
                nowPoint++;
                carRb.gravityScale = 12;
                playerTriggerCheck.isExitOnce = false;
                waitTime = 0f;


                if (nowPoint + 1 >= movePoint.Length)
                {
                    returnPoint = true;
                }
            }
        }
        else
        {
            waitTime += Time.deltaTime;
        }

    }

    void ReturnPointStage()
    {
        if (waitTime >= 2f)
        {
            carRb.GetComponent<Rigidbody2D>().gravityScale = 30;
            int nextPoint = nowPoint - 1;

            if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
            {
                Vector2 toVector = Vector2.MoveTowards(rb.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);
                rb.MovePosition(toVector);
            }
            else
            {
                rb.MovePosition(movePoint[nextPoint].transform.position);
                --nowPoint;
                playerTriggerCheck.isExitOnce = false;
                waitTime = 0f;
                Invoke("ResetGravity", 0.5f);

                if (nowPoint <= 0)
                {
                    returnPoint = false;
                }
            }
        }
        else
        {
            waitTime += Time.deltaTime;
        }
    }

    public void ResetGravity()
    {
        carRb.gravityScale = 12;
    }



}
