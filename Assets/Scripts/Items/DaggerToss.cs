using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DaggerToss : MonoBehaviour
{
    public GameObject daggerDrop;
    public GameObject damageTextPrefab;
    public int damage;
    //public GameObject hitEffect;//animation for projectile explosion
    public void OnTriggerEnter2D(Collider2D other)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity); //para efeitos após acerto
        //Destroy(effect, 2f);
        if (other.gameObject.CompareTag("Enemy"))
        {
            Transform pos = other.transform;
            other.gameObject.GetComponent<EnemyHealthManager>().heldAmmo++;
            other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(2);//causa 2 de dano ao acertar inimigo
            GameObject damageTextInstance = Instantiate(damageTextPrefab, pos);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(255, 255, 255, 255);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            Destroy(gameObject);
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(daggerDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
