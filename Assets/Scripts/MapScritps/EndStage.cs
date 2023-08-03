﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//script utilizado para criar um portal de saída para o jogador após cumprir missões ativas

public class EndStage : MonoBehaviour
{
    DataGenerator dataGen;
    MapGenAutomata mapReference;
    PlayerScript player;
    bool finished;


    [SerializeField]
    public GameObject spawnPlayerInBoss;

    private void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        mapReference = FindObjectOfType<MapGenAutomata>();
        dataGen = FindObjectOfType<DataGenerator>();
        finished = false;
    }
    private void Update()
    {
        //if (dataGen.activeQuests <= 0 && !finished)
        //{
        //    transform.position = FindSpawningPoint();
        //    finished = true;
        //}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")){

            dataGen.SaveAsCSV();
            dataGen.ato++;

            switch (dataGen.ato)
            {
                case 1:
                    SceneManager.LoadScene("Game");
                    break;
                case 2:
                    SceneManager.LoadScene("Game2");
                    break;
                case 3:
                    //SceneManager.LoadScene("Game");
                    StartCoroutine(WaitForTransport(1, other));
                    break;
                default:
                    SceneManager.LoadScene("Game"); //mudar
                    break;
            }

        }
    }

    IEnumerator WaitForTransport(int secs, Collider2D other)
    {
        yield return new WaitForSeconds(secs);
        other.gameObject.transform.position = spawnPlayerInBoss.transform.position;
        //StartCoroutine(uiManager.FadeOut("", false, 1));
    }

    Vector2 FindSpawningPoint()
    {
        int playerPosX = Mathf.FloorToInt(player.transform.position.x + (mapReference.width / 2));
        int playerPosY = Mathf.FloorToInt(player.transform.position.y + (mapReference.height / 2));

        int returnX = playerPosX - (mapReference.width / 2) + 2;
        int returnY = playerPosY - (mapReference.height / 2) + 2;

        for(int i = playerPosX-2; i < playerPosX + 2; i += 2)
        {
            for (int j = playerPosY - 2; j < playerPosY + 2; j += 2)
            {
                if(mapReference.map[i,j] == 0 && ( i != playerPosX || j != playerPosY ))
                {
                    returnX = i - (mapReference.width / 2);
                    returnY = j - (mapReference.height / 2);
                }
            }
        }
        return new Vector2(returnX, returnY);
    }

    

}
