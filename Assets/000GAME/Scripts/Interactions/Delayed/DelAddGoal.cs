using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelAddGoal : DelayedReactionBase
{

    public int goalDBIndex;               // The item asset to be added to the Inventory.
    public GameObject Notification;


    private DD_GameManager.GameGoalsManager goalManager;    // Reference to the Inventory component.


    protected override void SpecificInit()
    {

        goalManager = GameObject.FindObjectOfType<DD_GameManager.GameGoalsManager>();
    }


    protected override void ImmediateReaction()
    {
        Debug.Log("Called DelAddGoal" + gameObject.name); 
        goalManager.AddGoalToDo(goalDBIndex);
        if (Notification != null)
        {
            GameObject addNotif = Instantiate(Notification, Notification.transform.parent);
            Text notificatioText = addNotif.transform.Find("GoalFrame/GoalText").GetComponent<Text>();
            notificatioText.text = LocalizableData.instance.languageText[DD_GameManager.GameGoalsList.instance.gameGoals[goalDBIndex].titleIdx];
            addNotif.SetActive(true);
            addNotif.GetComponent<Animator>().SetTrigger("ShowNotification");
        }
    }
}
