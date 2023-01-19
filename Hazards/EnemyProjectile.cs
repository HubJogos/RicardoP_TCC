using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public int damage;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity); //para efeitos após acerto
        //Destroy(effect, 2f);
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerScript>().HurtPlayer(damage);//causa 2 de dano ao acertar inimigo
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            Destroy(gameObject);
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

    }
}
