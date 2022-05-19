using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public GameObject dialogueBox;
    public Text nameText;
    public Text dialogueText;
    //public Animator animator;
    public GameObject interactionText;
    public GameObject questButtonMage;
    public GameObject questButtonWarrior;

    public QuestGiver questGiver;
    public bool activeDialogue = false;
    void Start()
    {
        sentences = new Queue<string>();
        //animator = GetComponent<Animator>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        if (!questGiver)
        {
            questButtonMage.SetActive(false);
            questButtonWarrior.SetActive(false);
        }
        else
        {
            if (questGiver.name == "MageNialNPC")
            {
                questButtonMage.SetActive(true);
                questButtonWarrior.SetActive(false);
            }
            if (questGiver.name == "WarriorDaveNPC")
            {
                questButtonWarrior.SetActive(true);
                questButtonMage.SetActive(false);
            }
            FindObjectOfType<QuestTracker>().quest = questGiver.quest;
        }

        interactionText.SetActive(false);
        activeDialogue = true;
        //animator.SetBool("IsOpen", activeDialogue);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        interactionText.SetActive(true);
        activeDialogue = false;
        dialogueBox.SetActive(false);
        //animator.SetBool("IsOpen", activeDialogue);
    }
}
