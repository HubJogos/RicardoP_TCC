using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndStage : MonoBehaviour
{
    DataGenerator dataGen;
    private void Start()
    {
        dataGen = FindObjectOfType<DataGenerator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        dataGen.SaveAsCSV();
        SceneManager.LoadScene("Questionario");//vai pro questionario
        
    }

    

}
