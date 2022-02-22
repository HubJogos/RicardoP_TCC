using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerScript>().PickUpAmmo();
            Destroy(gameObject);
        }
    }
}
