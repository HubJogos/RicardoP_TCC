using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

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
        if (1 == '1')
        {
            Debug.Log("true");
        }
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

            //saving death info on matrix file

            string input = "Assets/deathMatrix_" + mapReference.seed + ".txt";//caminho da matriz de mortes
            string[] lines;

            if (File.Exists(input))//se matriz de mortes ja existe
            {
                lines = File.ReadAllLines(input);//lê todas as linhas como string
                File.Delete(input);//deleta existente para salvar corretamente
            }
            else//senão lê a matriz do mapa
            {
                lines = File.ReadAllLines(mapReference.input);
            }

            int[,] deathMatrix = new int[lines.Length, lines[0].Length];//declara uma matriz de mesmo tamanho do mapa para salvar as mortes
            //lê arquivo em forma de matriz
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '-')
                    {
                        deathMatrix[i, j] = -((int)char.GetNumericValue(lines[i][j + 1]));
                        j++;
                    }
                    else
                    {




                        //-------------------------------------------------------------------------------------------------------------------------------
                        deathMatrix[i, j] = (int)char.GetNumericValue(lines[i][j]);//salva cada caracter na posição correta da matriz
                    }
                    
                    
                }
            }
            
            deathMatrix[playerX, playerY]--;//marca posição da morte do jogador

            using (TextWriter tw = new StreamWriter(input))
            {

                for (int i = 0; i < lines.Length; i++)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        tw.Write(deathMatrix[i, j]);
                    }
                    tw.WriteLine();
                }
            }//salva matriz de mortes
            
        }
    }
}
