using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class DataGenerator : MonoBehaviour
{

    [SerializeField]
    float startTime;
    Scene activeScene;
    bool done = false;
    public bool doneGen = false;

    bool victory = false;
    public bool foundSecret = false;
    public int deathCounter = 0;

    public float completedQuests = 0;
    float totalQuests = 2;
    float percentQuests;

    public int interactions = 0;

    MapGenAutomata mapReference;
    PlayerScript playerScript;
    public PlayerData playerData;
    public GenData genData;
    public QuestionData questionData;
    

    Questionario questionario;
    bool doneQuestion = false;

    [Header("Final Data")]
    //player data for multiple plays
    public string totalLifeLost;
    public string time;
    public string steps;
    public string runLevel;

    public string precision;
    public string percentKills;
    public string percentItemPickup;
    public string percentAmmoPickup;

    [Header("Generation")]
    //GenData for multiple plays
    public string width;
    public string height;
    public string smooth;
    public string minRegionSize;
    public string randomFillPercent;
    public string minEnemyDistance;
    public string minItemDistance;
    public string averageEnemyDistance;
    public string averageItemDistance;
    public string enemyDensity;
    public string itemDensity;
    public string maxEnemies;
    public string maxItems;
    public string currentEnemies;
    public string currentItems;
    public string seed;
    [Header("Playthrough")]
    public Vector2 finalPosition;
    public string runVictory;
    public string runDeath;
    public string playerStartPos;
    public string exitDoorPos;
    public string itemPositions;
    public string enemyPositions;
    public int playthroughs = 0;
    [Header("Questions")]
    //Partial questions answers for multiple plays
    public string[] answers = new string[6];



    private void FixedUpdate()
    {
        if (FindObjectOfType<Questionario>() && !doneQuestion)
        {
            questionario = FindObjectOfType<Questionario>();
            doneQuestion = true;
        }
        percentQuests = completedQuests / totalQuests;
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Preload")
        {
            SceneManager.LoadScene("Game");
        }
        if (activeScene.name=="Game" && !done)
        {
            if (FindObjectOfType<PlayerScript>() != null)
            {
                playerScript = FindObjectOfType<PlayerScript>();
            }
            startTime = Time.time;
            done = true;
        }

        if (activeScene.name == "MapGeneration")
        {
            if (!doneGen)
            {
                if (FindObjectOfType<PlayerScript>() != null)
                {
                    playerScript = FindObjectOfType<PlayerScript>();
                }
                if (FindObjectOfType<MapGenAutomata>() != null)
                {
                    mapReference = FindObjectOfType<MapGenAutomata>();
                    //where all map generation data is stored
                    genData = new GenData(mapReference.width, 
                        mapReference.height,

                        mapReference.smooth,
                        mapReference.minRegionSize,
                        mapReference.randomFillPercent,

                        
                        mapReference.minEnemyDistance,
                        mapReference.minItemDistance,
                        mapReference.averageEnemyDistance,
                        mapReference.averageItemDistance,
                        

                        mapReference.enemyDensity,
                        mapReference.itemDensity,
                        mapReference.maxEnemies,
                        mapReference.maxItems,
                        mapReference.currentEnemies,
                        mapReference.currentItems,
                        
                        mapReference.endStage.transform.position,
                        mapReference.itemPositions,
                        mapReference.enemyPositions,
                        mapReference.seed);

                }
            }//executa 1 vez só pra referenciar objetos relevantes
            doneGen = true;
        }
            
    }

    public void SaveAsCSV()
    {
        if(questionario != null)
        {
            questionData = new QuestionData(questionario.answers);
        }

        if (completedQuests < 2)//se cumpriu todas as quests
        {
            victory = false;
        }
        else
        {
            victory = true;
        }


        finalPosition = playerScript.transform.position;

        //data per playthrough is separated
        totalLifeLost += playerScript.totalLifeLost.ToString() + " / ";
        time += Mathf.FloorToInt(Time.time - startTime).ToString() + " / ";
        steps += playerScript.steps.ToString() + " / ";
        runLevel += playerScript.playerLevel.ToString() + " / ";

        //averages

        if (playerScript.attacksAttempted == 0)
        {
            precision += Double.NaN.ToString().Replace(",", ".") + " / ";

        }else precision += playerScript.precision.ToString().Replace(",",".") + " / ";

        percentKills += playerScript.percentKills.ToString().Replace(",", ".") + " / ";
        percentItemPickup += playerScript.percentItemsCollected.ToString().Replace(",", ".") + " / ";

        percentAmmoPickup += playerScript.ammoPickupRate.ToString().Replace(",", ".") + " / ";
        runVictory += victory.ToString() + " / ";

        //where all player data is stored

        playerData = new PlayerData(playerScript.totalLifeLost, 
            playerScript.playerLevel, 
            (Time.time - startTime),
            playerScript.steps, 
            deathCounter,

            playerScript.precision,
            playerScript.percentKills,
            playerScript.percentItemsCollected,
            playerScript.ammoPickupRate,

            interactions,
            percentQuests,
            victory,
            foundSecret,
            finalPosition);

        playerStartPos += mapReference.playerStartPos.ToString().Replace(",", ".") + " / ";
        exitDoorPos += genData.exitDoor.ToString().Replace(",", ".") + " / ";


        for (int i = 0; i < mapReference.currentItems; i++)
        {
            itemPositions += genData.itemPositions[i].ToString().Replace(",",".");
        }
        for (int i = 0; i < mapReference.currentEnemies; i++)
        {
            enemyPositions += genData.enemyPositions[i].ToString().Replace(",", ".");
        }
        itemPositions += " / ";
        enemyPositions += " / ";//separando dados das playthroughs


        done = false;
        doneGen = false;

        //concatenating multiple playthroughs data
        width += genData.width.ToString() + " / ";
        height += genData.height.ToString() + " / ";
        smooth += genData.smooth.ToString() + " / ";
        minRegionSize += genData.minRegionSize.ToString() + " / ";
        randomFillPercent += genData.randomFillPercent.ToString() + " / ";
        minEnemyDistance += genData.minEnemyDistance.ToString() + " / ";
        minItemDistance += genData.minItemDistance.ToString() + " / ";
        averageEnemyDistance += genData.averageEnemyDistance.ToString().Replace(",", ".") + " / ";
        averageItemDistance += genData.averageItemDistance.ToString().Replace(",", ".") + " / ";
        enemyDensity += genData.enemyDensity.ToString() + " / ";
        itemDensity += genData.itemDensity.ToString() + " / ";
        maxEnemies += genData.maxEnemies.ToString() + " / ";
        maxItems += genData.maxItems.ToString() + " / ";
        currentEnemies += genData.currentEnemies.ToString() + " / ";
        currentItems += genData.currentItems.ToString() + " / ";
        seed += genData.seed.ToString().Replace(",", ".") + " / ";

    }


    public void UpdateCounters()
    {
        if (deathCounter > 0)
        {
            runDeath += true.ToString() + " / ";
        }else runDeath += false.ToString() + " / ";

        deathCounter = 0;
    }
}


