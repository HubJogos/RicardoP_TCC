using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<DataGenerator>().SaveAsCSV();
        }
    }
}
