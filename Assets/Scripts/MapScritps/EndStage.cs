using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndStage : MonoBehaviour
{
    public GameObject partialQuestions;
    DataGenerator dataGen;
    private void Start()
    {
        dataGen = FindObjectOfType<DataGenerator>();
        partialQuestions = GameObject.FindGameObjectWithTag("PartialQuestions").gameObject;
        partialQuestions.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(dataGen.completedQuests < 2)
            {
                partialQuestions.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                dataGen.SaveAsCSV();
                SceneManager.LoadScene("Questionario");//vai pro questionario
            }
            //com 1 ou menos quests, exibe questionário parcial
        }
    }

    

}
