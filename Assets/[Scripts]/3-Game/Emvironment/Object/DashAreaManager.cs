using UnityEngine;

public class DashAreaManager : MonoBehaviour
{
    [Header("方向制限")] public KEY key;
    public enum KEY
    {
        [InspectorName("両方")] both = 0,
        [InspectorName("右→")] right = 1,
        [InspectorName("←左")] left = -1,
    }

    [Header("パワー")] public DashLevel dashPower;
    public enum DashLevel
    {
        [InspectorName("弱")] zero = 0,
        [InspectorName("中")] one,
        [InspectorName("強")] two,
        [InspectorName("特強")] three
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

            //if (grypsCrl.stageCrl.controlStatus == StageCtrl.ControlStatus.unControl) return;

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
            grypsCrl.stageCrl.jetMg.DisplayJetStock(grypsCrl.stageCrl.jetMg.stockNum + 1);
        }
    }
}
