using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    public Rigidbody2D rb;//script needs to be attached to an object with a rigidbody2d
    public Camera cam;
    Vector2 mousePosition;
    public GameObject projectilePrefab;
    public float projectileForce = 20f;
    public Vector2 shootDirection;
    public float angle;
    Animator animator;

    public bool isShooting;
    [SerializeField]
    private float shootingTime = .25f;//time player must wait between attacks
    private float shootingCounter;//counts time to shoot again

    public int maxAmmo;
    public int currentAmmo;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shootingCounter = shootingTime;
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        shootDirection = (mousePosition - rb.position).normalized;

        angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 235f;


        if (isShooting)
        {
            GetComponent<PlayerController>().canRoll = false;
            rb.velocity = Vector2.zero;//blocks movement when shooting
            shootingCounter -= Time.fixedDeltaTime;//counts down attack timer
            if (shootingCounter <= 0)
            {
                animator.SetBool("IsShooting", false);
                isShooting = false;//resets shooting
                GetComponent<PlayerController>().canRoll = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isShooting)
            {
                shootingCounter = shootingTime;//resets attack timer
                isShooting = true;//sets attack variables
                animator.SetBool("IsShooting", true);
                Shoot();
            }
            
        }
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            GameObject projectile = Instantiate(projectilePrefab, rb.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.AddForce(new Vector2(shootDirection.x, shootDirection.y) * projectileForce, ForceMode2D.Impulse);
        }
        
    }
    public void PickUpAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++;
        }
        
    }
}
