using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTowardsPlayer : MonoBehaviour
{
    public Animator anim;
    public Transform homePos;//where enemy will return to
    private Transform target;

    [SerializeField]//variable is still private, but can be edited on unity editor
    private float speed = 0f;
    [SerializeField]
    private float maxRange = 0f;//maximum range that the enemy will keep pursuing player
    [SerializeField]
    private float minRange = 0f;//distance enemy will remain from player
    void Start()
    {
        anim = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
        {
            FollowPlayer();
        }
        else if (Vector3.Distance(target.position, transform.position) > maxRange)
        {
            GoHome();
        }
    }
    public void FollowPlayer()//called when player is between maximum and minimum range
    {
        anim.SetBool("isMoving", true);//activates moving animations
        anim.SetFloat("moveX", (target.position.x - transform.position.x));//controls movement animation variables
        anim.SetFloat("moveY", (target.position.y - transform.position.y));

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);//actually does the moving part
    }
    public void GoHome()//called when player is beyond maximum range
    {
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);//actually does the moving part
        anim.SetFloat("moveX", (homePos.position.x - transform.position.x));//controls movement animation variables
        anim.SetFloat("moveY", (homePos.position.y - transform.position.y));
        if (transform.position == homePos.position)
        {
            anim.SetBool("isMoving", false);//stops movement if reached home
        }
    }
    public void OnTriggerEnter2D(Collider2D other)//damage detection
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Vector2 difference = transform.position - other.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);//if is hit by weapon, gets knocked back
        }
    }
}
