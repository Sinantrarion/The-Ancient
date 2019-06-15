using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfDamage { Phisical, Magical }
public class Damage_Ability : Ability
{
    public TypeOfDamage DamageType;

    public float Damage = 0;
    public float DamageOverTime = 0;
    public float Mod = 0;
    public int StunLength = 0;
    public int SlowLenght = 0;
    public int SlowStrength = 0;

    public bool AoE;
    
    //Just passes a lot of elements. 
    public override void UseAbility(EnemyOrNo HeroOrEnemy)
    {
        GameIntel.Instance.CallBack(canAttack, Damage, DamageOverTime, Mod, StunLength, SlowLenght, SlowStrength, DamageType, AoE, HeroOrEnemy);
    }
}
