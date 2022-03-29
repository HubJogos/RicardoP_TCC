using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataGenerator : MonoBehaviour
{

    [SerializeField]
    float startTime;
    Vector2 deathPos;
    MapGenAutomata mapReference;
    PlayerScript playerScript;
    public PlayerData playerData;
    private void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        mapReference = FindObjectOfType<MapGenAutomata>();
        startTime = Time.time;

        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            SaveIntoJson();
        }
    }
    public void SaveIntoJson()
    {
        
        
        //reads death map matrix
        string dataPath = "Assets/PlayerData_" + mapReference.seed + ".json";
        if (File.Exists(dataPath))//se já existe arquivo de dados
        {
            File.Delete(dataPath);//deleta para salvar corretamente
        }
        if (playerScript.currentHealth <= 0)//se função foi chamada quando player morreu
        {
            deathPos = new Vector2(Mathf.FloorToInt(playerScript.transform.position.x + mapReference.height / 2), Mathf.FloorToInt(playerScript.transform.position.y + mapReference.width / 2));//decrementa na posição da morte
        }//receives any pre-existing combat data to increment, if none exist, initializes a new one

        playerData = new PlayerData((Time.time - startTime), playerScript.percentItemsCollected, playerScript.totalLifeLost, deathPos, playerScript.precision, playerScript.percentEnemiesDefeated, mapReference.enemyPositions, playerScript.pathing);
        
        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        using (var sw = new StreamWriter(dataPath))
        {
            sw.Write(json);
            sw.Flush();
            sw.Close();
        }
    }


}


[System.Serializable]
public class PlayerData
{
    public float timeSpent;
    //treasures found
    public float percentItemsCollected;
    public int totalLifeLost;
    //experiência ganha
    //número de mortes
    //número de continues
    public Vector2 deathPos;
    //ataques realizados
    //munição recuperada
    public float precision;
    public float percentEnemiesDefeated;
    public Vector2[] enemyPos;
    //terminou fase?
    public int[,] playerPath;
    //somatório de passos, medindo o quanto se moveu
    
    public PlayerData(float time, float percent, int lifeLost, Vector2 deathPosition, float attackPercent, float enemyPercent, Vector2[] posList, int[,] pathing)
    {
        timeSpent = time;
        percentItemsCollected = percent;
        totalLifeLost = lifeLost;
        deathPos = deathPosition;
        precision = attackPercent;
        percentEnemiesDefeated = enemyPercent;
        enemyPos = posList;
        playerPath = pathing;
    }
}

/*
     * Dados de Geração: (feitos em outros scripts, tentando mensurar qualidade da geração, possibilitando comparações futuras)
     * -matriz de geração (mapa salvo em formato binário)(feito)
     * -posição de objetos
     * 
     * Dados do Jogador
         * Dados de Exploração: (tentando mensurar "aproveitamento" do jogador)
         * -caminho percorrido (comparar com caminho mínima A*?) (feito)
         * -porcentagem de itens coletados (feito)                  (playerScript possui variáveis para o total de itens gerados e coletados)
         *
         * -tempo gasto na fase (feito)
         * 
         * Dados de Combate: (tentando mensurar habilidade do jogador)
         * -posição das mortes do jogador (feito)
         * -posicionamento de inimigos (feito)
         * -vida perdida (feito)
         * -porcentagem de inimigos derrotados (feito)              (playerScript possui variáveis para o total de inimigos gerados e derrotados)
         *
         * -precisão? (métrica de ataques que resultam em acerto)
     */
