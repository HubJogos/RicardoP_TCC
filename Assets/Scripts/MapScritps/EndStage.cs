using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndStage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<DataGenerator>().playthroughs++;
            if(FindObjectOfType<DataGenerator>().completedQuests < 2)
            {
                FindObjectOfType<DataGenerator>().SaveAsCSV();
                SceneManager.LoadScene("Game");//reloads town scene
            }
            else
            {
                FindObjectOfType<DataGenerator>().SaveAsCSV();
                SceneManager.LoadScene("Questionario");//vai pro questionario
            }
            //com 1 quest, retorna a cidade
            FindObjectOfType<DataGenerator>().SaveAsCSV();//somente ao completar ambas as quests
        }
    }

    IEnumerator WaitToRestart()
    {
        yield return new WaitForSeconds(2);
    }//auxiliar para reiniciar o jogo
}
