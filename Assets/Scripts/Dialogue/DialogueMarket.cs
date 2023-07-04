using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMarket : MonoBehaviour
{
    // Start is called before the first frame update

    public TMPro.TextMeshProUGUI numberOfCoins;
    [SerializeField]
    public GameObject HudMarket;

    PlayerScript playerScript;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
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
            playerScript.UpgradeHealth(25);
            playerScript.coins--;
            Debug.Log("Upgrade de vida");
        } else
        {
            Debug.Log("Não tem dinheiro");
        }
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void comprarVelocidade()
    {
        Debug.Log("Em falta");
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void comprarForca()
    {
        Debug.Log("Em falta");
        numberOfCoins.text = playerScript.coins.ToString();
    }

    public void fechaHud()
    {
        numberOfCoins.text = playerScript.coins.ToString();
        HudMarket.SetActive(false);
    }

}
