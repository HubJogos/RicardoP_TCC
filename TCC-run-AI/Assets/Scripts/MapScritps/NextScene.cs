using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//

public class NextScene : MonoBehaviour
{
    [SerializeField] GameObject mapConfigMenu;
    MapConfig config;
    bool done = false;
    //open map config
    //if map config is accepted, send to next scene
    private void Start()
    {
        done = false;
        config = mapConfigMenu.GetComponent<MapConfig>();
    }
    private void Update()
    {
        if (config.accept && !done)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            done = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mapConfigMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
