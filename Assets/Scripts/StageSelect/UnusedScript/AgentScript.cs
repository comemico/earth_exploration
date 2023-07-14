using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{

    private NavMeshAgent agent;
    private Vector3 distance;
    private StagePoint stagePoint;
    public bool isOn;
    //public bool isPassCourse;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        //FaceVelocity();
    }

    public void IncreaceStagePoint(Vector3 stagePosition)
    {
        if (isOn)
        {
            agent.SetDestination(stagePosition);
            //isPassCourse = false;
        }
        else
        {
            Debug.Log("コースを通過していないよ");
        }

    }

    public void DecreaceStagePoint(Vector3 stagePosition)
    {
        if (isOn)
        {
            agent.SetDestination(stagePosition);
            //isPassCourse = false;
        }
        else
        {
            Debug.Log("コースを通過していないよ");
        }
    }

    void FaceVelocity()//進む方向に向くよう回転する
    {
        distance = agent.desiredVelocity;
        transform.right = distance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            isOn = true;
        }
    }


}
