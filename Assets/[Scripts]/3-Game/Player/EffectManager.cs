using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : MonoBehaviour
{
    [SerializeField] public ParticleSystem[] particle;

    private void Start()
    {
        //var main = GetComponent<ParticleSystem>();
        //main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void RevUpEffect()
    {
        particle[0].gameObject.SetActive(true);
    }

    public void IdleEffect()
    {
        particle[0].gameObject.SetActive(false);
    }

    public void JetEffect()
    {
        particle[0].gameObject.SetActive(false);
        particle[1].Play();
    }

    public void Brake(bool isTrue)
    {
        particle[2].gameObject.SetActive(isTrue);
    }

    private void OnParticleSystemStopped()
    {
        Debug.Log("終了");
    }

}
