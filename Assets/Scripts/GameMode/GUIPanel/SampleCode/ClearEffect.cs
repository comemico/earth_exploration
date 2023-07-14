using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClearEffect : MonoBehaviour
{
    [Header("拡大縮小のアニメーションカーブ")] public AnimationCurve curve;

    private bool comp = false;
    private float timer;
    public Text recordTime;
    //private Transform clearText;

    private void Start()
    {
        //clearText = GetComponentInChildren<Transform>();
        recordTime.text = "RecordTime : " + PlayerPrefs.GetFloat("RecordTime").ToString("F2") + " s";
    }
    void Update()
    {

        if (!comp)
        {
            if (timer < 1.0f)
            {
                recordTime.transform.localScale = Vector3.one * curve.Evaluate(timer);
                timer += Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.one;
                comp = false;
            }
        }

    }
}
