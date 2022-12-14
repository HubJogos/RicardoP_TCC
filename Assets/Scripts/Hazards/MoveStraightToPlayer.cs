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
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        player = FindObjectOfType<PlayerScript>().transform;
        if (Vector2.Distance(transform.position, player.position) < range)
        {
            Vector2 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
            MoveTowards(movement);
        }
        else
        {
            Vector2 direction = rootPosition.position - transform.position;
            direction.Normalize();
            movement = direction;
            MoveTowards(movement);
        }
    }
    void MoveTowards(Vector2 target)
    {
        rb.velocity = target * speed;
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
