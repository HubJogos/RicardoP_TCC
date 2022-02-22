using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth, currentHealth;
    private bool flashActive;
    [SerializeField]
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer enemySprite;
    [SerializeField]
    private int experienceGiven = 10;//experience the enemy will award player

    public GameObject daggerDrop;
    public int heldAmmo = 0;

    void Start()
    {
        currentHealth = maxHealth;
        enemySprite = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (flashActive)
        {
            if (flashCounter > flashLength * .99f)//flashes enemy in and out
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .82f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .66f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .49f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .33f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .16f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCounter > 0)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
                flashActive = false;//resets to not flashing
            }
            flashCounter -= Time.deltaTime;//counts down flash timer
        }//controls flashing when taking damage
    }
    public void HurtEnemy(int damage)
    {
        flashActive = true;//starts flashing when taking damage
        flashCounter = flashLength;
        currentHealth -= damage;//decreases health
        if (currentHealth <= 0)
        {
            PlayerScript giveExp = FindObjectOfType<PlayerScript>();
            giveExp.currentExp += experienceGiven;//updates player experience
            DropAmmo();
            Destroy(gameObject);//kills enemy
        }
    }
    public void DropAmmo()
    {
        for(int i = heldAmmo; i>=0; i--)
        {
            Instantiate(daggerDrop, transform.position, Quaternion.identity);
        }
    }
}
