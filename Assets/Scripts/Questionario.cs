using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
//Questionário foi removido do ciclo de gameplay do jogo
public class Questionario : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);
    DataGenerator dataGen;
    public GameObject[] questions;
    int activeQuestion;
    public GameObject replayButton;
    private CallAI callAI;

    //string url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScRToMptMqLz3J0mSTqUqnZpBWOIxLEwiGF6B5qOY_s96DsJQ/formResponse";
    //string url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeTWlIZjBk9SPzWS3e6JbhDtIf1UdTbiTv2EUxs2FHQ2DP3Qg/formResponse"; //original
    string url = "https://docs.google.com/forms/d/e/1FAIpQLSf5Jp4HtbpZWZPxRPRxZZqgvVAZjFk5ZYqKJqhBpInbMwNitA/formResponse";


    public string[] answers;
    private void Start()
    {
        replayButton.SetActive(false);
        dataGen = FindObjectOfType<DataGenerator>();
        callAI = FindObjectOfType<CallAI>();
        dataGen.UpdateCounters();
        activeQuestion = 0;
        answers = new string[questions.Length];
    }

    private void BuildRegressionSample()
    {
       callAI.data[0]   = (float)dataGen.playerData.totalLifeLost;
       callAI.data[1]   = (float)dataGen.playerData.timeSpent;
        //callAI.data[2]   = (float)dataGen.playerData.steps; 
       callAI.data[2] = (float) 2.0; // valor para funcionar por enquanto
       callAI.data[3]   = (float)dataGen.playerData.deaths; 
       callAI.data[4]   = (float)dataGen.playerData.percentKills; 
       callAI.data[5]   = (float)dataGen.playerData.percentItemsCollected; 
       callAI.data[6]   = float.Parse(answers[1]); // complexety
       callAI.data[7]   = float.Parse(answers[6]); // difficulty
       callAI.data[8]   = (float)dataGen.genData.averageEnemyDistance; 
       callAI.data[9]   = (float)dataGen.genData.averageItemDistance;
        //callAI.data[10]  = (float)dataGen.playerData.interactions; 
        //callAI.data[11]  = (float)dataGen.playthroughs;
        //callAI.data[12]  = float.Parse(answers[3]); // enemydensety
        //callAI.data[13]  = (float)dataGen.genData.itemDensity;
        //callAI.data[9]  = float.Parse(answers[0]); // mapsize

        Dictionary<string, float> variableDictionary = new Dictionary<string, float>();
        float totalLifeLost = (float)dataGen.playerData.totalLifeLost;
        float timeSpent = (float)dataGen.playerData.timeSpent;
        float steps = (float) 2.0;//(float)dataGen.playerData.steps;
        float deaths = (float)dataGen.playerData.deaths;
        float percentKills = (float)dataGen.playerData.percentKills;
        float percentItemsCollected = (float)dataGen.playerData.percentItemsCollected;
        float complexity = float.Parse(answers[1]);
        float difficulty = float.Parse(answers[6]);
        float averageEnemyDistance = (float)dataGen.genData.averageEnemyDistance;
        float averageItemDistance = (float)dataGen.genData.averageItemDistance;
        // Armazena as variáveis e seus respectivos valores no dicionário
        variableDictionary.Add("totalLifeLost", totalLifeLost);
        variableDictionary.Add("timeSpent", timeSpent);
        variableDictionary.Add("steps", steps);
        variableDictionary.Add("deaths", deaths);
        variableDictionary.Add("percentKills", percentKills);
        variableDictionary.Add("percentItemsCollected", percentItemsCollected);
        variableDictionary.Add("complexity", complexity);
        variableDictionary.Add("difficulty", difficulty);
        variableDictionary.Add("averageEnemyDistance", averageEnemyDistance);
        variableDictionary.Add("averageItemDistance", averageItemDistance);

        // Mostra os valores no console usando um loop
        foreach (var kvp in variableDictionary)
        {
            Debug.Log(kvp.Key + ": " + kvp.Value);
        }
    }

    public void Send()
    {
        StartCoroutine(Post(dataGen));
        //Analytics.CustomEvent("LifeLost", new Dictionary<string, object> { { "TotalLifeLost", dataGen.playerData.totalLifeLost } });
    }
    

    IEnumerator Post(DataGenerator data)
    {
        
        WWWForm form = new WWWForm();

        //player data to forms
        form.AddField("entry.1129405122", data.playerData.interactions);//interactions
        form.AddField("entry.1565504169", data.playerData.percentQuests.ToString().Replace(",", "."));//percentQuests

        //one playthrough info-------------------------------------------------------------------------------------
        form.AddField("entry.243107409", data.totalLifeLost);//lifelost
        form.AddField("entry.1462353102", data.runLevel);//nível nas runs
        form.AddField("entry.1326127552", data.time);//time
        form.AddField("entry.1710124973", data.steps);//steps
        form.AddField("entry.1411949559", data.playerData.deaths.ToString());//deaths
        form.AddField("entry.823628511", data.precision);//precision
        form.AddField("entry.1989127704", data.percentKills);//percentKills
        form.AddField("entry.1295511642", data.percentItemPickup);//percentItems
        form.AddField("entry.153510353", data.percentAmmoPickup);//percentAmmo

        form.AddField("entry.201828663", data.playthroughs);//playthroughs
        form.AddField("entry.744182287", data.runVictory);//victorious
        form.AddField("entry.780342899", data.playerData.foundSecret.ToString());//foundSecret
        form.AddField("entry.952918269", data.playerData.finalPosition.ToString().Replace(",", "."));//finalPosition

        //genData
        form.AddField("entry.1014127927", data.width);//width
        form.AddField("entry.663182150", data.height);//height

        form.AddField("entry.1397100487", data.playerStartPos);//playerStart
        form.AddField("entry.1428732229", data.exitDoorPos);//exitDoor

        form.AddField("entry.1753551012", data.itemPositions);//itemPos
        form.AddField("entry.267469238", data.enemyPositions);//EnemyPos
        form.AddField("entry.1555329560", data.seed);//seed

        form.AddField("entry.177556040", data.smooth);//smooth
        form.AddField("entry.1838180293", data.minRegionSize);//minRegionSize
        form.AddField("entry.527601752", data.randomFillPercent);//randomFillPercent

        form.AddField("entry.66114686", data.minEnemyDistance);//minEnemyDistance
        form.AddField("entry.1814962338", data.minItemDistance);//minItemDistance
        form.AddField("entry.194656318", data.averageEnemyDistance);//averageEnemyDistance
        form.AddField("entry.1578609904", data.averageItemDistance);//averageItemDistance

        form.AddField("entry.462206930", data.enemyDensity);//EnemyDensity
        form.AddField("entry.1182551679", data.itemDensity);//ItemDensity
        form.AddField("entry.1503070918", data.maxEnemies);//MaxEnemies
        form.AddField("entry.212415700", data.maxItems);//MaxItems
        form.AddField("entry.638872648", data.currentEnemies);//GeneratedEnemies
        form.AddField("entry.1135456508", data.currentItems);//GeneratedItems

        //playerInput
        form.AddField("entry.1151053099", answers[0]);//mapSize
        form.AddField("entry.1779334571", answers[1]);//complexity
        form.AddField("entry.968548155", answers[2]);//enemyAmount
        form.AddField("entry.20587696", answers[3]);//enemyDensity

        form.AddField("entry.1862005822", answers[4]);//interactionAmount
        form.AddField("entry.1906391024", answers[5]);//conversationMaterial

        form.AddField("entry.374425141", answers[6]);//difficulty
        form.AddField("entry.1632273714", answers[7]);//fun

        //end one playthrough info--------------------------------------------------------------------------------------------------

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
    }

    public void Answer()
    {
        answers[activeQuestion] = questions[activeQuestion].GetComponentInChildren<Slider>().value.ToString();

        questions[activeQuestion].SetActive(false);
        activeQuestion += 1;
        if (activeQuestion<8)
        {
            questions[activeQuestion].SetActive(true);
        }
        else
        {
            replayButton.SetActive(true);
            BuildRegressionSample();
            callAI.CallAPI();
        }
    }

    public void Replay()
    {
        Send();
        SceneManager.LoadScene("Game2");
    }
    public void QuitGame()
    {
        StartCoroutine(Quit());
    }
    public IEnumerator Quit()
    {
        Send();
        yield return new WaitForSeconds(2);
        Application.Quit();
    }

}
