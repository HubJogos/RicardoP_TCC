using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataGenerator : MonoBehaviour
{

    [SerializeField]
    float startTime;
    Scene activeScene;
    bool done = false;
    bool doneGen = false;
    bool victory = false;
    public int deathCounter = 0;
    public int continues = 0;

    MapGenAutomata mapReference;
    PlayerScript playerScript;
    public PlayerData playerData;
    private void FixedUpdate()
    {
        
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
                }
            }//executa 1 vez só pra referenciar objetos relevantes
            if (playerScript.currentHealth <= 0)
            {
                playerScript.gameObject.SetActive(false);//desativa player
                SaveAsCSV();//gera dados
            }
            doneGen = true;
        }
            
    }
    IEnumerator WaitToRestart()
    {
        yield return new WaitForSeconds(2);
    }//auxiliar para reiniciar o jogo

    public void SaveAsCSV()
    {
        
        
        //reads death map matrix
        string dataPath = Application.dataPath + "/Data/Player/PlayerData_" + mapReference.seed + ".csv";
        if (File.Exists(dataPath))//se já existe arquivo de dados
        {
            File.Delete(dataPath);//deleta para salvar corretamente
        }
        if (playerScript.currentHealth <= 0)//se função foi chamada quando player morreu
        {
            //deathPos = new Vector2(Mathf.FloorToInt(playerScript.transform.position.x + mapReference.height / 2), Mathf.FloorToInt(playerScript.transform.position.y + mapReference.width / 2));//decrementa na posição da morte
        }
        else
        {
            victory = true;
        }//receives any pre-existing combat data to increment, if none exist, initializes a new one

        //deaths, continues
        playerData = new PlayerData(playerScript.totalLifeLost, 
            playerScript.currentExp, 
            (Time.time - startTime), 
            playerScript.steps, 
            playerScript.precision, 
            playerScript.percentKills, 
            playerScript.percentItemsCollected, 
            playerScript.ammoPickupRate, 
            victory);


        TextWriter tw =  new StreamWriter(dataPath, true);
        tw.WriteLine(playerData.totalLifeLost + "," + 
            playerData.expGain + "," + 
            playerData.timeSpent.ToString().Replace(",",".") + "," + 
            playerData.steps + "," + 
            playerData.precision.ToString().Replace(",", ".") + "," + 
            playerData.percentKills.ToString().Replace(",", ".") + "," + 
            playerData.percentItemsCollected.ToString().Replace(",", ".") + "," +
            playerData.percentAmmo.ToString().Replace(",", ".") + "," + 
            victory + "," +
            mapReference.seed.ToString().Replace(",","."));
        tw.Close();

        WaitToRestart();
        SceneManager.LoadScene("Game");//reloads town scene
        done = false;
        doneGen = false;
    }



}


[System.Serializable]
public class PlayerData
{
    public int totalLifeLost;
    public int expGain;
    public float timeSpent;
    public int steps;
    //número de mortes
    //número de continues
    public float precision;
    public float percentKills;
    public float percentItemsCollected;
    public float percentAmmo;
    public bool victorious;



    /*
    public Vector2 deathPos;//remove these 3 or save differently
    public Vector2[] enemyPos;
    public int[,] playerPath;
    */
    
    public PlayerData(int _totalLifeLost, int _expGain, float _timeSpent, int _steps, float _precision, float _percentKills, float _percentItemsCollected, float _percentAmmo, bool _victorious)
    {//deaths, continues

        totalLifeLost = _totalLifeLost;
        expGain = _expGain;
        timeSpent = _timeSpent;
        steps = _steps;
        //deaths
        //continues
        precision = _precision;
        percentKills = _percentKills;
        percentItemsCollected = _percentItemsCollected;
        percentAmmo = _percentAmmo;
        victorious = _victorious;



        /*
        deathPos = deathPosition;
        enemyPos = posList;
        playerPath = pathing;
        */
    }
}

/*
     * Dados de Geração: (feitos em outros scripts, tentando mensurar qualidade da geração, possibilitando comparações futuras)
     * -matriz de geração (mapa salvo em formato binário)(feito)
     * -posição de objetos
     * 
     * Dados do Jogador
         * Dados de Exploração: (tentando mensurar "aproveitamento" do jogador)
         * -caminho percorrido (feito)
         * -porcentagem de itens coletados (feito)                  (playerScript possui variáveis para o total de itens gerados e coletados)
         *
         * -tempo gasto na fase (feito)
         * 
         * Dados de Combate: (tentando mensurar habilidade do jogador)
         * -posição das mortes do jogador (feito)
         * -posicionamento de inimigos (feito)
         * -vida perdida (feito)
         * -porcentagem de inimigos derrotados (feito)              (playerScript possui variáveis para o total de inimigos gerados e derrotados)
         *
         * -precisão? (métrica de ataques que resultam em acerto)
     */
