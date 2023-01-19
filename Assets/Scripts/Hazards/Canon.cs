using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public float rotationSpeed;
    public float range;
    public float rateOfFire;
    float rofCounter;
    public float projectileSpeed;
    public Transform firePoint;
    public GameObject projectile;
    bool isFiring;
    Vector2 playerPos;
    Transform player;
    private void Start()
    {
        player = FindObjectOfType<PlayerScript>().transform;

    }
    private void FixedUpdate()
    {
        playerPos = player.position;
        Vector2 aimDir = playerPos - new Vector2(transform.position.x,transform.position.y);
        float angle = Mathf.Atan2(aimDir.y,aimDir.x)*Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);

        if (isFiring)
        {
            rofCounter -= Time.fixedDeltaTime;//counts down attack timer
            if (rofCounter <= 0)
            {
                isFiring = false;//resets shooting
            }
        }

        if (Vector2.Distance(playerPos, transform.position) < range && !isFiring)
        {

            rofCounter = rateOfFire;//resets attack timer
            isFiring = true;//sets attack variables
            ShootPlayer();            
        }

    }
    void ShootPlayer()
    {
        GameObject shot = Instantiate(projectile, firePoint.position, transform.rotation);
        Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
        shot.GetComponent<EnemyProjectile>().damage = GetComponent<EnemyStats>().damage;
        rb.AddForce(firePoint.up * projectileSpeed, ForceMode2D.Impulse);
    }
}
