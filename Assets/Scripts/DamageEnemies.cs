using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageEnemies : MonoBehaviour
{
    public int damage = 2; 
    public GameObject damageTextPrefab;
    public string textToDisplay;
    public void OnTriggerEnter2D(Collider2D other)//deals damage to enemy when trigger occurs
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthManager enemyHealthManager = other.GetComponent<EnemyHealthManager>();
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            enemyHealthManager.HurtEnemy(damage);
        }
    }
}
