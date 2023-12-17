using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAreaManager : MonoBehaviour
{
    public enum TUTORIAL
    {
        [InspectorName("Boost")] boost = 0,
        [InspectorName("Jet")] jet = 1,
        [InspectorName("Salt")] salto = -1,
    }
    public TUTORIAL tutorial;

    GrypsController grypsCrl;

    public void DisplayLead(TUTORIAL tutorial)
    {
        switch (tutorial)
        {
            case TUTORIAL.boost:
                if (grypsCrl.stageCrl.controlScreenMg.tutorialMg.transform.gameObject.activeSelf)
                {
                    grypsCrl.stageCrl.controlScreenMg.tutorialMg.LeadBoost();
                }
                break;

            case TUTORIAL.jet:
                //TutorialManager.LeadJet();
                break;

            case TUTORIAL.salto:
                //TutorialManager.LeadSalto();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            DisplayLead(tutorial);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (grypsCrl == null) grypsCrl = collision.gameObject.GetComponent<GrypsController>();
            grypsCrl.stageCrl.controlScreenMg.tutorialMg.HideLeadMark();
        }
    }

}
