using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CSVReader : MonoBehaviour

// aqui serve para ler o csv com todas as seeds, e usar o indices coletados para pegar a seed mais próxima
{
   

    public double seed_atual;
    public string CURRENT_SEED;
    public TextAsset textassetdata;
    private PersistentStats stats;
    
    [System.Serializable]
    public class Seeds {
       public string seed;
    }

    [System.Serializable]
    public class SeedList{
        public Seeds[] seedinhas;

    }

    public SeedList mySeeds = new SeedList();

    // Start is called before the first frame update
    void Start()
    {
        stats = FindObjectOfType<PersistentStats>();
        readCSV();
        
    }

    void readCSV(){
        string[]data = textassetdata.text.Split(new String[] {",", "\n"},StringSplitOptions.None);

        int tableSize = data.Length - 1;
        mySeeds.seedinhas = new Seeds[tableSize];

        for(int i=0; i<tableSize; i++){
            mySeeds.seedinhas[i] = new Seeds(); 
            mySeeds.seedinhas[i].seed =(data[i+1]);
        }
        

        seed_atual = CallAI.index_map1;

        CURRENT_SEED = mySeeds.seedinhas[Convert.ToInt32(seed_atual)].seed;
        
        Debug.Log(mySeeds.seedinhas[Convert.ToInt32(seed_atual)].seed);
        stats.SetNewSeed(CURRENT_SEED);
    }
   
}
