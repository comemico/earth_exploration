using UnityEngine;

public class DashAreaManager : MonoBehaviour
{
    [Header("•ûŒü§ŒÀ")] public KEY key;
    public enum KEY
    {
        [InspectorName("—¼•û")] both = 0,
        [InspectorName("‰E")] right = 1,
        [InspectorName("¶")] left = -1,
    }

    [Header("ƒpƒ[")] [Range(0, 3)] public int powerLevel;

    GameObject right, left;

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
    int test;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (key == KEY.both)
            {
                if ((int)collision.gameObject.transform.localScale.x >= 1)
                {
                    test = 1;
                }
                else if ((int)collision.gameObject.transform.localScale.x <= -1)
                {
                    test = -1;
                }
            }
            else if (key == KEY.left || key == KEY.right)
            {
                test = (int)key;
            }
            collision.gameObject.GetComponent<GrypsController>().DashA(test, powerLevel);
            //collision.gameObject.GetComponent<GrypsController>().Dash((int)dashPower, directionLimit, (int)transform.localScale.x);
        }
    }
}
