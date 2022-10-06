using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraightToPlayer : MonoBehaviour
{
    Transform player;
    public Transform rootPosition;
    public float speed;
    public float range;
    Rigidbody2D rb;
    Vector2 movement;
    EnemyHealthManager enemyHealth;
    float huntDuration = 5f;
    float huntCounter;
    private void Start()
    {        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerScript>().transform;
        enemyHealth = GetComponent<EnemyHealthManager>();
    }
    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.position) < range || (enemyHealth.huntPlayer && huntCounter > 0))
        {
            Vector2 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            MoveTowards(movement);
            if (enemyHealth.huntPlayer)
            {
                huntCounter -= Time.deltaTime;
            }
        }
        else
        {
            ReturnToSpawnPoint();
            huntCounter = huntDuration;
            enemyHealth.huntPlayer = false;
        }
    }
    public void MoveTowards(Vector2 target)
    {
        rb.velocity = target * speed;
    }

    void ReturnToSpawnPoint()
    {
        Vector2 direction = rootPosition.position - transform.position;
        direction.Normalize();
        movement = direction;
        MoveTowards(movement);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Vector2 difference = transform.position - other.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);
        }
    }
}
