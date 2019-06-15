using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : Ability
{
    public override void UseAbility(EnemyOrNo HeroOrEnemy)
    {
        GameIntel.Instance.Reviving(canAttack);
    }
}
