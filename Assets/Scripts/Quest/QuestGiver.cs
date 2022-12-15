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
    DataGenerator dataGen;
    PlayerScript player;
    bool thisDialogueActive;

    private void Start()
    {
        dataGen = FindObjectOfType<DataGenerator>();
        tracker = FindObjectOfType<QuestTracker>();
        player = FindObjectOfType<PlayerScript>();
        thisDialogueActive = false;
    }
    private void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 5f && thisDialogueActive)
        {
            RefuseQuest();
        }
    }


    public void OpenQuestWindow()
    {
        thisDialogueActive = true;
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
        thisDialogueActive = false;
        quest.isActive = true;
        if (!tracker.quest[0].isActive)
        {
            tracker.quest[0] = quest;
        }
        else if(tracker.quest[0].goal != quest.goal) tracker.quest[1] = quest;
        dataGen.activeQuests += 1;
    }
    public void RefuseQuest()
    {
        questWindow.SetActive(false);
        thisDialogueActive = false;
    }
}
