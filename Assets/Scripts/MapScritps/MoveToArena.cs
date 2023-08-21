using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//associado ao portal que transporta jogador do mapa gerado para a arena do "chefe"

public class MoveToArena : MonoBehaviour
{
    public GameObject arenaSpawnPos;//posição inicial do jogador na arena
    UIManager uiManager;
    DataGenerator dataGen;
    bool loadedUI = false;
    private void Update()
    {
        if (!loadedUI && uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            loadedUI = true;
        }//garante referência à UI
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dataGen = FindObjectOfType<DataGenerator>();

            Debug.Log("Salvou dados de dataGen");
            dataGen.SaveAsCSV();
            StartCoroutine(uiManager.FadeOut("", true, 1));
            StartCoroutine(WaitForTransport(1, other));
        }
    }

    //IEnumerator são úteis nesse cenário pois não dependem do "Time.timeScale"
    //no momento da chamada dessa função, o jogo estaria pausado
    IEnumerator WaitForTransport(int secs, Collider2D other)
    {
        yield return new WaitForSeconds(secs);
        other.gameObject.transform.position = arenaSpawnPos.transform.position;
        StartCoroutine(uiManager.FadeOut("", false, 1));
    }
}
