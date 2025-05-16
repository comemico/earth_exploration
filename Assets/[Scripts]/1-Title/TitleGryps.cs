using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class TitleGryps : MonoBehaviour
{

    // Power�{�^��������A�ڃ����v�_��(�W�F�b�g+�}�[�N).
    // JetAnimation( Idle / Open:Close )�̐؂�ւ�����.
    [Header("WakeUp")]
    public SpriteRenderer decoLamp;
    public Renderer matJetline;
    public Animator animatorJet;
    public SpriteRenderer headLamp;
    public Light2D underLamp;
    [Space(10)]

    [Range(0.1f, 1.5f)] public float onTime = 0.5f;


    [Header("Wheel")]
    public Transform frontWheel;
    public Transform rearWheel;
    public float radiusFront; //CircleCollider2D�R���|�[�l���g���A�^�b�`���āARadius���m�F����.
    public float radiusRear; //CircleCollider2D�R���|�[�l���g���A�^�b�`���āARadius���m�F����.
    float factorFront;
    float factorRear;


    [Header("Jet")]
    Rigidbody2D rb;

    GrypsParameter parameter;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        parameter = GetComponentInChildren<GrypsParameter>();

        factorFront = 1 / radiusFront;
        factorRear = 1 / radiusRear;
    }


    private void Update()
    {
        if (Mathf.Abs(rb.velocity.x) >= 0.5f)
        {
            SpinWheel(frontWheel, factorFront);//�O��
            SpinWheel(rearWheel, factorRear);//���
        }
    }

    public Sequence WakeUp()
    {
        Sequence s_wakeUp = DOTween.Sequence();
        s_wakeUp.AppendCallback(() => animatorJet.SetFloat("IdleSpeed", 1));
        s_wakeUp.Append(decoLamp.DOFade(1f, onTime).SetEase(Ease.OutQuint));
        s_wakeUp.Join(DOTween.To(() => matJetline.material.GetFloat("_Power"), x => matJetline.material.SetFloat("_Power", x), 1f, onTime).SetEase(Ease.OutQuint));

        s_wakeUp.Append(headLamp.DOFade(1f, onTime).SetEase(Ease.InQuint));
        s_wakeUp.Join(DOTween.To(() => underLamp.intensity, x => underLamp.intensity = x, 1f, onTime).SetEase(Ease.InQuint));

        return s_wakeUp;

    }


    // ForceJet()�̎���.
    public void ForceJet(int power)
    {
        //Brake(false);//ForceJet()�Ńu���[�L������
        Vector2 force = transform.localScale.x * transform.right * parameter.jetPower[power];
        rb.AddForce(force, ForceMode2D.Impulse);

        SoundManager.Instance.PlaySE(SESoundData.SE.Force_Jet);
        SoundManager.Instance.seAudioSource.pitch = 0.85f;
    }



    // SpinWheel()�̎���.
    public void SpinWheel(Transform wheel, float factor)
    {
        var translation = rb.velocity * Time.deltaTime; // �ʒu�̕ω���
        var distance = translation.magnitude; // �ړ���������
        var angle = distance * factor; // (distance / circleCollider.radius) ������]����ׂ��� 

        if (translation.x > 0)
        {
            wheel.Rotate(0f, 0f, (int)transform.localScale.x * -1f * angle * Mathf.Rad2Deg);
        }
        else if (translation.x <= 0)
        {
            wheel.Rotate(0f, 0f, (int)transform.localScale.x * 1f * angle * Mathf.Rad2Deg);
        }
    }



}
