using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("エフェクトがついた床を判定するか")] public bool checkPlatformGround = true;

    private string groundTag = "Ground"; //ただの床
    private string platformTag = "GroundPlatform";//すり抜ける床
    private string moveFloorTag = "MoveFloor";//動く床
    private string fallFloorTag = "FallFloor";//落ちる床
    private string elevatorTag = "Elevator";//エレベーター
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;


    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag || collision.tag == elevatorTag))
        {
            isGroundEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            //Debug.Log("地面が判定に入り続けています");
            isGroundStay = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag || collision.tag == elevatorTag))
        {
            isGroundStay = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            //Debug.Log("地面が判定を出ました");
            isGroundExit = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag || collision.tag == elevatorTag))
        {
            isGroundExit = true;

        }
    }

}
