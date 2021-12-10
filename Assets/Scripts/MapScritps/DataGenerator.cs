using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGenerator : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private MapData mapData = new MapData();
    float startTime;
    private void Awake()
    {
        startTime = Time.time;
    }
    public void SaveIntoJson()
    {
        mapData.timeSpent = Time.time - startTime;

        string map = JsonUtility.ToJson(mapData);
        System.IO.File.WriteAllText(Application.dataPath + "/MapData.json", map);
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
