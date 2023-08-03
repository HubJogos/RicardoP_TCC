using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMarket : MonoBehaviour
{
    // Start is called before the first frame update

    public TMPro.TextMeshProUGUI numberOfCoins;
    [SerializeField]
    public GameObject HudMarket;

    AudioManager audioManager;

    PlayerScript playerScript;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //numberOfCoins.text = playerScript.coins.ToString();
    }

    //Controla comportamento em relação ao alcance de interação
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                numberOfCoins.text = playerScript.coins.ToString();
                HudMarket.SetActive(true);
            }
            //interactionText.SetActive(true);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                numberOfCoins.text = playerScript.coins.ToString();
                HudMarket.SetActive(true);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //interactionText.SetActive(false);
            numberOfCoins.text = playerScript.coins.ToString();
            HudMarket.SetActive(false);
        }
    }

    public void comprarVida()
    {
        if (playerScript.coins >= 1)
        {
            Debug.Log("Jogador quer coins");
            playerScript.UpgradeHealth(25);
            playerScript.coins--;
            audioManager.PlayUnrestricted("CashRegister");
            Debug.Log("Upgrade de vida");
        } else
        {
            audioManager.PlayUnrestricted("CashDenied");
            Debug.Log("Não tem dinheiro");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void comprarVelocidade()
    {
        if (playerScript.coins >= 1)
        {
            Debug.Log("Jogador gosta de explorar");
            playerScript.speed = playerScript.speed + 2;
            playerScript.coins--;
            audioManager.PlayUnrestricted("CashRegister");
            Debug.Log("Upgrade de vida");
        }
        else
        {
            audioManager.PlayUnrestricted("CashDenied");
            Debug.Log("Não tem dinheiro");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void comprarForca()
    {
        if (playerScript.coins >= 1)
        {
            Debug.Log("Jogador gosta de combate.");
            playerScript.damage++;
            playerScript.coins--;
            audioManager.PlayUnrestricted("CashRegister");
            Debug.Log("Upgrade de força");
        }
        else
        {
            audioManager.PlayUnrestricted("CashDenied");
            Debug.Log("Não tem dinheiro");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void fechaHud()
    {
        numberOfCoins.text = playerScript.coins.ToString();
        HudMarket.SetActive(false);
    }

}
