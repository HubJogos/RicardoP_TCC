using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script associado aos NPCs que atribuem missões, mas é referenciado por diversos outros componentes

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    //Interface visual para descrever missão e dar a opção de aceitar ou recusar
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Text trackerText;
    public Text expText;
    public Text healthText;

    public GameObject acceptButtonMage;//foi criado um botão para cada NPC na interface, deve existir uma maneira melhor de gerenciar qual missão aceitar
    public GameObject acceptButtonWarrior;

    QuestTracker tracker;
    DataGenerator dataGen;
    PlayerScript player;
    bool thisDialogueActive;

    [SerializeField]
    public GameObject NpcPortal;

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
        }//assume que jogador recusou a missão caso caminhe muito longe do "QuestGiver"
    }

    //Funções abaixo são chamadas a partir de botões dentro do jogo
    public void OpenQuestWindow()
    {
        //ao clicar na opção "missão" na caixa de diálogo
        thisDialogueActive = true;

        //deve haver uma maneira melhor de fazer essa etapa do "if"
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
        if (dataGen.activeQuests < 2)
        {
            questWindow.SetActive(false);
            thisDialogueActive = false;
            quest.isActive = true;
            if (!tracker.quest[0].isActive)
            {
                tracker.quest[0] = quest;
            }
            else if (tracker.quest[0].goal != quest.goal) tracker.quest[1] = quest;
            dataGen.activeQuests += 1;



            if (NpcPortal) //homem portal está referenciado
            {
                if (!NpcPortal.active)
                {
                    NpcPortal.SetActive(true);
                    dataGen.missaoPrincipal = "Fale com o homem encapuzado. Ele está na parte de cima do vilarejo.";
                }
            }
        }
        

    }
    public void RefuseQuest()
    {
        questWindow.SetActive(false);
        thisDialogueActive = false;
    }
}
