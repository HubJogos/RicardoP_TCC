using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity); //para efeitos após acerto
        //Destroy(effect, 2f);
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerScript>().HurtPlayer(damage);//causa 2 de dano ao acertar inimigo
            Destroy(gameObject);
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

    }
}
