using UnityEngine;

public class DashAreaManager : MonoBehaviour
{
    [Header("��������")] public KEY key;
    public enum KEY
    {
        [InspectorName("����")] both = 0,
        [InspectorName("�E")] right = 1,
        [InspectorName("��")] left = -1,
    }

    //[Header("�p���[")] [Range(0, 3)] public int powerLevel;
    [Header("�p���[")] public DashLevel dashPower;
    public enum DashLevel
    {
        [InspectorName("��")] zero = 0,
        [InspectorName("��")] one,
        [InspectorName("��")] two
    }

    GameObject right, left;
    int KeyNum;

    private void Start()
    {
        right = transform.GetChild(0).gameObject;
        left = transform.GetChild(1).gameObject;

        switch (key)
        {
            case KEY.both:
                right.SetActive(true);
                left.SetActive(true);
                break;
            case KEY.right:
                right.SetActive(true);
                left.SetActive(false);
                break;
            case KEY.left:
                right.SetActive(false);
                left.SetActive(true);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (key == KEY.both)
            {
                if ((int)collision.gameObject.transform.localScale.x >= 1)
                {
                    KeyNum = 1;
                }
                else if ((int)collision.gameObject.transform.localScale.x <= -1)
                {
                    KeyNum = -1;
                }
            }
            else if (key == KEY.left || key == KEY.right)
            {
                KeyNum = (int)key;
            }
            collision.gameObject.GetComponent<GrypsController>().DashA(KeyNum, (int)dashPower);
            //collision.gameObject.GetComponent<GrypsController>().Dash((int)dashPower, directionLimit, (int)transform.localScale.x);
        }
    }
}
