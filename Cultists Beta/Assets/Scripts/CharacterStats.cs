using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{

    public string name;                            // Inspector Element Name
    public int health;
    public int speed;
    public int magRes;
    public int phisRes;
    public Sprite Face;
    public List<Ability> Abilities;

    public CharacterStats(string newName, int newHealth, int newSpeed, int newMagRes, int newPhisRes,
        Sprite newFace, List<Ability> newAbility)
    {
        name = newName;
        health = newHealth;
        speed = newSpeed;
        magRes = newMagRes;
        Face = newFace;
        Abilities = newAbility;
    }

}
