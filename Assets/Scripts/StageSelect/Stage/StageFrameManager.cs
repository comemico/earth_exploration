using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageFrameManager : MonoBehaviour
{
    const float PreHeader = 20f;

    public RectTransform RectTransform => this.transform as RectTransform;

    [Space(PreHeader)]
    [Header("�^�[�Q�b�g�X�R�[�v")]
    [Header("-----------------------------")]
    public GameObject frame_Out;
    public GameObject frame_In;
    [Header("�X�P�[��")]
    public float maxValue;
    [Header("����")]
    public float duration_Scale;
    public float duration_Rotation;
    public float duration_Fade;
    public float duration_Loop;
    [Header("�^�C�v")]
    public Ease easeType_Target;

    private RectTransform frame_Out_Rect;
    private CanvasGroup frame_Out_Canvas;
    private RectTransform frame_In_Rect;


    [Space(PreHeader)]
    [Header("�X���C�_�[")]
    [Header("-----------------------------")]
    public Slider slider_First;
    public Slider slider_Second;
    [Header("����")]
    public float duration_First;
    public float duration_Second;
    [Header("�^�C�v")]
    public Ease easeType_Slider;
    [Header("1�}�X�̒���")]
    public float distance_Slider;

    private RectTransform slider_Second_Rect;


    [Space(PreHeader)]
    [Header("�o�b�N�p�l��")]
    [Header("-----------------------------")]
    public GameObject level_Mark;
    [Header("1�}�X�̒���")]
    public float distance_BackPanel;
    public float distance_BackPanel_Initial;

    private RectTransform level_BackPanel_Rect;
    private CanvasGroup level_BackPanel_Canvas;


    [Space(PreHeader)]
    [Header("�}�[�N�擾")]
    [Header("-----------------------------")]
    [Header("�X�e�[�W���x�����")]
    public GameObject maxLevelOb;
    public GameObject LevelOb;
    [Header("�X�e�[�W���x��")]
    public Image[] maxLevelBox;
    public Image[] levelBox;

    Tween tween;
    private List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.Add(tween);
        tweenList.KillAllAndClear();
    }

    void Awake()
    {
        AllGetComponent();
        InitializeStageLevel();

        tween = frame_In_Rect.DOLocalRotate(new Vector3(0, 0, 360f), duration_Loop, RotateMode.FastBeyond360)
         .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetRelative(true);

    }

    void AllGetComponent()
    {
        frame_Out_Rect = frame_Out.GetComponent<RectTransform>();
        frame_Out_Canvas = frame_Out.GetComponent<CanvasGroup>();
        frame_In_Rect = frame_In.GetComponent<RectTransform>();

        slider_Second_Rect = slider_Second.gameObject.GetComponent<RectTransform>();

        level_BackPanel_Canvas = level_Mark.GetComponent<CanvasGroup>();
        level_BackPanel_Rect = level_Mark.transform.GetChild(0).GetComponent<RectTransform>();
    }


    private void InitializeStageLevel()
    {
        maxLevelBox = new Image[maxLevelOb.transform.childCount];
        levelBox = new Image[LevelOb.transform.childCount];

        for (int i = 0; i < maxLevelOb.transform.childCount; i++)
        {
            maxLevelBox[i] = maxLevelOb.transform.GetChild(i).GetComponent<Image>();
            levelBox[i] = LevelOb.transform.GetChild(i).GetComponent<Image>();

            maxLevelBox[i].enabled = false;
            levelBox[i].enabled = false;
        }
    }

    public void DisplayMaxLevel(int num)
    {
        slider_Second_Rect.sizeDelta = new Vector2((distance_Slider * num), slider_Second_Rect.sizeDelta.y);
        level_BackPanel_Rect.sizeDelta = new Vector2(distance_BackPanel_Initial + (distance_BackPanel * num), level_BackPanel_Rect.sizeDelta.y);

        for (int i = 0; i < maxLevelBox.Length; i++)
        {
            maxLevelBox[i].enabled = (num > i);
        }
    }

    private void DisplayStageLevel(int num)
    {
        for (int i = 0; i < levelBox.Length; i++)
        {
            levelBox[i].enabled = (num > i);
        }
    }

    public void ChangeTarget(int levelNum)
    {
        DisplayStageLevel(levelNum);

        tweenList.KillAllAndClear();

        Sequence sequence = DOTween.Sequence();
        Sequence sequence_Target = DOTween.Sequence();
        Sequence sequence_Level = DOTween.Sequence();


        sequence_Target.OnStart(() =>
        {
            frame_Out_Rect.transform.localScale = new Vector3(maxValue, maxValue, 1f);
            frame_Out_Rect.transform.localEulerAngles = Vector3.zero;
            frame_Out_Canvas.alpha = 0f;
            slider_First.value = 0f;
            slider_Second.value = 0f;
            level_BackPanel_Canvas.alpha = 0f;
        });



        sequence_Target.Append(frame_Out_Canvas.DOFade(1f, duration_Fade).SetEase(easeType_Target));
        sequence_Target.Join(frame_Out_Rect.transform.DOScale(Vector3.one, duration_Scale).SetEase(easeType_Target));
        sequence_Target.Join(frame_Out_Rect.transform.DORotate(new Vector3(0f, 0f, 120f), duration_Rotation).SetEase(easeType_Target));


        sequence_Level.Append(slider_First.DOValue(1f, duration_First).SetEase(easeType_Slider));
        sequence_Level.Append(slider_Second.DOValue(1f, duration_Second).SetEase(easeType_Slider));
        sequence_Level.Join(level_BackPanel_Canvas.DOFade(1f, duration_First).SetEase(easeType_Slider));


        sequence.Append(sequence_Target);
        sequence.Insert(0.175f, sequence_Level);

        tweenList.Add(sequence);

    }


}
