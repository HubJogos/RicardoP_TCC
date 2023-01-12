using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    void Awake(){
        
        DontDestroyOnLoad(this);
        if (instance == null)
            instance = this;
        else
            DestroyObject(gameObject);
    }

}

