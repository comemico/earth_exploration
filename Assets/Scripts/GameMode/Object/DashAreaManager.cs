using UnityEngine;

public class DashAreaManager : MonoBehaviour
{
    [Header("ï˚å¸êßå¿")] public KEY key;
    public enum KEY
    {
        [InspectorName("óºï˚")] both = 0,
        [InspectorName("âE")] right = 1,
        [InspectorName("ç∂")] left = -1,
    }

    [Header("ÉpÉèÅ[")] public DashLevel dashPower;
    public enum DashLevel
    {
        [InspectorName("é„")] zero = 0,
        [InspectorName("íÜ")] one,
        [InspectorName("ã≠")] two,
        [InspectorName("ì¡ã≠")] three
    }

    GrypsController grypsCrl;

    SpriteRenderer right, left;
    int KeyNum;

    private void Start()
    {
        right = transform.GetChild(0).GetComponent<SpriteRenderer>();
        left = transform.GetChild(1).GetComponent<SpriteRenderer>();

        switch (key)
        {
            case KEY.both:
                right.enabled = true;
                left.enabled = true;
                break;
            case KEY.right:
                right.enabled = true;
                left.enabled = false;
                break;
            case KEY.left:
                right.enabled = false;
                left.enabled = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();

            if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl) return;

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

            grypsCrl.ForceDash(KeyNum, (int)dashPower);
            grypsCrl.stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.restrictedControl);
            grypsCrl.stageCrl.jetMg.jetCountMg.DisplayJetCount(grypsCrl.stageCrl.jetMg.jetCountMg.jetNumber + 1);
        }
    }
}
