using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialScreen;
    private void Start()
    {
        if (!FindObjectOfType<PersistentStats>().closedTutorial)
        {
            OpenTutorial();
        }
    }
    public void OpenTutorial()
    {
        tutorialScreen.SetActive(true);
    }
    public void CloseTutorial()
    {
        tutorialScreen.SetActive(false);
        FindObjectOfType<PersistentStats>().closedTutorial = true;
    }
}
