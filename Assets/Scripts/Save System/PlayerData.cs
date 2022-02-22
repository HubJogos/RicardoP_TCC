using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{
    public int level;
    public int health;
    public float[] position;

    public PlayerData(PlayerScript script)
    {
        level = script.playerLevel;
        health = script.currentHealth;
        position = new float[3];
        position[0] = script.transform.position.x;
        position[1] = script.transform.position.y;
        position[2] = script.transform.position.z;
    }



}
