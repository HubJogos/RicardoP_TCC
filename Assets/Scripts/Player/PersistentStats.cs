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

    public int maxEnemies=19;
    public int maxItems=12;
    
    public void SetNewGeneratedMap(Predictions predictions){
        width               = (int)predictions.predictions[0][0];
        height              = (int)predictions.predictions[0][1];
        smooth              = (int)predictions.predictions[0][2];
        minRegionSize       = (int)predictions.predictions[0][3];
        minEnemyDistance    = (int)predictions.predictions[0][4];
        minItemDistance     = (int)predictions.predictions[0][5];
        maxEnemies          = (int)predictions.predictions[0][6];
        maxItems            = (int)predictions.predictions[0][7];
        randomFillPercent   = (int)predictions.predictions[0][8];
        enemyDensity        = (int)predictions.predictions[0][9];
        itemDensity         = (int)predictions.predictions[0][10];
    }

}
