using UnityEngine;
using UnityEngine.UI;

public class CourseInformation : MonoBehaviour
{
    [Range(0, 10)] public int courseNumber;

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
