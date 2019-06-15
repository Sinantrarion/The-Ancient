using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyOrNo {Hero, Enemy}
public class Stats : MonoBehaviour
{
    public List<CharacterStats> m_CharacterStats = new List<CharacterStats> ();
    public EnemyOrNo HeroOrEnemy;
    public List<int> CD;
    [HideInInspector]
    public List<int> CurrCD;
    public float Health = 100;
    public float speed;
    [HideInInspector]
    public float initia;
    public float MagRes;
    public float PhisRes;
    public Sprite Face;

    float DamageRec, HealingRec;
    float cap = 70;
    float capRed = 0.7f;
    float MR;
    float PR;

    float randoming;
    float curspeed;
    [HideInInspector]
    public float curHealth;

    float CurMR, CurPR, CurLength, curMaxHealth;
    int curability;
    float Amount;
    float DamageoT;

    [HideInInspector]
    public int WhoAmI;
    [HideInInspector]
    public int TurnAmI;
    [HideInInspector]
    public List<float> Length;
    [HideInInspector]
    public List<string> Debuffs;



    public List<Ability> Abilities;
    public List<string> AbilityDesc;
    public List<Sprite> AbilityImage;
    public List<AudioClip> AbilitySounds;
    [HideInInspector]
    public List<bool> canattack;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        curMaxHealth = Health;
        curspeed = speed;
        curHealth = curMaxHealth;

        foreach (Ability ability in Abilities)
        {
            canattack.AddRange(ability.canAttack);
        }
        //Just changing Magical and Physical resist to better form, and if it's bigger that cap- changing it to cap.
        if (MagRes > cap)
        {
            MR = capRed;
        }
        else
        {
            MR = MagRes / 100;
        }
        if (PhisRes > cap)
        {
            PR = capRed;
        }
        else
        {
            PR = PhisRes / 100;
        }

