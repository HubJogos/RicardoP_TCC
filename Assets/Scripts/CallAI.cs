using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CallAI : MonoBehaviour{

    private string jsonDownloaded;
    private DataGenerator data;
    //private Questionario questionario;
    private List<object> data_values_regression = new List<object>();
    public List<object> predicted_values = new List<object>();

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
        data_values_regression.Add(data.playerData.totalLifeLost);
        data_values_regression.Add(Mathf.FloorToInt(data.playerData.timeSpent));
        data_values_regression.Add(data.playerData.steps);
        data_values_regression.Add(data.playerData.deaths);
        data_values_regression.Add(data.playerData.percentKills);
        data_values_regression.Add(data.playerData.percentItemsCollected);
        //data_values_regression.Add(questionario.answers[1]);   //complexity
        //data_values_regression.Add(questionario.answers[6]);   //difficulty
        //data_values_regression.Add(questionario.answers[7]);   //fun
        data_values_regression.Add(2);   //complexity
        data_values_regression.Add(2);   //difficulty
        data_values_regression.Add(1);   //fun
        data_values_regression.Add(data.genData.averageEnemyDistance);
        data_values_regression.Add(data.genData.averageItemDistance);
        data_values_regression.Add(data.playerData.interactions);

    }

    public IEnumerator JsonReader(){

        float[] data = {90f,50f,261f,0f,0.105263f,0.555556f,2f,2f,1f,25.253560f,16.643700f,7f};
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
