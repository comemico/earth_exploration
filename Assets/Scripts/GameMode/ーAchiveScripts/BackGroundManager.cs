using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] float multiplierX = 0.0f;
    [SerializeField] float multiplierY = 0.0f;
    //[SerializeField] bool vertexOnly = true;

    private Transform cameraTransform;

    private Vector3 startCameraPos;
    private Vector3 startPos;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 position = startPos;
        position.x += multiplierX * (cameraTransform.position.x - startCameraPos.x);
        position.y += multiplierY * (cameraTransform.position.y - startCameraPos.y);
        transform.position = position;
        /*
        if (vertexOnly)
        {
        }
        else
        {
            position += multiplier * (cameraTransform.position - startCameraPos);
        }
         */

    }
}
