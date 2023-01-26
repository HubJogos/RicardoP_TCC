using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

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
    PersistentStats stats;
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
    [HideInInspector] public float waitToDamage = 3f;//time to wait before dealing another instance of damage
    public float nextDamage;
    [HideInInspector] public bool isTouching;//detects if player came into contact
    public float knockbackStrength = 5f;
    #endregion

    #region DashVariables
    [Header("Dash Variables")]
    private float rollSpeed;
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
    public int damage = 5;
    public GameObject damageTextPrefab;
    #endregion

    #region ShootingVariables
    [Header("TopDown Shooting")]
    public GameObject projectilePrefab;
    public float projectileForce = 25f;
    [HideInInspector] public Vector2 shootDirection;
    [HideInInspector] public float angle;
    [HideInInspector] public bool isShooting;
    [SerializeField] private float shootingTime = .05f;//time player must wait between attacks
    private float shootingCounter;//counts time to shoot again
    public int maxAmmo;
    public int currentAmmo;
    DataGenerator dataGen;
    public float firerate = 1f;
    float nextFire;
    float waitShoot;

    #endregion

    #region DataVariables
    [Header ("Data Variables")]
    MapGenAutomata mapReference;//referencia ao gerador
    [HideInInspector] public int[,] pathing;
    int currPlayerX, currPlayerY;
    int playerX, playerY;

    public float percentItemsCollected;
    [HideInInspector] public float itemsCollected;
    [HideInInspector] public float itemsGenerated;

    float ammoPickup=0;
    public float ammoPickupRate;
    float shotsFired = 0;
    public float attacksAttempted = 0;
    [HideInInspector] public float successfulAttacks = 0;
    public float precision;
    public int totalLifeLost = 0;

    public float percentKills;
    [HideInInspector] public float enemiesDefeated;
    public int steps = 0;
    public int coins = 0;
    Scene activeScene;

    public QuestTracker tracker;
    #endregion

    
    #region Sounds
    [Header ("Sounds")]
    AudioManager audioManager;
    #endregion



    void Start()
    {
        dataGen = FindObjectOfType<DataGenerator>();
        stats = FindObjectOfType<PersistentStats>();
        tracker = FindObjectOfType<QuestTracker>();
        audioManager = FindObjectOfType<AudioManager>();
        
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "MapGeneration")
        {
            mapReference = FindObjectOfType<MapGenAutomata>();

            itemsGenerated = mapReference.currentItems;
            playerX = Mathf.FloorToInt(transform.position.x + mapReference.width / 2);//conversão da posição global para a posição no mapa do jogador
            playerY = Mathf.FloorToInt(transform.position.y + mapReference.height / 2);
        }
        steps = 0;
        attacksAttempted = 0;

        attackCounter = attackTime;
        maxHealth = stats.maxHealth;
        currentExp = stats.currentExp;
        playerLevel = stats.playerLevel;

        currentHealth = maxHealth;//makes sure current health can't be greater than max health
        playerSprite = GetComponent<SpriteRenderer>();

        levelText.text = "Level: " + playerLevel;//sets screen text for player level
        expToLevelUp = new int[maxLevel];
        expToLevelUp[0] = baseExp;
        for (int i = 1; i < expToLevelUp.Length; i++)
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
        }//gerencia troca de movimentos de rolamento/caminhada  
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canRoll)
            {
                canRoll = false;
                isRolling = true;
                animator.SetBool("IsRolling", true);
                rollSpeed = defaultRollSpeed;
            }
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
        if (moveX == 1 || moveX == -1 || moveY == 1 || moveY == -1)
        {
            animator.SetFloat("lastMoveX", moveX);//required for correcting idle direction
            animator.SetFloat("lastMoveY", moveY);
        }//idle look direction

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
        if (Input.GetKeyDown(KeyCode.Mouse2))//attack button is set to left mouse button
        {
            if (!isAttacking)//checks if player is not attacking already
            {
                attackCounter = attackTime;//resets attack timer
                isAttacking = true;//sets attack variables
                if (Time.timeScale != 0)
                {
                    attacksAttempted++;
                }
                
                animator.SetBool("IsAttacking", true);
                audioManager.Play("PlayerSlash");
            }
        }//regulates attacking

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
            if (Time.time > nextDamage)
            {
                nextDamage = Time.time + waitToDamage;
                HurtPlayer(damage);//causa dano ao permanecer encostado na hitbox
                GameObject damageTextInstance = Instantiate(damageTextPrefab, transform);
                damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);
                damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());

            }
        }//controla recepção de dano ao permanecer tocando um inimigo
        
        animator.SetFloat("moveX", rb.velocity.x);//sets movement animation variables to animate correctly
        animator.SetFloat("moveY", rb.velocity.y);
        animator.SetFloat("mousePosX", mouseDirection.x);//variáveis direcionais de ataque
        animator.SetFloat("mousePosY", mouseDirection.y);//ataca na direção do mouse, não do movimento

        if (activeScene.name == "MapGeneration")
        {
            precision = successfulAttacks / attacksAttempted;
            ammoPickupRate = ammoPickup / shotsFired;
            percentItemsCollected = itemsCollected / itemsGenerated;
            percentKills = enemiesDefeated / mapReference.currentEnemies;
            playerX = Mathf.FloorToInt(transform.position.x + mapReference.width / 2);//conversão da posição global para a posição no mapa do jogador
            playerY = Mathf.FloorToInt(transform.position.y + mapReference.height / 2);
            if (playerX != currPlayerX || playerY != currPlayerY)//se mudar de posição
            {
                steps++;
            }
        }//controla dados do jogador no mapa gerado

        shootDirection = (mousePosition - rb.position).normalized;//aqui2
        angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 235f;


        if (Input.GetKeyDown(KeyCode.Mouse1))//Joga a faca com o botão direito do mouse
        {
            animator.SetBool("IsShooting", true);
            StartCoroutine(ExecuteAfterTime(firerate));
            Shoot();
            
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("IsShooting", false);
    }
    void LevelUp()
    {
        UpgradeHealth(10);
        stats.currentExp = stats.currentExp-expToLevelUp[playerLevel];
        currentExp = stats.currentExp;
        stats.playerLevel++;
        playerLevel = stats.playerLevel;
        levelText.text = "Level: " + playerLevel;
    }
    public void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + firerate;
            if (currentAmmo > 0)
            {
                currentAmmo--;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
                projectile.GetComponent<DaggerToss>().damage = damage;
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                projectileRb.AddForce(new Vector2(shootDirection.x, shootDirection.y) * projectileForce, ForceMode2D.Impulse);//aqui
                if (Time.timeScale != 0)
                {
                    attacksAttempted++;
                }
                shotsFired++;
            }
        }


    }//dispara projétil

    public void PickUpAmmo()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo++;
        }
        ammoPickup++;
    }//incrementa munição
    void GetExp(int amount)
    {
        stats.currentExp += amount;
        currentExp = stats.currentExp;
        if (currentExp >= expToLevelUp[playerLevel]){
            LevelUp();
        }
    }
    public void UpgradeHealth(int amount)
    {
        stats.maxHealth += amount;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
    }//incrementa munição
    public void GetCoin()
    {
        for(int i = 0; i < tracker.quest.Length; i++)
        {
            if (tracker.quest[i].goal.goalType == QuestGoal.GoalType.Gather && tracker.quest[i].isActive)
            {
                tracker.quest[i].goal.Gathered();
                if (tracker.quest[i].goal.IsReached())
                {
                    GetExp(tracker.quest[i].expReward);
                    UpgradeHealth(tracker.quest[i].healthImprovement);
                    tracker.quest[i].Complete();
                    dataGen.completedQuests++;
                    dataGen.activeQuests -= 1;
                }
            }
        }
        coins++;
        itemsCollected++;
    }

    public void DefeatEnemy(int exp)
    {
        isTouching = false;
        GetExp(exp);
        enemiesDefeated++;
        for (int i = 0; i < tracker.quest.Length; i++)
        {
            if (tracker.quest[i].goal.goalType == QuestGoal.GoalType.Kill && tracker.quest[i].isActive)
            {
                tracker.quest[i].goal.EnemyKilled();
                if (tracker.quest[i].goal.IsReached())
                {
                    GetExp(tracker.quest[i].expReward);
                    UpgradeHealth(tracker.quest[i].healthImprovement);
                    tracker.quest[i].Complete();
                    dataGen.completedQuests++;
                    dataGen.activeQuests -= 1;
                }
            }
        }

    }

    public void OnTriggerEnter2D(Collider2D other)//causa dano ao inimigo quando trigger ocorre
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthManager enemyHealthManager = other.GetComponent<EnemyHealthManager>();
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            enemyHealthManager.HurtEnemy(damage);
        }
    }//se colide com inimigo, recebe dano
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector2 knockback = transform.position - other.transform.position;
            int enemyDamage = other.gameObject.GetComponent<EnemyStats>().damage;
            HurtPlayer(enemyDamage);//causa dano na colisão
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(enemyDamage.ToString());
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);//red 
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
        totalLifeLost += damageTaken;
        if (currentHealth <= 0)
        {
            for (int i = 0; i < tracker.quest.Length; i++)
            {
                tracker.quest[i].goal.currentAmount = 0;
            }
            FindObjectOfType<Continue>().GameOverScreen();
        }
    }
}
