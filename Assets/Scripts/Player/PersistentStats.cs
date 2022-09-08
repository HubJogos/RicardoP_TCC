using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerLevel = 1;
    public int currentExp = 0;

    [Header("Health Variables")]
    public int maxHealth = 50;
    public bool closedTutorial;

    [Header("Map configuration")]
    public int width = 100;
    public int height = 60;//largura e altura do mapa
    public int smooth = 2;//grau de suavização dos quadrados gerados
    public int minRegionSize = 50;//tamanho mínimo das regiões geradas (exclui o que estiver abaixo)

    public bool useRandomSeed = true;
    public string seed = "0";

    [Range(0, 100)]
    public int randomFillPercent = 55;//porcentagem de terreno/parede

    [Range(0, 100)]
    public int enemyDensity = 50;
    [Range(0, 100)]
    public int itemDensity = 20;

    public int minEnemyDistance = 15;
    public int minItemDistance = 25;

    //max enemies
    //max items

}
