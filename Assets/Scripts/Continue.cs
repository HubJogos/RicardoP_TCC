using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    DataGenerator dataGen;
    public GameObject gameOverScreen;
    bool gameHasEnded = false;

    private void Start()
    {
        
        dataGen = FindObjectOfType<DataGenerator>();
        gameOverScreen.SetActive(false);
        gameHasEnded = false;
    }
    public void GameOverScreen()
    {
        Time.timeScale = 0;
        if (!gameHasEnded)
        {
            dataGen.deathCounter++;
            gameHasEnded = true;
            gameOverScreen.SetActive(true);
        }
    }
    public void EndGame()
    {
        dataGen.SaveAsCSV();
        SceneManager.LoadScene("Questionario");
    }
    public void Restart()
    {
        gameOverScreen.SetActive(false);
        gameHasEnded = false;
        Time.timeScale = 1;
        dataGen.SaveAsCSV();
        SceneManager.LoadScene("MapGeneration");
    }
}
