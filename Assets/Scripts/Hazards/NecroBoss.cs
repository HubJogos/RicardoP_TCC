using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NecroBoss : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject afterFightDrops;
    [SerializeField] GameObject[] rewards;

    public Slider healthBar;
    public Text hpText;
    public Transform target;
    public bool activeFight = false;

    AudioManager audioManager;
    EnemyStats contactDamage;
    SpriteRenderer spriteRenderer;
    EnemyHealthManager health;
    LineRenderer lineRenderer;
    UIManager uiManager;

    bool loadedUI;
    bool deathAnim = false;
    bool endingFight = false;
    bool isStunned = false;
    bool targetAcquired = false;
    int prevDamage;//usado para trocar entre dano de contato e dano de carga(ver ataques)

    //movimenta��o err�tica
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDuration;
    float moveCounter;
    bool isMoving = false;
    //----------------


    //vari�veis que configuram ataques
    [SerializeField] float attackWaitTime = .75f;//tempo de espera antes de ataques (wind-up)

    [SerializeField] GameObject bossProjectile;//ataque de proj�til (deveria ricochetear?)
    [SerializeField] GameObject firingPos;
    [SerializeField] GameObject firingDir;
    [SerializeField] int projectileDamage;
    [SerializeField] int projectileForce;

    [SerializeField] GameObject tentacleAttack;//cria regi�o que causa dano no cen�rio
    [SerializeField] int tentacleDamage;

    [SerializeField] int chargeDamage;//ataque de carga
    [SerializeField] int chargeSpeed;
    [SerializeField] GameObject chargeDir;

    [SerializeField] float attackCooldown;//tempo para poder atacar novamente
    float attackCounter;
    bool isAttacking;

    //---------------
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        health = GetComponent<EnemyHealthManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>();
        contactDamage = gameObject.GetComponent<EnemyStats>();
        prevDamage = contactDamage.damage;

        afterFightDrops.SetActive(false);
        chargeDir.SetActive(false);
        healthBar.gameObject.SetActive(false);
        lineRenderer.enabled = false;
    }


    void Update()
    {
        if (!targetAcquired)
        {
            target = FindObjectOfType<PlayerScript>().transform;
            targetAcquired = true;
        }//recebe refer�ncia do jogador
        if (!loadedUI && uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            loadedUI = true;
        }//recebe refer�ncia a UI do jogador para controlar a troca de cena
        if (Vector2.Distance(target.position, transform.position) < 10)
        {
            activeFight = true;
            healthBar.gameObject.SetActive(true);
        }//ativa luta do chefe quando jogador estiver perto suficiente
        else return;
        //return acima cancela a execu��o do c�digo abaixo enquanto o jogador n�o ativar a luta

        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z-1)); //posi��o de onde a linha parte, do inimigo
                lineRenderer.SetPosition(1, new Vector3(target.position.x, target.position.y, target.position.z - 1)); //posi��o onde a linha termina, no jogador
        }//regula "mira" do inimigo, indicando a prepara��o de um proj�til

        if (health.currentHealth <= 0)
        {
            deathAnim = true;
            animator.SetBool("Die", deathAnim);
        }//regula anima��o de morte


        //Sequ�ncia abaixo define comportamento na luta. Essencialmente anda em qualquer dire��o, e ataca de uma maneira qualquer
        //da forma como foi constru�do, se inimigo pode se mover ou atacar, ataque possui prioridade

        if(attackCounter <= 0 && !isMoving)
        {
            isAttacking = true;//usado para ativar os efeitos visuais e sonoros que declaram ataques
            attackCounter = attackCooldown;
            moveSpeed = 0;//para de se mover enquanto prepara ataque
            RandomizeAttack();
            moveCounter -= Time.deltaTime;
        }//se pode atacar e parou de se mover, ataca
        else if(moveCounter <=0 && !isAttacking && !isMoving)
        {
            isMoving = true;
            ErraticMovement();
            attackCounter -= Time.deltaTime;
        }//se parou de se mover, n�o est� atacando e pode se mover, move novamente
        else
        {
            moveCounter -= Time.deltaTime;
            if(moveCounter <= 0)
            {
                isMoving = false;
            }
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0)
            {
                isAttacking = false;
            }
        }//se n�o se move, nem ataca, somente decrementa contadores
        
    }

    public void UpdateHealthBar()//n�o est� no Update por ser chamado a partir do "EnemyHealthManager" somente quando necess�rio, servindo somente para atualizar a barra de HP
    {
        healthBar.maxValue = health.maxHealth;
        healthBar.value = health.currentHealth;//controls updating of healthbar
        hpText.text = "HP: " + health.currentHealth + "/" + health.maxHealth;
        if(health.currentHealth <= 0)
        {
            EndFight();
        }
    }
    void ErraticMovement()
    {
        if (isStunned) return;//cancela movimento se est� atordoado

        //aleatoriza par�metros de movimenta��o, valores abaixo demonstraram boa variabilidade
        float xDir = Random.Range(-1f, 1f);
        float yDir = Random.Range(-1f, 1f);
        moveDuration = Random.Range(.5f, 1.5f);
        moveSpeed = Random.Range(2f, 6f);
        moveCounter = moveDuration;

        if (xDir < 0) //inverte orienta��o das sprites para que chefe se volte para a dire��o do movimento
        {
            spriteRenderer.flipX = true;
        }else spriteRenderer.flipX = false;

        rb.velocity = new Vector2(xDir, yDir).normalized * moveSpeed;

    }
    void RandomizeAttack()
    {
        if (isStunned) return;//cancela ataque se est� atordoado
        rb.velocity = Vector2.zero;
        int decider;
        decider = Mathf.FloorToInt(Random.Range(0, 3));//n�mero aleat�rio de 0 a 2
        animator.SetBool("IsAttacking", isAttacking);//ativa anima��o
        audioManager.PlayUnrestricted("BossAttacking");//toca efeito sonoro

        switch (decider)
        {
            case 0:
                ChargeAttack();
                break;
            case 1:
                ProjectileAttack();
                break;
            case 2:
                GroundAttack();
                break;
        }
    }
    #region Charge and Contact damage
    void ChargeAttack()
    {
        //se move mais r�pido em linha reta
        //usa OnCollisionEnter2D para detectar colis�o
        chargeDir.SetActive(true);//ativa seta indicando movimento
        Vector2 chargeDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized;
        float angle = Mathf.Atan2(chargeDirection.y, chargeDirection.x) * Mathf.Rad2Deg - 90f;//calcula �ngulo para onde seta aponta
        chargeDir.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//rotaciona seta para �ngulo correto
        StartCoroutine(Charge(attackWaitTime, chargeDirection));//inicia carga
    }//fun��o chamada para realizar ataque

    //fun��es auxiliares
    IEnumerator Charge(float secs, Vector2 direction)
    {
        yield return new WaitForSeconds(secs);//n�o se move at� que o tempo de espera acabe
        contactDamage.damage = chargeDamage;
        chargeDir.SetActive(false);//desabilita sprite de dire��o
        rb.velocity = direction * chargeSpeed;//o que realmente movimenta
    }
    IEnumerator StunRecovery(int secs)
    {
        yield return new WaitForSeconds(secs);
        isStunned = false;
        animator.SetBool("IsStunned", isStunned);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioManager.PlayUnrestricted("BossCollision");
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            //dano � causado atrav�s do script do jogador quando o jogador entra em contato com o inimigo desse script,
            //necessitando somente a altera��o do par�metro de dano de contato
            rb.velocity = Vector2.zero;
            isAttacking = false;
            animator.SetBool("IsAttacking", isAttacking);
        }//se acerta jogador
        else if(isAttacking)
        {
            //stop movement, wait a few seconds (stunned effect), restart erratic
            rb.velocity = Vector2.zero;
            isStunned = true;
            animator.SetBool("IsStunned", isStunned);
            isAttacking = false;
            animator.SetBool("IsAttacking", isAttacking);
            StartCoroutine(StunRecovery(1));//atordoamento dura 1 segundo
        }//se ainda estava atacando quando colidiu, ou seja, errou o ataque
        contactDamage.damage = prevDamage;
    }
    #endregion

    #region Spawning traps damage
    void GroundAttack()
    {
        //cria regi�o de dano no local do jogador ap�s tempo de espera
        StartCoroutine(SpawnGroundAttack(attackWaitTime));
    }
    IEnumerator SpawnGroundAttack(float secs)
    {
        yield return new WaitForSeconds(secs);
        GameObject groundAttack = Instantiate(tentacleAttack, target.transform.position, Quaternion.identity);
        groundAttack.GetComponent<GroundHazard>().damage = tentacleDamage;
        groundAttack.transform.localScale *= 3;
    }
    #endregion

    #region Projectile damage
    void ProjectileAttack()
    {
        StartCoroutine(WaitForShot(attackWaitTime));
    }
    IEnumerator WaitForShot(float secs)
    { 
        //enquanto proj�til n�o � disparado, mant�m atualiza��o da linha de mira
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;

        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        yield return new WaitForSeconds(secs);

        //ao disparar proj�til
        lineRenderer.enabled = false;//desativa linha de mira

        //calcula dire��o
        Vector3 targ = target.transform.position;
        targ.x = targ.x - transform.position.x;//argumento 1 subtra�do do argumento 2 resulta em dire��o partindo do argumento 2 em dire��o ao argumento 1
        targ.y = targ.y - transform.position.y;
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg - 90f;//calcula angulo
        firingDir.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //configura proj�til
        GameObject projectile = Instantiate(bossProjectile, firingPos.transform.position, Quaternion.identity);
        projectile.transform.localScale *= 3;//ao testar, proj�til era muito pequeno, essa altera��o poderia ser feita diretamente no prefab do proj�til
        projectile.GetComponent<EnemyProjectile>().damage = projectileDamage;

        //dispara proj�til e efeitos associados
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(firingPos.transform.up * projectileForce, ForceMode2D.Impulse);
        audioManager.PlayUnrestricted("BossProjectile");
        isAttacking = false;
        animator.SetBool("IsAttacking", isAttacking);
    }
    #endregion

    void EndFight()
    {
        if (!endingFight)
        {
            //pausa jogo ao terminar luta
            Time.timeScale = 0;
            //desabilita todos os tipos de dano
            chargeDamage = 0;
            projectileDamage = 0;
            tentacleDamage = 0;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);//torna inimigo invis�vel
            afterFightDrops.SetActive(true);//abre menu de sele��o de recompensa
            endingFight = true;//garante que essa fun��o seja chamada somente uma vez
            audioManager.PlayUnrestricted("BossDeath");
            StopAllCoroutines();//para quaisquer execu��es ativas
        }
    }
    public void SelectReward(int slot)
    {
        //mais da esquerda � o slot 0
        //mais da direita � o slot 2

        //c�digo para controlar recep��o do item deve vir aqui---------------
        Debug.Log("reward selected");//placeholder para testar funcionalidade
        //-------------------------------------------------------------------

        Time.timeScale = 1;//despausa jogo ap�s sele��o
        StartCoroutine(uiManager.FadeOut("Game", true, 1));//retorna para a cena da cidade inicial
        StartCoroutine(WaitToDie(2));
    }
    IEnumerator WaitToDie(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(gameObject);//destroi gameObject do inimigo ap�s um curto tempo de espera para garantir bom funcionamento das execu��es em progresso
    }

}
