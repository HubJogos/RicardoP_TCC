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
    Animator anim;
    Rigidbody2D rb;
    PlayerScript playerRef;

    public GameObject daggerDrop;
    public int heldAmmo = 0;
    PlayerScript playerScript;
    public bool huntPlayer;

    void Start()
    {
        huntPlayer = false;
        playerRef = FindObjectOfType<PlayerScript>();
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        enemySprite = GetComponent<SpriteRenderer>();
        playerScript = FindObjectOfType<PlayerScript>();
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
        if (anim)
        {
            if (rb.velocity.x != 0 || rb.velocity.y != 0)
            {
                anim.SetBool("isMoving", true);//activates moving animations
                anim.SetFloat("moveX", (playerRef.transform.position.x - transform.position.x));//controls movement animation variables
                anim.SetFloat("moveY", (playerRef.transform.position.y - transform.position.y));
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
    }
    public void HurtEnemy(int damage)
    {
        huntPlayer = true;
        playerScript.successfulAttacks++;
        flashActive = true;//starts flashing when taking damage
        flashCounter = flashLength;
        currentHealth -= damage;//decreases health
        if (currentHealth <= 0)
        {
            playerScript.DefeatEnemy(experienceGiven);
            if (heldAmmo > 0)
            {
                DropAmmo();
            }
            Destroy(gameObject);//kills enemy
        }
    }
    public void DropAmmo()
    {
        for(int i = heldAmmo; i>0; i--)
        {
            float distX = Random.Range(0f, 2f);
            float distY = Random.Range(0f, 2f);
            if (i == heldAmmo)
            {
                Vector2 pos = transform.position;
                Instantiate(daggerDrop, pos, Quaternion.identity);

            }
            else
            {
                Vector2 pos = new Vector2(transform.position.x + distX, transform.position.y + distY);
                Instantiate(daggerDrop, pos, Quaternion.identity);
            }
        }
    }
}
