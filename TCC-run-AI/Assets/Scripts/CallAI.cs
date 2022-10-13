using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CallAI : MonoBehaviour{

    public string jsonDownloaded;
    private List<object> data_values_regression = new List<object>();
    //private List<object> knn_seeds = new List<object>();
    //public float[] data = new float[12];
    public float[] dataKnn = new float[10]; 
    public Predictions predicted = new Predictions();
    //private PersistentStats stats;
    private CSVReader index_seed;

    private void Start() {
        //stats = FindObjectOfType<PersistentStats>();
        //index_seed = FindObjectOfType<CSVReader>();
    }

    public void CallAPI(){
        StartCoroutine(JsonReader());
    }

      [System.Serializable]
     public class Root
    {
        public List<List<List<double>>> Mapas { get; set; }
    }

    public static double index_map1;
    public static double index_map2;
    public static double index_map3;
  

    public IEnumerator JsonReader(){

        //float[] data1 = {100f,62f,477f,0f,0.210526f,0.700000f,0f,1f,33.681550f,32.242820f,4f,4f};

        //string data_api = BuildString(data);
        string data_api = BuildString(dataKnn);
        //string url = "https://python-integration.herokuapp.com/regression";
        //string url = "http://127.0.0.1:5000/regression";
        string url = "http://127.0.0.1:5000/kneighbors";
        WWWForm form = new WWWForm();

        form.AddField("key", data_api.ToString());
        
        using(UnityWebRequest request = UnityWebRequest.Post(url, form)){
            yield return request.SendWebRequest();
            if(request.isNetworkError) Debug.Log(request.error);
            else{
                jsonDownloaded = request.downloadHandler.text;
                Debug.Log(jsonDownloaded);
                Debug.Log("vamo merda");
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonDownloaded);  
           
                for(var itemIndex = 0; itemIndex < myDeserializedClass.Mapas.Count; itemIndex++){
                    var item = myDeserializedClass.Mapas[itemIndex];
                    for(var xIndex = 0; xIndex < item.Count; xIndex++) {
                        var x = item[xIndex];
                        for(var yIndex = 0; yIndex < x.Count; yIndex++) {
                            var y = x[yIndex];
                            index_map1 = x[0];
                            index_map2 = x[1];
                            index_map3 = x[2];
                        }
                    }
                }    
                Debug.Log("Seed correta abaixo:");
                Debug.Log(index_map1);
                //Debug.Log(index_map2);
                //Debug.Log(index_map3);
                //Debug.Log(index_seed.CURRENT_SEED);
                //stats.SetNewSeed(index_seed);
                
            }
        }
    }


    private string BuildString(float[] dataKnn){
        string x = string.Join(" ", dataKnn);
        string y = x.Replace(',', '.');
        string z = y.Replace(' ', ',');
        return z;
    }

}

[System.Serializable]
public class Predictions{
    public float[][] predictions;
}

