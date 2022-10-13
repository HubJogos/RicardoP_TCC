using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerScript>().UpgradeHealth(25);
            FindObjectOfType<DataGenerator>().foundSecret = true;
            Destroy(gameObject);
        }
    }
}
