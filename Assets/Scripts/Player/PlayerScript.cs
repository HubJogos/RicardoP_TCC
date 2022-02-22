using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    #region ControllerVariables
    private Rigidbody2D rb;
    private Animator animator;
    public Camera cam;
    private float moveX, moveY;
    private Vector2 mousePosition;
    private Vector3 mouseDirection;
    public float speed = 7f;
    #endregion

    #region PlayerStats
    [Header("Player Stats")]
    public int playerLevel = 1;
    public int maxLevel = 99;
    public int currentExp;
    public int baseExp = 100;
    public Text levelText;
    public int[] expToLevelUp;
    #endregion

    #region HealthVariables
    [Header("Health Variables")]
    public int maxHealth;
    public int currentHealth;
    private bool flashActive;//variable to flash player when hit
    [SerializeField] private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer playerSprite;
    [HideInInspector][SerializeField] int[,] deathMatrix;

    public float waitToDamage = 2f;//time to wait before dealing another instance of damage
    public bool isTouching;//detects if player came into contact
    #endregion

    #region DashVariables
    [Header("Dash Variables")]
    [SerializeField] private float rollSpeed;
    [SerializeField] private float defaultRollSpeed = 1000f;
    [HideInInspector] public bool canRoll = true;
    [SerializeField] private float dashTime = .25f;//time player must wait between dashes
    private float rollCounter;
    private bool isRolling;
    #endregion

    #region AttackVariables
    [Header("Attack Variables")]
    [SerializeField] private float attackTime = .25f;//time player must wait between attacks
    private float attackCounter;
    private bool isAttacking;
    public int damage = 2;
    public GameObject damageTextPrefab;
    public string textToDisplay;
    #endregion

    #region ShootingVariables
    [Header("TopDown Shooting")]
    public GameObject projectilePrefab;
    public float projectileForce = 20f;
    [HideInInspector] public Vector2 shootDirection;
    [HideInInspector] public float angle;
    [HideInInspector] public bool isShooting;
    [SerializeField] private float shootingTime = .25f;//time player must wait between attacks
    private float shootingCounter;//counts time to shoot again
    public int maxAmmo;
    public int currentAmmo;
    #endregion

    MapGenAutomata mapReference;//referencia ao mapa
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mapReference = FindObjectOfType<MapGenAutomata>();
        attackCounter = attackTime;

        currentHealth = maxHealth;//makes sure current health can't be greater than max health
        playerSprite = GetComponent<SpriteRenderer>();

        levelText.text = "Level: " + playerLevel;//sets screen text for player level
        expToLevelUp = new int[maxLevel];
        expToLevelUp[1] = baseExp;
        for (int i = 2; i < expToLevelUp.Length; i++)
        {
            expToLevelUp[i] = Mathf.FloorToInt(expToLevelUp[i - 1] * 1.1f);//sets experience thresholds for leveling up
        }

        shootingCounter = shootingTime;
        currentAmmo = maxAmmo;
    }

    void Update()//atualiza comandos de movimento o mais rápido possível
    {
        moveX = Input.GetAxisRaw("Horizontal");//receives directional inputs
        moveY = Input.GetAxisRaw("Vertical");
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);//recebe posição do mouse na tela
        mouseDirection = -((Vector2)transform.position - mousePosition).normalized;//calcula vetor direcional do ataque, com valor absoluto = 1

        if (!isRolling)
        {
            rb.velocity = new Vector2(moveX, moveY).normalized * speed;//actually does the moving part
        }
        else
        {
            canRoll = false;
            rb.velocity = new Vector2(moveX, moveY).normalized * rollSpeed;
        }        

        if (moveX == 1 || moveX == -1 || moveY == 1 || moveY == -1)
        {
            animator.SetFloat("lastMoveX", moveX);//required for correcting idle direction
            animator.SetFloat("lastMoveY", moveY);
        }//idle looking direction

        if (isAttacking)
        {
            canRoll = false;
            rb.velocity = Vector2.zero;//blocks movement when attacking
            attackCounter -= Time.deltaTime;//counts down attack timer
            if (attackCounter <= 0)
            {
                animator.SetBool("IsAttacking", false);
                isAttacking = false;//resets attack
                canRoll = true;
            }
        }//regulates attack timer

        if (Input.GetKeyDown(KeyCode.Mouse0))//attack button is set to left mouse button
        {
            if (!isAttacking)//checks if player is not attacking already
            {
                attackCounter = attackTime;//resets attack timer
                isAttacking = true;//sets attack variables
                animator.SetBool("IsAttacking", true);
            }
        }//regulates attacking

        if (canRoll && Input.GetKeyDown(KeyCode.Space))
        {
            canRoll = false;
            isRolling = true;
            animator.SetBool("IsRolling", true);
            rollSpeed = defaultRollSpeed;
        }//starts rolling
        if (!canRoll)
        {
            rollCounter -= Time.deltaTime;
            if (rollCounter <= 0)
            {
                canRoll = true;
            }//resets rolling
        }
        else
        {
            rollCounter = dashTime;
        }//regulates rolling timers
        if (isRolling)
        {
            float dashSlowForce = 3f;
            float minDashSpeed = speed;

            GetComponent<TrailRenderer>().enabled = true;
            rollSpeed -= rollSpeed * dashSlowForce * Time.deltaTime;

            if ((rollSpeed < minDashSpeed))
            {
                isRolling = false;
                GetComponent<TrailRenderer>().enabled = false;
                animator.SetBool("IsRolling", false);
            }//stops rolling animation
        }//actually does the rolling part
        if (flashActive)
        {
            if (flashCounter > flashLength * .99f)//flashes player in and out
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .82f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .66f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .49f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .33f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .16f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCounter > 0)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
                flashActive = false;//resets flashing
            }
            flashCounter -= Time.deltaTime;//counts down on flash times
        }//controls flashing when taking damage

        if (isTouching)
        {
            waitToDamage -= Time.deltaTime;//counts down damage instance timer
            if (waitToDamage <= 0)
            {
                HurtPlayer(damage);//causa dano ao permanecer encostado na hitbox
                GameObject damageTextInstance = Instantiate(damageTextPrefab, transform);
                damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);
                damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());

                waitToDamage = 2f;//resets counter
            }
        }

        animator.SetFloat("moveX", rb.velocity.x);//sets movement animation variables to animate correctly
        animator.SetFloat("moveY", rb.velocity.y);
        animator.SetFloat("mousePosX", mouseDirection.x);//variáveis direcionais de ataque
        animator.SetFloat("mousePosY", mouseDirection.y);//ataca na direção do mouse, não do movimento
    }
    void FixedUpdate()//60 vezes por segundo, atualiza os comandos de mirar e atirar
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        shootDirection = (mousePosition - rb.position).normalized;

        angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 235f;


        if (isShooting)
        {
            canRoll = false;
            rb.velocity = Vector2.zero;//blocks movement when shooting
            shootingCounter -= Time.fixedDeltaTime;//counts down attack timer
            if (shootingCounter <= 0)
            {
                animator.SetBool("IsShooting", false);
                isShooting = false;//resets shooting
                canRoll = true;
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
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.AddForce(new Vector2(shootDirection.x, shootDirection.y) * projectileForce, ForceMode2D.Impulse);
        }

    }//dispara projétil

    public void PickUpAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++;
        }

    }//incrementa munição

    public void OnTriggerEnter2D(Collider2D other)//causa dano ao inimigo quando trigger ocorre
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthManager enemyHealthManager = other.GetComponent<EnemyHealthManager>();
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            enemyHealthManager.HurtEnemy(damage);
        }
    }

    IEnumerator WaitToRestart()
    {
        yield return new WaitForSeconds(2);
    }//auxiliar para reiniciar o jogo

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            int enemyDamage = other.gameObject.GetComponent<EnemyStats>().damage;
            HurtPlayer(enemyDamage);//causa dano na colisão
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(enemyDamage.ToString());
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);//red

            if (currentHealth <= 0)
            {
                WaitToRestart();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//reloads current scene when timer reaches 0
            }
        }
    }//se colide com inimigo, recebe dano

    private void OnCollisionStay2D(Collision2D other)//detects if player remains in contact with an enemy
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isTouching = true;

        }
    }

    public void OnCollisionExit2D(Collision2D other)//resets "touching" status
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isTouching = false;
        }
    }

    public void HurtPlayer(int damageTaken)
    {
        flashActive = true;//begins flashing player
        flashCounter = flashLength;//resets health timer
        currentHealth -= damageTaken;//decreases health
    }
}
