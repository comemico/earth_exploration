using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CourseController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("�擾&�m�F")]
    public GameObject Course;
    public List<CourseInformation> courseInfoList;

    [Header("�h���b�O�`�A�j���[�V���������ǂ���")]
    public bool isDragging;

    [Header("�X���C�v���x")]
    public float dragSensitivity;

    [Header("---�I��:����----")]
    [Header("�ő�l")]
    public float maxValue_Distance;
    [Header("�e���͈�")]
    public float rangeOfInfluence_Distance;

    [Header("---�I��:�傫��----")]
    [Header("�ő�l")]
    public float maxValue_Scale;
    [Header("�e���͈�")]
    public float rangeOfInfluence_Scale;

    [Header("DOTween")]
    const float DEFAULT = 0.25f;
    public float fadeDuration;
    public Ease courseType;

    InformationManager informationMg;
    AreaController areaCtrl;
    CourseFrameManager courseFrameMg;

    private int nearestNumber;//�ł��߂��v�f���ς������X�V����ׂ̕ϐ�
    private float factorDistance;
    private float factorScale;
    private List<Tween> tweenList = new List<Tween>();
    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        GetComponent();
        InitializedStagePosition(informationMg.data.recentCourseAdress);
        factorDistance = 1 / rangeOfInfluence_Distance;
        factorScale = 1 / rangeOfInfluence_Scale;
    }

    void GetComponent()
    {
        informationMg = GetComponentInParent<InformationManager>();
        areaCtrl = transform.parent.GetComponentInChildren<AreaController>();
        courseFrameMg = GetComponentInChildren<CourseFrameManager>();
    }

    public void InitializedStagePosition(int initialValue)
    {
        for (int i = 0; i < Course.transform.childCount; i++)
        {
            courseInfoList.Add(Course.transform.GetChild(i).GetComponent<CourseInformation>());
        }

        courseFrameMg.RectTransform.anchoredPosition = new Vector2(maxValue_Distance, courseInfoList[initialValue].RectTransform.anchoredPosition.y);

        SelectionEnable(initialValue); //�R�[�X�����v�_��

        informationMg.courseNum = informationMg.data.recentCourseAdress;

        /*
        //�J�n���ɑI�������R�[�X�ԍ��ֈړ����鏈��
        Vector2 target = courseInfoList[initialValue].RectTransform.anchoredPosition;
        for (int i = 0; i < courseInfoList.Count; i++)
        {
            courseInfoList[i].RectTransform.anchoredPosition -= target;
        }
         */
    }

    private void Update()
    {
        UpdateSelection();
        if (!isDragging)
        {
            return;
        }
        if (nearestNumber != (int)NearestCourseInfo().courseNumber)
        {
            CourseInformation courseInfo = NearestCourseInfo();

            areaCtrl.MoveArea((int)courseInfo.courseNumber);
            SelectionEnable((int)courseInfo.courseNumber);
            courseFrameMg.SelectCourse();

            nearestNumber = (int)courseInfo.courseNumber;
            return;
        }
        courseFrameMg.RectTransform.anchoredPosition = NearestCourseInfo().RectTransform.anchoredPosition;//�Ǐ]
    }

    private void UpdateSelection()
    {
        foreach (var item in courseInfoList)
        {
            float distance = Mathf.Abs(item.RectTransform.anchoredPosition.y);
            float value_Distance = Mathf.Clamp(maxValue_Distance - (distance * factorDistance), 0, maxValue_Distance);
            Vector2 pos = item.RectTransform.anchoredPosition;
            pos.x = value_Distance;
            item.RectTransform.anchoredPosition = pos;

            float value_Scale = Mathf.Clamp(maxValue_Scale - (distance * factorScale), 0.95f, maxValue_Scale);
            Vector2 scale = item.RectTransform.anchoredPosition;
            scale.x = value_Scale;
            scale.y = value_Scale;
            item.RectTransform.localScale = scale;
        }
    }

    public void OnDrag(PointerEventData e)
    {
        float delta_y = (e.delta.y * dragSensitivity);// ����ʂɉ�����Y�����Ɉړ�����

        foreach (var item in courseInfoList)
        {
            RectTransform rect = item.RectTransform;
            var pos = rect.anchoredPosition;
            pos.y += delta_y;
            rect.anchoredPosition = pos;
        }
    }

    public void OnBeginDrag(PointerEventData e)
    {
        tweenList.KillAllAndClear();
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData e)
    {
        MagnetItems(NearestCourseInfo().RectTransform.anchoredPosition.y, DEFAULT);
    }

    private void DragCompleted() => isDragging = false;

    private CourseInformation NearestCourseInfo()// �}�O�l�b�g���S�ɍł��߂��v�f��I������
    {
        CourseInformation courseInfo = null;
        RectTransform nearestRect = null;
        foreach (var item in courseInfoList)
        {
            if (nearestRect == null)
            {
                nearestRect = item.RectTransform; // ����I��
                courseInfo = item;
            }
            else
            {
                if (Mathf.Abs(item.RectTransform.anchoredPosition.y) < Mathf.Abs(nearestRect.anchoredPosition.y))
                {
                    nearestRect = item.RectTransform; // ��蒆�S(0)�ɋ߂��ق���I��
                    courseInfo = item;
                }
            }
        }
        return courseInfo;
    }

    public void MoveUpArea()
    {
        int nearestNum = (int)NearestCourseInfo().courseNumber;
        if (courseInfoList.Count - 1 > nearestNum)
        {
            nearestNumber = nearestNum + 1;
            MoveCourse(nearestNumber);
            /*
            areaCtrl.MoveArea(nearestNumber);
            SelectionEnable(nearestNumber);
            MagnetItems(courseInfoList[nearestNumber].RectTransform.anchoredPosition.y);
             */
        }
    }
    public void MoveDownArea()
    {
        int nearestNum = (int)NearestCourseInfo().courseNumber;
        if (nearestNum >= 1)
        {
            nearestNumber = nearestNum - 1;
            MoveCourse(nearestNumber);
            /*
            areaCtrl.MoveArea(nearestNumber);
            SelectionEnable(nearestNumber);
            MagnetItems(courseInfoList[nearestNumber].RectTransform.anchoredPosition.y);
             */
        }
    }

    public void MoveCourse(int courseNum, float duration = DEFAULT)
    {
        nearestNumber = courseNum;
        areaCtrl.MoveArea(courseNum);
        SelectionEnable(courseNum);
        MagnetItems(courseInfoList[courseNum].RectTransform.anchoredPosition.y, duration);
    }

    void MagnetItems(float targetY, float duration)
    {
        courseFrameMg.RectTransform.DOComplete();
        courseFrameMg.RectTransform.anchoredPosition = new Vector2(maxValue_Distance, targetY);
        courseFrameMg.RectTransform.DOAnchorPosY(-targetY, duration).SetRelative(true).SetEase(courseType);
        courseFrameMg.SelectCourse();

        for (int i = 0; i < courseInfoList.Count; i++)
        {
            Tween t = courseInfoList[i].RectTransform.DOAnchorPosY(-targetY, duration).SetRelative(true).SetEase(courseType);

            if (i <= courseInfoList.Count)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(t);
                sequence.AppendCallback(DragCompleted);
                tweenList.Add(sequence);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }

    public void FadeOutItems()
    {
        courseFrameMg.RectTransform.DOAnchorPosY(650f, fadeDuration).SetRelative(true).SetEase(courseType);
        for (int i = 0; i < courseInfoList.Count; i++)
        {
            Tween t = courseInfoList[i].RectTransform.DOAnchorPosY(650f, fadeDuration).SetRelative(true).SetEase(courseType);

            if (i <= courseInfoList.Count)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(t);
                sequence.AppendCallback(DragCompleted);
                tweenList.Add(sequence);
            }
            else
            {
                tweenList.Add(t);
            }
        }
    }

    public void SelectionEnable(int courseNum)
    {
        foreach (var item in courseInfoList)//�S�Ă�Image��enable��Off
        {
            item.CheckEnable(false);
        }
        courseInfoList[courseNum].CheckEnable(true);

    }


}