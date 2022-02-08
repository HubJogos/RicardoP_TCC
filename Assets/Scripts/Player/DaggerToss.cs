using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerToss : MonoBehaviour
{
    public GameObject daggerDrop;
    //public GameObject hitEffect;//animation for projectile explosion
    public void OnTriggerEnter2D(Collider2D other)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 2f);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealthManager>().heldAmmo++;
            Destroy(gameObject);
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(daggerDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