[System.Serializable]
public class PlayerData
{
    public int totalLifeLost;
    public int playerLevel;
    public float timeSpent;
    public int steps;
    public int deaths;
    public float precision;
    public float percentKills;
    public float percentItemsCollected;
    public float percentAmmo;
    public int interactions;
    public float percentQuests;
    public bool victorious;
    public bool foundSecret;
    public Vector2 finalPosition;
    public PlayerData(int _totalLifeLost, int _playerLevel, float _timeSpent, int _steps, int _deaths, float _precision, float _percentKills, float _percentItemsCollected, float _percentAmmo, int _interactions, float _percentQuests, bool _victorious, bool _foundSecret, Vector2 _finalPosition)
    {

        totalLifeLost = _totalLifeLost;
        playerLevel = _playerLevel;
        timeSpent = _timeSpent;
        steps = _steps;
        deaths = _deaths;
        precision = _precision;
        percentKills = _percentKills;
        percentItemsCollected = _percentItemsCollected;
        percentAmmo = _percentAmmo;
        interactions = _interactions;
        percentQuests = _percentQuests;
        victorious = _victorious;
        foundSecret = _foundSecret;
        finalPosition = _finalPosition;
    }
    
}

[System.Serializable]
public class GenData
{
    public int width;
    public int height;

    public int smooth;
    public int minRegionSize;
    public int randomFillPercent;
    
    public int minEnemyDistance;
    public int minItemDistance;
    public float averageEnemyDistance;
    public float averageItemDistance;

    public int enemyDensity;
    public int itemDensity;
    public int maxEnemies;
    public int maxItems;
    public int currentEnemies;
    public int currentItems;

    public string seed;

    public Vector2 exitDoor;
    public Vector2[] itemPositions;
    public Vector2[] enemyPositions;
    public GenData(int _width, int _height, int _smooth, int _minRegionSize, int _randomFillPercent, int _minEnemyDistance, 
        int _minItemDistance, float _averageEnemyDistance, float _averageItemDistance, int _enemyDensity,
        int _itemDensity, int _maxEnemies, int _maxItems, int _currentEnemies, int _currentItems, 
        Vector2 _exitDoor, Vector2[] _itemPositions, Vector2[] _enemyPositions, string _seed)
    {
        width = _width;
        height = _height;

        smooth = _smooth;
        minRegionSize = _minRegionSize;
        randomFillPercent = _randomFillPercent;
        
        minEnemyDistance = _minEnemyDistance;
        minItemDistance = _minItemDistance;
        averageEnemyDistance = _averageEnemyDistance;
        averageItemDistance = _averageItemDistance;
        
        enemyDensity = _enemyDensity;
        itemDensity = _itemDensity;
        maxEnemies = _maxEnemies;
        maxItems = _maxItems;
        currentEnemies = _currentEnemies;
        currentItems = _currentItems;

        exitDoor = _exitDoor;
        itemPositions = _itemPositions;
        enemyPositions = _enemyPositions;
        seed = _seed;
    }
}
[System.Serializable]
public class QuestionData
{
    string[] answers;
    public QuestionData(string[] _answers)
    {
        answers = new string[_answers.Length];
        for(int i = 0; i < _answers.Length; i++)
        {
            answers[i] = _answers[i];
        }
    }
}