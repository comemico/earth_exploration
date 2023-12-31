﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{

    [SerializeField] float multiplier = 0.0f;
    [SerializeField] bool horizontalOnly = true;

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
        if (horizontalOnly)
        {
            position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
        }
        else
        {
            position += multiplier * (cameraTransform.position - startCameraPos);
        }
        transform.position = position;

    }
}
