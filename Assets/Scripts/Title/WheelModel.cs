using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelModel : MonoBehaviour
{
    [Header("�o�[�i�E�g�l")]
    [NamedArrayAttribute(new string[] { "1�i�K", "2�i�K", "3�i�K", "4�i�K", "5�i�K", })]
    [Range(500, 2000)] public int[] burnPower;

    [Header("�O�� :�Q��")] public Transform frontTransform;
    [Header("�O�� :�~��")] public float radiusFront; //CircleCollider2D�R���|�[�l���g���A�^�b�`���āARadius���m�F����

    [Header("��� :�Q��")] public Transform[] wheelBlade;
    [Header("��� :�~��")] public float radiusRear; //CircleCollider2D�R���|�[�l���g���A�^�b�`���āARadius���m�F����

    [Header("---�C�[�W���O----")]
    [Header("�I���l")]
    public float endValue;
    [Header("���")]
    public Ease easeType;
    [Header("����")]
    public float easeDuration;

    private GrypsController grypsCrl;
    private float factorFront;
    private float factorRear;
    //public Light2D light2d;

    private List<Tween> tweenList = new List<Tween>();
    private void Awake()
    {
        grypsCrl = GetComponentInParent<GrypsController>();
        //light2d = GetComponentInChildren<Light2D>();        
    }
    private void Start()
    {
        factorFront = 1 / radiusFront;
        factorRear = 1 / radiusRear;
    }

    private void Update()
    {

        if (Mathf.Abs(grypsCrl.rb.velocity.x) >= 1f)
        {
            SpinWheel(frontTransform, factorFront);//�O��
            SpinWheel(transform, factorRear);//���
        }
        /*
        //��ւ̂�
        if (Mathf.Abs(grypsCrl.rb.velocity.x) <= 20f)
        {
            if (grypsCrl.stageCrl.controlScreenMg.gearNum >= 1)
            {
                BurnOutWheel(grypsCrl.stageCrl.controlScreenMg.gearNum - 1);
            }
        }
         */

    }

    public void SpinWheel(Transform wheel, float factor)
    {
        var translation = grypsCrl.rb.velocity * Time.deltaTime; // �ʒu�̕ω���
        var distance = translation.magnitude; // �ړ���������
        var angle = distance * factor; // (distance / circleCollider.radius) ������]����ׂ��� 
        wheel.Rotate(0f, 0f, -1 * angle * Mathf.Rad2Deg);
    }

    public void BurnOutWheel(int gearNum)
    {
        transform.Rotate(0f, 0f, -1 * burnPower[gearNum] * Time.deltaTime);
        //light
    }

    public void WheelBlade(bool isBrake)
    {
        for (int i = 0; i < wheelBlade.Length; i++)
        {
            wheelBlade[i].transform.DOComplete();
            if (isBrake)
            {
                wheelBlade[i].transform.DOLocalRotate(new Vector3(0f, 0f, endValue), easeDuration).SetEase(easeType);
            }
            else
            {
                wheelBlade[i].transform.DOLocalRotate(Vector3.zero, easeDuration).SetEase(easeType);
            }
        }
    }

}
