using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialScreen;
    PersistentStats persistentStats;
    PlayerScript player;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        persistentStats = FindObjectOfType<PersistentStats>();
        if (!persistentStats.closedTutorial)
        {
            OpenTutorial();
        }
    }
    public void OpenTutorial()
    {
        Time.timeScale = 0;
        tutorialScreen.SetActive(true);
    }
    public void CloseTutorial()
    {
        if (player.attacksAttempted != 0) player.attacksAttempted = 0;
        tutorialScreen.SetActive(false);
        persistentStats.closedTutorial = true;
        Time.timeScale = 1;
    }
}
