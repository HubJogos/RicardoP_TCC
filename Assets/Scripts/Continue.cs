using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    public GameObject gameOverScreen;
    bool gameHasEnded = false;
    private void Start()
    {
        gameOverScreen.SetActive(false);
        gameHasEnded = false;
    }
    public void GameOverScreen()
    {
        if (!gameHasEnded)
        {
            FindObjectOfType<DataGenerator>().deathCounter++;
            gameHasEnded = true;
            gameOverScreen.SetActive(true);
        }
    }
    public void EndGame()
    {
        FindObjectOfType<DataGenerator>().SaveAsCSV();
        SceneManager.LoadScene("Questionario");
    }
    public void Restart()
    {
        FindObjectOfType<DataGenerator>().continues++;
        FindObjectOfType<DataGenerator>().SaveAsCSV();
        gameOverScreen.SetActive(false);
        gameHasEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
