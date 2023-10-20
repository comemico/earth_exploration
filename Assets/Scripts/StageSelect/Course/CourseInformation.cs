using UnityEngine;
using UnityEngine.UI;

public class CourseInformation : MonoBehaviour
{
    public enum CourseNumber
    {
        Course_01 = 0,
        Course_02,
        Course_03,
        Course_04,
        Course_05,
        Course_06,
        Course_07,
        Course_08,
        Course_09,
        Course_10
    }

    public CourseNumber courseNumber;
    public string courseName;


    public RectTransform RectTransform => transform as RectTransform;
    private CourseController courseCtrl;

    void Start()
    {
        courseCtrl = GetComponentInParent<CourseController>();
        GetComponentInChildren<Button>().onClick.AddListener(CourseClick);
    }
    void CourseClick()
    {
        courseCtrl.MoveCourse((int)courseNumber);
    }

    public void CheckEnable(bool interactible)
    {
        transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().enabled = interactible;
    }

}
