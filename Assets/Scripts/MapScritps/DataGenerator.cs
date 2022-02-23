using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataGenerator : MonoBehaviour
{

    [SerializeField]
    private MapData mapData;
    float startTime;
    MapGenAutomata mapReference;
    PlayerScript playerScript;
    public int[,] deathMatrix;
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
        Generation genData = new Generation(mapReference.map);

        //reads death map matrix
        string input = "Assets/mapData_" + mapReference.seed + ".json";
        if (File.Exists(input))//se já existe arquivo de dados
        {
            mapData = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(input));//usa o existente
            File.Delete(input);//deleta para salvar corretamente
        }
        else//senão, quer dizer que nunca morreu nesse mapa, então usa o mapa gerado
        {
            deathMatrix = mapReference.map;
        }
        if (playerScript.currentHealth <= 0)//se função foi chamada quando player morreu, decrementa posição
        {
            deathMatrix[Mathf.FloorToInt(transform.position.x + mapReference.height / 2),
            Mathf.FloorToInt(transform.position.y + mapReference.width / 2)]--;//decrementa na posição da morte
        }

        Combat combatData = new Combat(mapReference.enemyPositions, deathMatrix);
        Exploration expData = new Exploration((Time.time - startTime), playerScript.pathing);

        mapData = new MapData(genData, combatData, expData);
        

        string json = JsonConvert.SerializeObject(mapData, Formatting.None);
        using (var sw = new StreamWriter(input))
        {
            sw.Write(json);
            sw.Flush();
            sw.Close();
        }
        //
    }

}

[System.Serializable]
public class MapData
{
    public Generation generation;
    public Combat combat;
    public Exploration exploration;

    public MapData(Generation genData, Combat combatData, Exploration expData)
    {
        generation = genData;
        combat = combatData;
        exploration = expData;
    }
    
}

public class Generation
{
    public int[,] map;//mapa gerado
    public Generation(int[,] matrix)
    {
        map = matrix;
    }
}
public class Combat
{    
    public Vector2[] enemyPos;//posição dos inimigos
    public int[,] deathPos;//posição das mortes
    public Combat(Vector2[] posList, int[,] deathMatrix)
    {
        enemyPos = posList;
        deathPos = deathMatrix;
    }
}
public class Exploration
{
    public float timeSpent;//tempo gasto
    public int[,] playerPath;//caminho percorrido
    public Exploration(float time, int[,] pathing)
    {
        timeSpent = time;
        playerPath = pathing;
    }
}

/*
     * Dados de Geração: (feitos em outros scripts, tentando mensurar qualidade da geração, possibilitando comparações futuras)
     * -matriz de geração (mapa salvo em formato binário)(feito)
     * -posição de objetos
     * 
     * Dados de Exploração: (tentando mensurar "aproveitamento" do jogador)
     * -caminho percorrido (comparar com caminho mínima A*?) (feito)
     * -itens coletados (ou porcentagem de itens coletados?)
     * -tempo gasto na fase (feito)
     * 
     * Dados de Combate: (tentando mensurar habilidade do jogador)
     * -posição das mortes do jogador (feito)
     * -posicionamento de inimigos (feito)
     * -vida perdida
     * -inimigos derrotados (ou porcentagem de inimigos derrotados?)
     * -precisão? (métrica de ataques que resultam em acerto)
     */
