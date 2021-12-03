using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int playerLevel = 1;
    public int maxLevel = 99;
    public int currentExp;
    public int baseExp = 100;
    public Text levelText;
    public int[] expToLevelUp;

    // Start is called before the first frame update
    void Start()
    {
        levelText.text = "Level: " + playerLevel;//sets screen text for player level

        expToLevelUp = new int[maxLevel];
        expToLevelUp[1] = baseExp;
        for(int i = 2; i < expToLevelUp.Length; i++)
        {
            expToLevelUp[i] = Mathf.FloorToInt(expToLevelUp[i-1] * 1.1f);//sets experience thresholds for leveling up
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
