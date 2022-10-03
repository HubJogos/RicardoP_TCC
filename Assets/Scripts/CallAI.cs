using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CallAI : MonoBehaviour{

    private string jsonDownloaded;
    private DataGenerator data;
    //private Questionario questionario;
    private List<object> data_values_regression = new List<object>();
    public Predictions predicted = new Predictions();

    void Start(){
        Init();
        StartCoroutine(JsonReader());
    }

    private void Init(){
        data = FindObjectOfType<DataGenerator>();
        //questionario = FindObjectOfType<Questionario>();
        //BuildSampleRegression();
    }

    private void BuildSampleRegression(){

        // Inputs
        /*data_values_regression.Add(data.playerData.totalLifeLost);
        data_values_regression.Add(Mathf.FloorToInt(data.playerData.timeSpent));
        data_values_regression.Add(data.playerData.steps);
        data_values_regression.Add(data.playerData.deaths);
        data_values_regression.Add(data.playerData.percentKills);
        data_values_regression.Add(data.playerData.percentItemsCollected);
        data_values_regression.Add(questionario.answers[1]);   //complexity
        data_values_regression.Add(questionario.answers[6]);   //difficulty
        data_values_regression.Add(data.genData.averageEnemyDistance);
        data_values_regression.Add(data.genData.averageItemDistance);
        data_values_regression.Add(data.playerData.interactions);
        data_values_regression.Add(data.playerData.playthroughs);

        data_values_regression[0] = data.playerData.totalLifeLost;
        data_values_regression[1] = Mathf.FloorToInt(data.playerData.timeSpent);
        data_values_regression[2] = data.playerData.steps;
        data_values_regression[3] = data.playerData.deaths;
        data_values_regression[4] = data.playerData.percentKills;
        data_values_regression[5] = data.playerData.percentItemsCollected;
        data_values_regression[6] = float.Parse(questionario.answers[1]);   //complexity
        data_values_regression[7] = float.Parse(questionario.answers[6]);   //difficulty
        data_values_regression[8] = data.genData.averageEnemyDistance;
        data_values_regression[9] = data.genData.averageItemDistance;
        data_values_regression[10] = data.playerData.interactions;
        data_values_regression[11] = data.playthroughs;*/

    }

    public IEnumerator JsonReader(){

        float[] data = {100f,62f,477f,0f,0.210526f,0.700000f,0f,1f,33.681550f,32.242820f,4f,4f};
        string data_api = BuildString(data);

        //string url = "https://python-integration.herokuapp.com/regression";
        string url = "http://127.0.0.1:5000/regression";
        WWWForm form = new WWWForm();
        /*form.AddField("key", data_values_regression[0].ToString());
        form.AddField("key", data_values_regression[1].ToString());
        form.AddField("key", data_values_regression[2].ToString());
        form.AddField("key", data_values_regression[3].ToString());
        form.AddField("key", data_values_regression[4].ToString());
        form.AddField("key", data_values_regression[5].ToString());
        form.AddField("key", data_values_regression[6].ToString());
        form.AddField("key", data_values_regression[7].ToString());
        form.AddField("key", data_values_regression[8].ToString());
        form.AddField("key", data_values_regression[9].ToString());
        form.AddField("key", data_values_regression[10].ToString());
        form.AddField("key", data_values_regression[11].ToString());*/
        //form.AddField("key", data_values_regression.ToString());

        form.AddField("key", data_api.ToString());
        
        using(UnityWebRequest request = UnityWebRequest.Post(url, form)){
            yield return request.SendWebRequest();
            if(request.isNetworkError) Debug.Log(request.error);
            else{
                jsonDownloaded = request.downloadHandler.text;
                Debug.Log(jsonDownloaded);
                predicted = JsonConvert.DeserializeObject<Predictions>(jsonDownloaded);
                Debug.Log(predicted.predictions[0][1]);
            }
        }
    }

    private string BuildString(float[] data){
        string x = string.Join(" ", data);
        string y = x.Replace(',', '.');
        string z = y.Replace(' ', ',');
        return z;
    }

}


[System.Serializable]
public class Predictions{
    public float[][] predictions;
}
