using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class JetManager : MonoBehaviour
{
    [Header("ButtonLamp")]
    public Image buttonLamp_Right;
    public Image buttonLamp_Left;
    [Space(10)]

    public bool isPush;
    public bool isCoolDown;

    [Header("DialGauge")]
    public Image gauge;
    public RectTransform pointer;
    public Image pointerLamp;
    public Color[] gaugeColor;
    [Space(10)]

    [Range(0.25f, 3f)] public float chargeTime;
    [Range(0.25f, 3f)] public float coolTime;
    [Range(0.25f, 3f)] public float resetTime;
    float resetTimePer;

    [Header("Stock")]
    public Image[] stock;
    public int stockNum;
    const int MAX_STOCK = 3;
    int consumeNum;

    [Header("TimeScale")]
    public float slowTimeScale = 0.1f;
    [Range(0.1f, 0.5f)] public float slowTime = 0.1f;
    public Ease slowType = Ease.OutQuint;
    [Range(0.1f, 0.5f)] public float returnTime = 0.25f;
    public Ease returnType = Ease.InQuint;

    Tween t_slow;

    StageCtrl stageCrl;
    [HideInInspector] public JetGUIManager jetGuiMg;
    CinemachineManager cinemachineCrl;
    //public GrypsController grypsCrl;


    private void Awake()
    {
        GetComponent();
        DisplayJetStock(stockNum);
        resetTimePer = resetTime / 120;
    }

    void GetComponent()
    {
        jetGuiMg = GetComponent<JetGUIManager>();
        stageCrl = transform.root.GetComponent<StageCtrl>();
        cinemachineCrl = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
    }


    public void DisplayJetStock(int Num)
    {
        if (Num > MAX_STOCK) return;

        //stack.DOKill(true);
        for (int i = 0; i < MAX_STOCK; i++)
        {
            stock[i].enabled = (Num > i);
        }

        this.stockNum = Num;

        if (stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)//�󒆎�(Salto)��JetHud�N�~ ���n��(Dash)��JetHud�N�Z
        {
            JugeJetMode();
        }
    }

    public void JugeJetMode()
    {
        if (!jetGuiMg.isHud && stockNum >= 1)
        {
            jetGuiMg.StartUpJetHud();
        }
    }

    public void ChargeGauge() //�������ōő�1�X�g�b�N�����
    {
        gauge.DOKill(true);
        gauge.color = gaugeColor[0];
        gauge.fillAmount = 0.07f;

        pointer.DOKill(true);
        pointer.localRotation = Quaternion.Euler(0, 0, 240);

        gauge.DOFillAmount(0.425f, chargeTime).SetEase(Ease.Linear).SetDelay(0.125f)
            .OnComplete(() =>
            {
                consumeNum++; //1�ɂȂ�
                gauge.color = gaugeColor[consumeNum];
            });

        pointer.DOLocalRotate(new Vector3(0, 0, 120), chargeTime, RotateMode.Fast).SetEase(Ease.Linear).SetDelay(0.125f)
            .OnComplete(() =>
            {
                pointerLamp.enabled = true;
                buttonLamp_Left.enabled = true;
                buttonLamp_Right.enabled = true;
            });

    }

    public void OnButtonDown()
    {
        isPush = true;
        jetGuiMg.JetButton(isPush);

        if (isCoolDown) return; //�N�[���_�E�����̓X���[������

        stageCrl.Regeneration(); //�ꎞ�I��LackMode����PlayMode�ֈڂ�
        stageCrl.grypsCrl.effector.animatorJet.SetBool("isDown", true);
        ChargeGauge();

        if (Time.timeScale > slowTimeScale)
        {
            t_slow = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, slowTimeScale, slowTime).SetEase(slowType);
        }
    }
    /*
        stageCrl.grypsCrl.jetAnimator.SetBool("isDown", isDown);
     */

    public void OnButtonUp()
    {
        isPush = false;
        jetGuiMg.JetButton(isPush);

        if (isCoolDown) return; //�N�[���_�E�����̓X���[������


        if (consumeNum >= 1)
        {
            //�X�g�b�N�������ꍇ
            ConsumeStock(consumeNum);
            gauge.DOKill(false);
            gauge.DOFillAmount(0.07f, coolTime).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    isCoolDown = false;
                    if (isPush) OnButtonDown();

                    pointerLamp.enabled = false;
                    stageCrl.grypsCrl.effector.animatorJet.SetBool("isDown", false);
                });
            pointer.DOKill(false);
            pointer.DOLocalRotate(new Vector3(0, 0, 240), coolTime, RotateMode.Fast).SetEase(Ease.Linear);

        }
        else
        {
            //�X�g�b�N������Ȃ��ꍇ
            float returnTime = (240 - (int)pointer.localEulerAngles.z) * resetTimePer;

            gauge.DOKill(false);
            gauge.DOFillAmount(0.07f, returnTime).SetEase(Ease.Linear);
            pointer.DOKill(false);
            pointer.DOLocalRotate(new Vector3(0, 0, 240), returnTime, RotateMode.Fast).SetEase(Ease.Linear);

            stageCrl.grypsCrl.effector.animatorJet.SetBool("isDown", false);

        }


        // �X�g�b�N����Ɋ֌W�Ȃ����ʎ��s
        buttonLamp_Left.enabled = false;
        buttonLamp_Right.enabled = false;

        if (!stageCrl.saltoMg.saltoHudMg.isHud) //Salto����TimeScale�J�ڂ����s�����邽�߃X���[������
        {
            t_slow = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnTime).SetEase(returnType);
        }

        //�������؂ꃂ�[�h�N��
        if (stageCrl.memoryGageMg.memoryGage <= 0)
        {
            stageCrl.Lack();
        }

    }
    /*
        stageCrl.grypsCrl.jetAnimator.SetBool("isDown", isDown);
     */

    void ConsumeStock(int consumeNum)
    {
        stockNum -= consumeNum;
        DisplayJetStock(stockNum);

        stageCrl.saltoMg.Release(); //�󒆂�Jet�����ۂɁASaltoHud��Shutdown�����邽�߂ɌĂ�
        stageCrl.grypsCrl.ForceJet(consumeNum - 1);
        if (stockNum <= 0) jetGuiMg.ShutDownJetHud();

        isCoolDown = true;
        this.consumeNum = 0;
    }



    /*
    void Release(int consumeNum)
    {
        stockNum -= consumeNum; //limitRing�̕\���������킹��

        DisplayJetStock(stockNum);

        if (consumeNum >= 1)
        {
            stageCrl.saltoMg.Release(); //�󒆂�Jet�����ۂɁASaltoHud��Shutdown�����邽�߂ɌĂ�
            stageCrl.grypsCrl.ForceJet(consumeNum - 1);
            if (stockNum <= 0)
            {
                jetGuiMg.ShutDownJetHud();
            }
        }

    }

    public void ChargeRing(int limitNum) //�������ōő�3�X�g�b�N�����
    {
        consumeNum = 0;

        chargeRingImg.fillAmount = 0f;
        chargeRingImg.DOFillAmount(0.3333333f, chargeTime).SetEase(chargeType).SetLoops(limitNum, LoopType.Incremental)
            .OnStepComplete(() =>
            {
                consumeNum++; //1�`3�܂ő������� (limitNum�ɉ�����)
                chargeRingImg.color = chargeColor[consumeNum];
            });
    }
     */


}
