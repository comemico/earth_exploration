using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using SoundSystem;

public class GrypsController : MonoBehaviour
{
    //��ɕ������Z�Ɋւ��X�N���v�g

    public StageCtrl stageCrl;
    public GrypsEffector effector;
    public WheelManager wheelMg;
    public GrypsParameter parameter;

    [HideInInspector] public EffectManager effectManager;
    GroundCheck ground;

    public bool isBrake;

    [HideInInspector] public Rigidbody2D rb = null;

    [Header("�u�[�X�g�̃N�[���^�C��")]
    public float reloadTime = 0.1f;
    [Range(25, 100)] public float reloadVelocity;
    private float time;

    private bool isGround = false;
    private bool isGroundPrev = false;


    private void Awake()
    {
        AllGetComponent();
        //targetVector = Quaternion.Euler(0f, 0f, 360f) * Vector2.up;//�^�[�Q�b�g�̊p�x���x�N�^�[
    }


    void AllGetComponent()
    {
        rb = GetComponent<Rigidbody2D>();

        effector = GetComponentInChildren<GrypsEffector>();
        parameter = GetComponentInChildren<GrypsParameter>();
        wheelMg = GetComponentInChildren<WheelManager>();
        effectManager = GetComponentInChildren<EffectManager>();
        ground = GetComponentInChildren<GroundCheck>();

    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//���n����

        if (isGround != isGroundPrev)
        {
            if (isGround == true)
            {
                Land();
                isGroundPrev = true;
            }
            else if (isGround == false)
            {
                TakeOff();
                isGroundPrev = false;
            }
        }

        if (isBrake)
        {
            rb.velocity *= parameter.breakPower;
            if (Mathf.Abs(rb.velocity.x) <= 0.25f) Brake(false);//velocity�� 0.25f�ȉ��ŉ���
        }

        if (stageCrl.controlStatus == StageCtrl.ControlStatus.restrictedControl && Mathf.Abs(rb.velocity.x) <= reloadVelocity)
        {
            if (time >= reloadTime)
            {
                stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
                wheelMg.WheelLamp(false, true);
                time = 0.0f;
            }
            else
            {
                time += Time.deltaTime;
            }
        }

    }


    public void ForceBoost(int gearNum)
    {
        Brake(false);//ForceBoost()�Ńu���[�L������
        Vector2 force = transform.localScale.x * transform.right * parameter.boostPower[gearNum - 1];
        rb.AddForce(force, ForceMode2D.Impulse);
        //effectManager.JetEffect();
        wheelMg.ArchedBack(false);

        SoundManager.Instance.PlaySE(SESoundData.SE.Force_Boost);
        SoundManager.Instance.seAudioSource.pitch = 1.15f;
    }

    public void Turn(int key, bool isBrake)
    {
        transform.localScale = new Vector3(key, transform.localScale.y, transform.localScale.z);
        if (isBrake) Brake(true);//ControlScreen����Ă΂��
    }

    public void Return()
    {
        rb.DORotate(0f, 0.5f).SetEase(Ease.OutSine);
    }

    public void ForceDash(int key, int power)
    {
        Brake(false);//ForceDash()�Ńu���[�L������
        if (key != (int)transform.localScale.x)
        {
            rb.velocity = Vector2.zero;
            Turn(key, false);
        }
        wheelMg.WheelLamp(true, true);
        stageCrl.controlScreenMg.KeyChange(key);
        Vector2 force = transform.localScale.x * transform.right * parameter.dashPower[power];
        rb.AddForce(force, ForceMode2D.Impulse);

        SoundManager.Instance.PlaySE(SESoundData.SE.Force_Dash);
        SoundManager.Instance.seAudioSource.pitch = 1.5f;
    }

    public void Brake(bool isBrake)
    {
        this.isBrake = isBrake;
        effectManager.Brake(isBrake);
        wheelMg.WheelBlade(isBrake);
    }

    public void ForceJet(int power)
    {
        Brake(false);//ForceJet()�Ńu���[�L������
        Vector2 force = transform.localScale.x * transform.right * parameter.jetPower[power];
        rb.AddForce(force, ForceMode2D.Impulse);

        //�n�� => restrictedControl , �� => unControl
        if (isGround)
        {
            stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.restrictedControl);
        }
        else
        {
            stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
        }

        SoundManager.Instance.PlaySE(SESoundData.SE.Force_Jet);
        SoundManager.Instance.seAudioSource.pitch = 0.85f;
    }

    private void TakeOff() //����
    {
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.unControl);
        Brake(false);//TakeOff()�Ńu���[�L������
        //rb.DORotate(-1 * (int)transform.localScale.x * 10f, 0.5f).SetEase(Ease.OutSine);
    }

    private void Land() //���n
    {
        stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
        wheelMg.WheelLamp(false, true);

        stageCrl.saltoMg.SaltoEnd(); //Salto�����n�����ꍇSaltoHud��Shutdown�����邽�߂ɌĂ�

    }


}
