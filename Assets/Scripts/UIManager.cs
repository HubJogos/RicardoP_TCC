using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerScript playerScript;//references to use healthbar
    public Slider healthBar;
    public Text hpText;
    public Slider ammoCounter;
    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
    }

    void Update()
    {
        ammoCounter.maxValue = playerScript.maxAmmo;
        ammoCounter.value = playerScript.currentAmmo;
        healthBar.maxValue = playerScript.maxHealth;
        healthBar.value = playerScript.currentHealth;//controls updating of healthbar
        hpText.text = "HP: " + playerScript.maxHealth + "/" + playerScript.currentHealth;
    }
}
