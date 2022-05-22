using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Questionario : MonoBehaviour
{
    DataGenerator dataGen;
    public GameObject[] questions;
    int activeQuestion;
    
    string url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeTWlIZjBk9SPzWS3e6JbhDtIf1UdTbiTv2EUxs2FHQ2DP3Qg/formResponse";
    string[] answers;
    private void Start()
    {
        dataGen = FindObjectOfType<DataGenerator>();
        activeQuestion = 0;
        answers = new string[questions.Length];
    }
    public void Send()
    {
        StartCoroutine(Post(dataGen));
    }
    public void QuitGame()
    {
        Send();
        Application.Quit();
    }

    IEnumerator Post(DataGenerator data)
    {
        
        WWWForm form = new WWWForm();
        //player data to forms
        form.AddField("entry.2005760843", data.playerData.totalLifeLost);//lifelost
        form.AddField("entry.1108869829", data.playerData.playerLevel);//exp
        form.AddField("entry.1204426239", Mathf.FloorToInt(data.playerData.timeSpent).ToString());//time
        form.AddField("entry.175035077", data.playerData.steps);//steps
        form.AddField("entry.1023732896", data.playerData.deaths);//deaths
        form.AddField("entry.213494061", data.playerData.continues);//continue
        form.AddField("entry.793916201", data.playerData.precision.ToString());//precision
        form.AddField("entry.788247621", data.playerData.percentKills.ToString());//percentKills
        form.AddField("entry.1765112349", data.playerData.percentItemsCollected.ToString());//percentItems
        form.AddField("entry.1340334397", data.playerData.percentAmmo.ToString());//percentAmmo
        form.AddField("entry.963488702", data.playerData.interactions);//interactions
        form.AddField("entry.596939258", data.playerData.percentQuests.ToString());//percentQuests
        form.AddField("entry.53939988", data.playerData.victorious.ToString());//victorious
        form.AddField("entry.646014955", data.playerData.foundSecret.ToString());//foundSecret
        form.AddField("entry.105534763", data.playerData.finalPosition.ToString());//finalPosition
        
        //genData
        form.AddField("entry.2048159362", data.genData.width);//width
        form.AddField("entry.1371681809", data.genData.height);//height
        form.AddField("entry.1451868055", data.playerStartPos);//playerStart
        form.AddField("entry.1889395333", data.exitDoorPos);//exitDoor
        form.AddField("entry.1933815164", data.itemPositions);//itemPos
        form.AddField("entry.592241768", data.enemyPositions);//EnemyPos
        form.AddField("entry.364457296", data.genData.seed);//seed

        //playerInput
        form.AddField("entry.777965021", answers[0]);//mapSize
        form.AddField("entry.1009410047", answers[1]);//complexity
        form.AddField("entry.299056882", answers[2]);//enemyAmount
        form.AddField("entry.203252442", answers[3]);//enemyDensity
        form.AddField("entry.258848594", answers[4]);//interactionAmount
        form.AddField("entry.675072857", answers[5]);//conversationMaterial
        form.AddField("entry.1282593994", answers[6]);//difficulty
        form.AddField("entry.1313662470", answers[7]);//fun

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
    }

    public void Answer()
    {
        answers[activeQuestion] = questions[activeQuestion].GetComponentInChildren<Slider>().value.ToString();
        Debug.Log(answers[activeQuestion]);

        questions[activeQuestion].SetActive(false);
        activeQuestion += 1;
        if (activeQuestion<8)
        {
            questions[activeQuestion].SetActive(true);
        }
        else
        {
            QuitGame();
        }
    }

}
