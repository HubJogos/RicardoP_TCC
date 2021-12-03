using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerHealthManager healthManager;//references to use healthbar
    public Slider healthBar;
    public Text hpText;
    public Slider ammoCounter;
    public TopDownShooting shootControls;
    void Start()
    {
        healthManager = FindObjectOfType<PlayerHealthManager>();
        shootControls = FindObjectOfType<TopDownShooting>();
    }

    void Update()
    {
        ammoCounter.maxValue = shootControls.maxAmmo;
        ammoCounter.value = shootControls.currentAmmo;
        healthBar.maxValue = healthManager.maxHealth;
        healthBar.value = healthManager.currentHealth;//controls updating of healthbar
        hpText.text = "HP: " + healthManager.maxHealth + "/" + healthManager.currentHealth;
    }
}
