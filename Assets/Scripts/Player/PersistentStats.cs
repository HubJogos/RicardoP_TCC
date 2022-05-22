using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerLevel = 1;
    public int currentExp = 0;

    [Header("Health Variables")]
    public int maxHealth = 50;
    public bool closedTutorial;
}
