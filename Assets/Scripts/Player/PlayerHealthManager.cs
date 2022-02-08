using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private bool flashActive;//variable to flash player when hit
    [SerializeField]
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer playerSprite;
    void Start()
    {
        currentHealth = maxHealth;//makes sure current health can't be greater than max health
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashActive)
        {
            if (flashCounter > flashLength * .99f)//flashes player in and out
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if(flashCounter > flashLength * .82f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }else if (flashCounter > flashLength * .66f)
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
    }

    public void HurtPlayer(int damageTaken)
    {
        flashActive = true;//begins flashing player
        flashCounter = flashLength;//resets health timer
        currentHealth -= damageTaken;//decreases health
        if(currentHealth <= 0)//when player dies
        {
            MapGenAutomata mapReference = FindObjectOfType<MapGenAutomata>();//referencia ao mapa
            
            int playerX = Mathf.FloorToInt(gameObject.transform.position.x + mapReference.width / 2);
            int playerY = Mathf.FloorToInt(gameObject.transform.position.y + mapReference.height / 2);//posições da morte do jogador
            gameObject.SetActive(false);//kills player
            int[,] deathMatrix;

            //reads map matrix
            string path = "Assets/deathMatrix_" + mapReference.seed + ".json";
            if (File.Exists(path))//se já existe arquivo da matriz de mortes
            {
                deathMatrix = JsonConvert.DeserializeObject<int[,]>(path);
                File.Delete(path);
            }//usa o existente, mas deleta para salvar corretamente
            else//senão, quer dizer que nunca morreu, então usa o mapa gerado
            {
                deathMatrix = mapReference.map;
            }

            deathMatrix[playerX, playerY]--;//decrementa na posição da morte
            string heatMap = JsonConvert.SerializeObject(deathMatrix);
            using (var sw = new StreamWriter(path))
            {
                sw.Write(heatMap);
                sw.Flush();
                sw.Close();
            }
        }
    }

}
