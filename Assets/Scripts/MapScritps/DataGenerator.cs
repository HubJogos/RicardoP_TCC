using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataGenerator : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private MapData mapData = new MapData();
    float startTime;
    MapGenAutomata mapReference;
    private void Awake()
    {
        mapReference = FindObjectOfType<MapGenAutomata>();
        startTime = Time.time;
    }
    public void SaveIntoJson()
    {
        mapData.timeSpent = Time.time - startTime;

        
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

    /*
     * Dados de Exploração: (tentando mensurar qualidade da geração)
     * -distância caminhada (comparar com distância mínima A*)
     * -itens coletados (ou porcentagem de itens coletados?)
     * 
     * Dados de Combate: (tentando mensurar habilidade do jogador)
     * -vida perdida
     * -inimigos derrotados (ou porcentagem de inimigos derrotados?)
     * -precisão? (métrica de ataques que resultam em acerto)
     */
}