        CurMR = MR;
        CurPR = PR;
    }


    //ROLL FOR INITIATIVE! Randoms 1-6, adds speed, and gives result as initia 
    public void Initiative()
    {

        randoming = Random.Range(1, 7);
        randoming = Mathf.Round(randoming);
        initia = randoming + curspeed;
        if (Length.Count > 0)
        {
            for (int i = 0; i < Length.Count; i++)
            {
                if (Length[i] == 0)
                {
                    switch (Debuffs[i])
                    {
                        case "HP":
                            curMaxHealth = Health;
                            Length.RemoveAt(Debuffs.IndexOf("HP"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("HP"));
                            break;
                        case "SP":
                            curspeed = speed;
                            Length.RemoveAt(Debuffs.IndexOf("SP"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("SP"));
                            break;
                        case "MR":
                            CurMR = MR;
                            Length.RemoveAt(Debuffs.IndexOf("MR"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("MR"));
                            break;
                        case "PR":
                            CurPR = PR;
                            Length.RemoveAt(Debuffs.IndexOf("PR"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("PR"));
                            break;
                        case "Slow":
                            curspeed = speed;
                            Length.RemoveAt(Debuffs.IndexOf("Slow"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("Slow"));
                            break;
                        case "Dot":
                            Length.RemoveAt(Debuffs.IndexOf("Dot"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("Dot"));
                            break;
                        case "Stun":
                            Length.RemoveAt(Debuffs.IndexOf("Stun"));
                            Debuffs.RemoveAt(Debuffs.IndexOf("Stun"));
                            break;
                        case null:
                            break;
                        default:
                            break;
                    }
                    Length.RemoveAt(i);
                    Debuffs.RemoveAt(i);
                }
                else
                {
                    if (Length != null) { 
                    Length[i]--;
                        switch (Debuffs[i])
                        {
                            case "Stun":
                                initia = 0;
                                break;
                            case "Dot":
                                curHealth -= DamageoT;
                                Amount = DamageoT * 100 / curMaxHealth;
                                Amount = Amount / 100;
                                GameIntel.Instance.HealthBarChange(true, Amount);
                                Debug.Log("Dot");
                                StartCoroutine(PlayDamaged());
                                break;
                            case null:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        for (int i = 1; i <= 3; i++)
        {
            CurrCD[i]--;
            if (CurrCD[i] < 0)
            {
                CurrCD[i] = 0;
            }
        }
    }

    public void IconTurn()
    {

        GameIntel.Instance.AbilityImage(AbilityImage, CurrCD, AbilityDesc);
    }

    private IEnumerator PlayDamaged()
    {
        yield return new WaitForSeconds(0.5f);
        if (curHealth <= 0)
        {
            anim.SetBool("Dead", true);
            GameIntel.Instance.Die(WhoAmI, TurnAmI);
            Length.Clear();
            Debuffs.Clear();
        }
        anim.SetTrigger("Damaged");
    }

    //callback on receiving Physical damage.
    public void DamagedPhis(float CurDamage, float DoT, float CurMod, float CurStunL, float CurSlowL, float CurSlowS)
    {
        if (CurDamage > 0)
        {
            DamageRec = (1 - CurMR) * CurDamage * (1 + CurMod);
            curHealth -= DamageRec;
            curHealth = Mathf.Round(curHealth);
            Amount = DamageRec * 100 / curMaxHealth;
            Amount = Amount / 100;
            GameIntel.Instance.HealthBarChange(true, Amount);
            StartCoroutine(PlayDamaged());
        }
        if (DoT > 0)
        {
            DamageoT = DoT;
            Debuffs.Add("Dot");
            Length.Add(4);
        }
        if (CurStunL > 0)
        {
            Debuffs.Add("Stun");
            Length.Add(CurStunL+1);
        }
        if (CurSlowL > 0)
        {
            curspeed =- CurSlowS;
            Debuffs.Add("Slow");
            Length.Add(CurSlowL+1);
        }
    }

    //callback on receiving Magical damage.
    public void DamagedMagical(float CurDamage, float DoT, float CurMod, float CurStunL, float CurSlowL, float CurSlowS)
    {
        if (CurDamage > 0)
        {
            DamageRec = (1 - CurMR) * CurDamage * (1 + CurMod);
            curHealth -= DamageRec;
            curHealth = Mathf.Round(curHealth);
            Amount = DamageRec * 100 / curMaxHealth;
            Amount = Amount / 100;
            GameIntel.Instance.HealthBarChange(true, Amount);
            StartCoroutine(PlayDamaged());
        }
        if (DoT > 0)
        {
            DamageoT = DoT;
            Debuffs.Add("Dot");
            Length.Add(4);
        }
        if (CurStunL > 0)
        {
            Debuffs.Add("Stun");
            Length.Add(CurStunL+1);
        }
        if (CurSlowL > 0)
        {
            curspeed =- CurSlowS;
            Debuffs.Add("Slow");
            Length.Add(CurSlowL + 1);
        }
    }

    //callback on receiving Healing.
    public void Healed(float Healing, float HMod, float SpBuff, float HpBuff, float MRBuff, float PRBuff, float length)
    {
        anim.SetTrigger("Healed");
        if (HpBuff > 0)
        {
            curMaxHealth += HpBuff;
            Debuffs.Add("HP");
            Length.Add(length+1);
        }

        if (Healing > 0)
        {
            HealingRec = Healing * (1 + HMod);
            curHealth += HealingRec;
            if (curHealth > curMaxHealth)
            {
                curHealth = curMaxHealth;
            }
            Amount = HealingRec * 100 / curMaxHealth;
            Amount = Amount / 100;
            GameIntel.Instance.HealthBarChange(false, Amount);
        }      
        if (SpBuff > 0)
        {
            curspeed += SpBuff;
            Debuffs.Add("SP");
            Length.Add(length+1);
        }        
        if (MRBuff > 0)
        {
            CurMR += MRBuff/100;
            if (CurMR > capRed)
            {
                CurMR = capRed;
            }
            Debuffs.Add("MR");
            Length.Add(length+1);
        }
        if (PRBuff > 0)
        {
            CurPR += PRBuff / 100;

            if (CurPR > capRed)
            {
                CurPR = capRed;
            }
            Debuffs.Add("PR");
            Length.Add(length+1);
        }
    }

    public void Ressurected()
    {
        anim.SetBool("Dead", false);
        GameIntel.Instance.Rez(WhoAmI);
        curHealth = 30;
    }


    //Whenever receives from GameIntel from specific ability- uses this. He checks List of abilities, uses the 1-4 out of them, activates specific animation, and so on. 
    public void UseAbility(int i)
    {
        curability = i;
        CurrCD[i] = CD[i];
        Abilities[i].UseAbility(HeroOrEnemy);
    }

    public void AnimateAbility()
    {
        anim.SetTrigger("Ability" + (curability + 1));
    }

    public void PlaySound()
    {
        if (AbilitySounds[curability] != null)
        {
            GameIntel.Instance.PlayClip(AbilitySounds[curability]);
        }
    }
}

