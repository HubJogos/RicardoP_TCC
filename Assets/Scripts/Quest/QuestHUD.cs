using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestHUD : MonoBehaviour
{
    QuestTracker tracker;
    public GameObject tracker1, tracker2;
    // Start is called before the first frame update
    void Start()
    {
        tracker = FindObjectOfType<QuestTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tracker.quest[0].isActive)
        {
            tracker1.SetActive(true);
            tracker1.GetComponentInChildren<Text>().text = tracker.quest[0].tracker + tracker.quest[0].goal.currentAmount + " \\ " + tracker.quest[0].goal.requiredAmount;
        }
        else tracker1.SetActive(false);
        if (tracker.quest[1].isActive)
        {
            tracker2.SetActive(true);
            tracker2.GetComponentInChildren<Text>().text = tracker.quest[1].tracker + tracker.quest[1].goal.currentAmount + " \\ " + tracker.quest[1].goal.requiredAmount;
        }
        else tracker2.SetActive(false);
    }
}
