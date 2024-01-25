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
    int oldStockNum;
    int consumeNum;
    const int MAX_STOCK = 3;

    [Header("TimeScale")]
    public float slowTimeScale = 0.1f;
    [Range(0.1f, 0.5f)] public float slowTime = 0.1f;
    public Ease slowType = Ease.OutQuint;
    [Range(0.1f, 0.5f)] public float returnTime = 0.25f;
    public Ease returnType = Ease.InQuint;


    [HideInInspector] public JetHudManager jetHudMg;
    StageCtrl stageCrl;
    CinemachineManager cinemachineCrl;


    private void Awake()
    {
        GetComponent();
        DisplayJetStock(stockNum);
        resetTimePer = resetTime / 120;
    }

    void GetComponent()
    {
        jetHudMg = GetComponent<JetHudManager>();
        stageCrl = transform.root.GetComponent<StageCtrl>();
        cinemachineCrl = Camera.main.transform.GetChild(0).GetComponent<CinemachineManager>();
    }


    public void DisplayJetStock(int stockNum)
    {
        if (MAX_STOCK < stockNum) return; //stockNum��3���傫���ꍇ�́A���^�[������.

        if (stockNum > oldStockNum) //����.
        {
            if (stock[stockNum - 1] == null) return;
            stock[stockNum - 1].DOKill(true);
            stock[stockNum - 1].DOFade(1f, 0.25f).SetEase(Ease.OutSine);

            if (!jetHudMg.isHud && stageCrl.controlStatus != StageCtrl.ControlStatus.unControl) // stock�ω����ɖ���Ă΂�邽��Hud��false�̏ꍇ�̂݋N�������� && �󒆎���Hud���N�����������Ȃ�����.
            {
                jetHudMg.StartUpJetHud();
            }
        }
        else if (stockNum < oldStockNum) //����.
        {
            if (stock[stockNum] == null) return;
            stock[stockNum].DOKill(true);
            stock[stockNum].DOFade(0f, 0.25f);

            if (jetHudMg.isHud && stockNum == 0)
            {
                jetHudMg.ShutDownJetHud();
            }
        }

        oldStockNum = stockNum;

        this.stockNum = stockNum; //DashArea����n���ꂽstockNum���X�V����.

        // for (int i = 0; i < MAX_STOCK; i++) stock[i].enabled = (stockNum > i);

    }

    public void ChargeGauge() //�������ōő�1�X�g�b�N�����.
    {
        gauge.DOKill(true);
        gauge.color = gaugeColor[0];
        gauge.fillAmount = 0.07f;

        pointer.DOKill(true);
        pointer.localRotation = Quaternion.Euler(0, 0, 240);

        gauge.DOFillAmount(0.425f, chargeTime).SetEase(Ease.Linear).SetDelay(0.125f)
            .OnComplete(() =>
            {
                consumeNum++; //1�ɂȂ�.
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
        jetHudMg.JetButton(isPush);

        if (isCoolDown) return; //�N�[���_�E�����̓X���[������.

        ChargeGauge();

        stageCrl.state = StageCtrl.State.Play; //�ꎞ�I��LackMode����PlayMode�ֈڂ�.
        stageCrl.grypsCrl.effector.animatorJet.SetBool("isDown", true);

        if (Time.timeScale > slowTimeScale)
        {
            cinemachineCrl.DOTimeScale(slowTimeScale, slowTime, slowType);
        }
    }

    public void OnButtonUp()
    {
        isPush = false;
        jetHudMg.JetButton(isPush);

        if (isCoolDown) return; //�N�[���_�E�����̓X���[������.

        if (consumeNum >= 1)
        {
            //�X�g�b�N�������ꍇ => ���߂���Ƀ{�^���𗣂����ꍇ.
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
            //�X�g�b�N������Ȃ��ꍇ => ���܂�O�Ƀ{�^���𗣂����ꍇ.
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

        if (!stageCrl.saltoMg.saltoHudMg.isHud) //Salto����TimeScale�J�ڂ����s�����邽�߃X���[������.
        {
            cinemachineCrl.DOTimeScale(1f, returnTime, returnType);
        }

        //�������؂ꃂ�[�h�N��
        if (stageCrl.memoryGageMg.memoryGage <= 0)
        {
            stageCrl.state = StageCtrl.State.Lack;
        }

    }

    void ConsumeStock(int consumeNum)
    {
        stockNum -= consumeNum;
        DisplayJetStock(stockNum);
        stageCrl.grypsCrl.ForceJet(consumeNum - 1);

        stageCrl.saltoMg.SaltoEnd(); //�󒆂�Jet�����ۂɁASaltoHud��Shutdown�����邽�߂ɌĂ�.

        this.consumeNum = 0;
        isCoolDown = true; //�N�[���_�E�����͓��͂��󂯕t���Ȃ�.
    }


    /*
    public void Land() //���n����Stock�������������𔻒f����
    {
        if (stockNum >= 1 && !jetHudMg.isHud) //stock��1�ȏ� && Hud��false�̏ꍇ�̂݋N��������
        {
            jetHudMg.StartUpJetHud();
        }
    }
     */

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
