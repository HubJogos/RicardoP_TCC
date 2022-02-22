using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataGenerator : MonoBehaviour
{

    [SerializeField]
    private MapData mapData = new MapData();
    float startTime;
    MapGenAutomata mapReference;
    PlayerScript playerScript;
    public int[,] deathMatrix;
    private void Awake()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        mapReference = FindObjectOfType<MapGenAutomata>();
        startTime = Time.time;
    }
    public void SaveIntoJson()
    {
        mapData.timeSpent = Time.time - startTime;
        mapData.binMap = mapReference.map;//facilitar leitura



        mapData.enemyPos = mapReference.enemyPositions;



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

        deathMatrix[Mathf.FloorToInt(transform.position.x + mapReference.width / 2),
            Mathf.FloorToInt(transform.position.y + mapReference.height / 2)]--;//decrementa na posição da morte



        string json = JsonConvert.SerializeObject(mapData);
        string input = "Assets/mapData_" + mapReference.seed + ".json";
        using (var sw = new StreamWriter(input))
        {
            sw.Write(json);
            sw.Flush();
            sw.Close();
        }
        //
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SaveIntoJson();
        }
    }
}

[System.Serializable]
public class MapData
{
    public float timeSpent;
    public Vector2[] enemyPos;
    public int[,] binMap;
    

    /*
     * Dados de Mapa: (feitos em outros scripts, tentando mensurar qualidade da geração, possibilitando comparações futuras)
     * -matriz de geração (mapa salvo em formato binário)(feito)
     * -posição das mortes do jogador (enquanto não houver um "continue", deve permanecer no script PlayerHealthManager
     * -posicionamento de inimigos e objetos (feito)
     * 
     * Dados de Exploração: (tentando mensurar "aproveitamento" do jogador)
     * -distância caminhada (comparar com distância mínima A*)
     * -itens coletados (ou porcentagem de itens coletados?)
     * -tempo gasto na fase (feito)
     * 
     * Dados de Combate: (tentando mensurar habilidade do jogador)
     * -vida perdida
     * -inimigos derrotados (ou porcentagem de inimigos derrotados?)
     * -precisão? (métrica de ataques que resultam em acerto)
     */
}
