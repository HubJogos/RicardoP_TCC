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
    DataGenerator dataGen;
    PersistentStats stats;

    PlayerScript playerScript;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        dataGen = FindObjectOfType<DataGenerator>();
        stats = FindObjectOfType<PersistentStats>();
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
                numberOfCoins.text = stats.coins.ToString();
                dataGen.visitouMercado = true;
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
                numberOfCoins.text = stats.coins.ToString();
                HudMarket.SetActive(true);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //interactionText.SetActive(false);
            numberOfCoins.text = stats.coins.ToString();
            HudMarket.SetActive(false);
        }
    }

    public void comprarVida()
    {
        if (stats.coins >= 1)
        {
            playerScript.UpgradeHealth(25);
            stats.coins--;
            audioManager.PlayUnrestricted("CashRegister");
            dataGen.comprouVida = true;
            Debug.Log("Upgrade de vida");
        } else
        {
            audioManager.PlayUnrestricted("CashDenied");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void comprarVelocidade()
    {
        if (stats.coins >= 1)
        {
            playerScript.speed++;
            stats.coins--;
            audioManager.PlayUnrestricted("CashRegister");
            dataGen.comprouVelocidade = true;
            Debug.Log("Upgrade de velocidade");
        }
        else
        {
            audioManager.PlayUnrestricted("CashDenied");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void comprarForca()
    {
        if (stats.coins >= 1)
        {
            playerScript.damage++;
            stats.coins--;
            audioManager.PlayUnrestricted("CashRegister");
            dataGen.comprouForca = true;
            Debug.Log("Upgrade de força");
        }
        else
        {
            audioManager.PlayUnrestricted("CashDenied");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void fechaHud()
    {
        numberOfCoins.text = playerScript.coins.ToString();
        HudMarket.SetActive(false);
    }

}
