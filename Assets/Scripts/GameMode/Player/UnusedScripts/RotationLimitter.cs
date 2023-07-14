using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLimitter : MonoBehaviour
{

    [Header("大きくするほどトルクカーブが鋭くなる")] [Range(1.0f, 25.0f)] public float TorqueExponent = 4.0f;
    [Header("回転押し戻しトルクの最大値")] [Range(0.0f, 2000.0f)] public float TorqueMagnitudeMax = 100.0f;
    [Header("回転角の限界値")] [Range(0.0f, 180.0f)] public float RotationLimit = 90.0f;

    private new Rigidbody2D rigidbody2D;

    void Start()
    {
        this.rigidbody2D = this.GetComponent<Rigidbody2D>();
        this.rigidbody2D.angularDrag = 4.0f;
    }

    void FixedUpdate()
    {
        // 現在の回転の大きさと向き
        float rotationMagnitude = this.rigidbody2D.rotation;
        float rotationSign = Mathf.Sign(rotationMagnitude);
        rotationMagnitude *= rotationSign;
        float factor = Mathf.Pow(Mathf.InverseLerp(0.0f, this.RotationLimit, rotationMagnitude), this.TorqueExponent);


        // 回転と逆向きにトルクをかける
        float torque = -rotationSign * this.TorqueMagnitudeMax * factor;
        this.rigidbody2D.AddTorque(torque);
    }
}
