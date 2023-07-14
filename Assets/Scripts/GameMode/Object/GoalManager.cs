using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalManager : MonoBehaviour
{
    private GrypsManager grypsMg;
    public TimelineManager timelineMg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsMg == null)
            {
                grypsMg = collision.GetComponent<GrypsManager>();
            }
            if (!grypsMg.isFreeze)
            {
                grypsMg.PausePlayer();
                grypsMg.transform.DOMove(this.gameObject.transform.position, 0.25f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    timelineMg.TapeChangeUI(1);
                    timelineMg.PlayTimeline();
                });
                //grypsMg.transform.DOLocalMove(new Vector3(30, 0, 0), 0.25f).SetRelative(true);

            }
        }
    }
}
