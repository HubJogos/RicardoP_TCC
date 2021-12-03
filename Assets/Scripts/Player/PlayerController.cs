using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public Camera cam;
    private float moveX, moveY;
    private Vector3 mousePosition;
    private Vector3 mouseDirection;

    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float rollSpeed;
    [SerializeField]
    private float defaultRollSpeed = 1000f;
    [SerializeField]
    private float attackTime = .25f;//time player must wait between attacks
    private float attackCounter;
    [SerializeField]
    private float dashTime = .25f;//time player must wait between dashes
    private float rollCounter;
    public bool canRoll = true;
    private bool isAttacking;
    private bool isRolling;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackCounter = attackTime;
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");//receives directional inputs
        moveY = Input.GetAxisRaw("Vertical");
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);//recebe posição do mouse na tela
        mouseDirection = -(transform.position - mousePosition).normalized;//calcula vetor direcional do ataque, com valor absoluto = 1

        if (!isRolling)
        {
            rb.velocity = new Vector2(moveX, moveY).normalized * speed;//actually does the moving part
        }
        else
        {
            canRoll = false;
            rb.velocity = new Vector2(moveX, moveY).normalized * rollSpeed;
        }
        

        animator.SetFloat("moveX", rb.velocity.x);//sets movement animation variables to animate correctly
        animator.SetFloat("moveY", rb.velocity.y);
        animator.SetFloat("mousePosX", mouseDirection.x);//variáveis direcionais de ataque
        animator.SetFloat("mousePosY", mouseDirection.y);//ataca na direção do mouse, não do movimento

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
        }else
        {
            rollCounter = dashTime;
        }//regulates rolling timers
        if (isRolling)
        {
            float dashSlowForce = 3f;
            float minDashSpeed = speed;

            GetComponent<TrailRenderer>().enabled = true;
            rollSpeed -= rollSpeed * dashSlowForce * Time.deltaTime;

            if((rollSpeed < minDashSpeed))
            {
                isRolling = false;
                GetComponent<TrailRenderer>().enabled = false;
                animator.SetBool("IsRolling", false);
            }//stops rolling animation
        }//actually does the rolling part
    }
}
