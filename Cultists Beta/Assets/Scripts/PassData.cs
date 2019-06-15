using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassData : MonoBehaviour
{
    public static int levelID = 0;
    public int WhatLevelIsIt = 0;
    public static PassData Instance;
    public List<GameObject> Unit;
    public LevelData[] levels;
    public LevelData currentLevel;
    // Start is called before the first frame update
    void Awake()
    {
        currentLevel = levels[levelID];
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Send(List<GameObject> SentUnits)
    {
        for(int i = 0; i < 3; i++)
        {
            Unit.Add(SentUnits[i]);
        }
    }

    public void GiveUnits()
    {
        if (WhatLevelIsIt > currentLevel.NumberOfLevels)
        {
            Debug.Log("You Absolutely Won");
        }
        else
        {
            GameIntel.Instance.TakeUnits(Unit);
        }
    }
    public void GiveEnemies()
    {
        if (WhatLevelIsIt == currentLevel.NumberOfLevels)
        {
            GameIntel.Instance.BossLevel(currentLevel.Boss, currentLevel.bossBackground);
        }
        else if (WhatLevelIsIt < currentLevel.NumberOfLevels)
        {
            GameIntel.Instance.Level(currentLevel);
        }
    }
}
