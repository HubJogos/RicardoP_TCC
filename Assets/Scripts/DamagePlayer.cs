using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealthManager healthManager;
    public GameObject damageTextPrefab;
    public string textToDisplay;

    public float waitToDamage = 2f;//time to wait before dealing another instance of damage
    public bool isTouching;//detects if player came into contact
    private bool reload;
    [SerializeField]
    private float waitToRestart = 2f;//time to wait after player dies before restarting the scene

    [SerializeField]
    private int damage = 10;//damage to deal to player
    void Start()
    {
        healthManager = FindObjectOfType<PlayerHealthManager>();
    }

    void Update()
    {
        
        if (reload)//only true when player died
        {
            waitToRestart -= Time.deltaTime;//counts down restart timer
            if(waitToRestart <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//reloads current scene when timer reaches 0
                reload = false;
            }
        }
        

        if (isTouching)
        {
            waitToDamage -= Time.deltaTime;//counts down damage instance timer
            if (waitToDamage <= 0)
            {
                healthManager.HurtPlayer(damage);//actually does the damaging part
                GameObject damageTextInstance = Instantiate(damageTextPrefab, transform);
                damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);
                damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
                
                waitToDamage = 2f;//resets counter
            }
        }
        
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damage);
            GameObject damageTextInstance = Instantiate(damageTextPrefab, other.transform);
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
            damageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(200, 100, 100, 255);

            if (other.gameObject.GetComponent<PlayerHealthManager>().currentHealth <= 0)//if player health reaches 0
            {
                reload = true;
                other.gameObject.SetActive(false);//disables player
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)//detects if player remains in contact with an enemy
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouching = true;

        }
    }

    public void OnCollisionExit2D(Collision2D other)//resets "touching" status
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouching = false;
        }
    }
}
