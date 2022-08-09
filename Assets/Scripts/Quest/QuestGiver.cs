using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Text trackerText;
    public Text expText;
    public Text healthText;

    public GameObject acceptButtonMage;
    public GameObject acceptButtonWarrior;
    QuestTracker tracker;

    public void OpenQuestWindow()
    {
        if(gameObject.name == "MageNialNPC")
        {
            acceptButtonMage.SetActive(true);
            acceptButtonWarrior.SetActive(false);
        }
        else
        {
            acceptButtonWarrior.SetActive(true);
            acceptButtonMage.SetActive(false);
        }
        questWindow.SetActive(true);
        FindObjectOfType<DialogueManager>().EndDialogue();
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        expText.text = quest.expReward.ToString();
        healthText.text = quest.healthImprovement.ToString();
    }
    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        tracker = FindObjectOfType<QuestTracker>();
        if (!tracker.quest[0].isActive)
        {
            tracker.quest[0] = quest;
        }
        else if(tracker.quest[0].goal != quest.goal) tracker.quest[1] = quest;
    }
    public void RefuseQuest()
    {
        questWindow.SetActive(false);
    }
}
