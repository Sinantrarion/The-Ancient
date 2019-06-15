using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing_Ability : Ability
{
    public float Healing;
    public float Mod;
    public float BuffToSpeed;
    public float BuffToHP;
    public float BuffToMR;
    public float BuffToPR;
    public float Length;

    
    public bool AoE;

    //Just passes a lot of elements. 
    public override void UseAbility(EnemyOrNo HeroOrEnemy)
    {
        GameIntel.Instance.HealingCallBack(canAttack, Healing, Mod, BuffToSpeed, BuffToHP, BuffToMR, BuffToPR, AoE, Length, HeroOrEnemy);
    }
}
