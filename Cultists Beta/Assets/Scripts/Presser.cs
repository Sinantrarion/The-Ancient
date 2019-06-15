using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presser : MonoBehaviour
{
    public bool IsDamaging;
    public bool Ressurect;
    public int Target;

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsDamaging == true && Ressurect == false)
            {
                GameIntel.Instance.ButtonTargetDamage(Target);
            } 
            if(IsDamaging == false && Ressurect == false)
            {
                GameIntel.Instance.ButtonTargetHeal(Target);
            }
            if(IsDamaging == false && Ressurect == true)
            {
                GameIntel.Instance.ReviveTarget(Target);
            }
        } 
    }
}