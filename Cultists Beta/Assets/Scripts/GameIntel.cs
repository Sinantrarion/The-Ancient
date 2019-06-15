using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameIntel : MonoBehaviour
{
    public static GameIntel Instance;
    [HideInInspector]
    public List<GameObject> Units;
    [HideInInspector]
    public List<GameObject> Heroes;
    //List of scripts Stats from these 6 Units(heroes\enemies, that are currently on battlefield)
    [HideInInspector]
    public List<Stats> Stat;
    //List of everyone alive
    [HideInInspector]
    public List<Stats> StatCurrently;
    [HideInInspector]
    public List<float> InitiatResult;
    public List<Stats> Turns;
    [HideInInspector]
    public List<int> CurrCD;
    public List<Image> Heads;
    public List<Image> OrderOfHeads;
    public List<Image> HealthBars;
    public List<GameObject> Healthb45;
    public GameObject arrow;
    private GameObject currentArrow;

    public List<GameObject> TargetPoint;

    public Sprite CircleBlue, CircleRed;
    public List<GameObject> Targets;
    public List<Button> AbilityButtons;
    [HideInInspector]
    public List<Image> AbilityIcon;
    [HideInInspector]
    public List<string> AbDescription;
    public GameObject Texter;
    public GameObject YouLost;
    public Image bcg;
    public Text Texting;
    int WhoIsAttacked;
    public int CurrentTurn = 0;
    int WhoSecond;
    public AudioSource source;

    private int EnemiesNumber;
    private float CurDamage, CurMod, CurStunL, CurSlowL, CurSlowS, CurDoT;
    private float CurHealing, CurHMod, CurSpBuff, CurHpBuff, CurMRBuff, CurPRBuff, CurLength;
    int CurChar, MaxCharPerRound;
    public int MaxChar;
    TypeOfDamage TypeOfDamage;
    EnemyOrNo EnemyOrNo;
    int check, check2,check3;

    public void cheat()
    {
        Stat[0].GetComponent<Stats>().curHealth = 10;
        Stat[1].GetComponent<Stats>().curHealth = 10;
        Stat[2].GetComponent<Stats>().curHealth = 10;
        Stat[3].GetComponent<Stats>().curHealth = 10;
        Stat[4].GetComponent<Stats>().curHealth = 10;
        Stat[5].GetComponent<Stats>().curHealth = 10;
    }

    public void playdead()
    {
        Heroes[3].GetComponent<Animator>().SetBool("Dead", true);
        Heroes[3].GetComponent<Animator>().SetTrigger("Damaged");
    }

    private IEnumerator SwitchToLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }

    //Things, happening on the first turn of every battle(Rolls for Initiative, and also assigns elements)
    void FirstTurn()
    { 
        DisableButton();
        CurrentTurn = -1;

        EnableAbilityButton();

        for (int i = 0; i < MaxCharPerRound; i++)
        {
            StatCurrently[i].Initiative();

            Heads[i].GetComponent<Image>().sprite = Stat[i].GetComponent<Stats>().Face;
            InitiatResult.Add(StatCurrently[i].initia);

            Turns.Add(StatCurrently[i]);
        }

        //sort
        Turns.Sort(delegate (Stats a, Stats b)
        {
            return (a.GetComponent<Stats>().initia).CompareTo(b.GetComponent<Stats>().initia);
        });
        Turns.Reverse();

        //Tell him, who they are. 
        for (int i = 0; i < MaxCharPerRound; i++)
        {
            Turns[i].GetComponent<Stats>().TurnAmI = i;
        }

        TurnIcons();
        SwitchingTurns();
    }

    //Things, happening on every turn circle(Full circle, everyone has a turn, then rolls initiative)
    public void NewTurn()
    {
        MaxCharPerRound = StatCurrently.Count;
        EnableAbilityButton();
        CurrentTurn = -1;
        Turns.Clear();
        InitiatResult.Clear();
        for (int i = 0; i < MaxCharPerRound; i++)
        {
            StatCurrently[i].Initiative();
        }
        for (int i = 0; i < MaxCharPerRound; i++)
        {
            InitiatResult.Add(StatCurrently[i].initia);
        }

        for (int i = 0; i < MaxCharPerRound; i++)
        {
            Turns.Add(StatCurrently[i]);
        }

        //sort
        Turns.Sort(delegate (Stats a, Stats b){
            return (a.GetComponent<Stats>().initia).CompareTo(b.GetComponent<Stats>().initia);
        });
        Turns.Reverse();

        for (int i = 0; i < MaxCharPerRound; i++)
        {
            Turns[i].GetComponent<Stats>().TurnAmI = i;
        }

        TurnIcons();
        SwitchingTurns();
    }

    //This happens for every small turn switch- When One hero does his turn and it changes
    public void SwitchingTurns()
    {
        //arrow destroy
        if(currentArrow != null)
        {
            Destroy(currentArrow);
        }

        CurrentTurn++;
        if (CurrentTurn >= MaxCharPerRound)
        {
            NewTurn();
        }
        else
        {
            if (Turns[CurrentTurn].GetComponent<Stats>().curHealth > 0 && Turns[CurrentTurn].GetComponent<Stats>().initia > 0)
            {
                EnableAbilityButton();
                check = 0;
                check2 = 0;
                //check if both teams are alive
                for (int i = 0; i < MaxChar; i++)
                {
                    if (Stat[i].GetComponent<Stats>().curHealth <= 0)
                    {
                        if (i >= 0 && i <= 2)
                        {
                            check++;
                        }
                        else
                        {
                            check2++;
                        }
                    }
                }
                if (check >= 3)
                {
                    YouLost.SetActive(true);
                }
                else if (check2 >= 3)
                {
                    Debug.Log("You Won!");
                    PassData.Instance.WhatLevelIsIt++;
                    StartCoroutine(SwitchToLevel());
                }
            }
            else
            {
                SwitchingTurns();
            }
            //create arrow
            if (MaxChar == 4)
            {
                //Please, for the love of god, fix it later ещ not be made by ducktape and spit. Check if Turns[CurrentTurn} is the boss, to correctly draw arrow above him
                if (CurrentTurn == 0)
                {
                    GameObject NewArrow = Instantiate(arrow, new Vector2(Turns[CurrentTurn].transform.position.x - 0.3f, Turns[CurrentTurn].transform.position.y + 2.5f), Quaternion.identity);
                    currentArrow = NewArrow;
                } else
                {
                    GameObject NewArrow = Instantiate(arrow, new Vector2(Turns[CurrentTurn].transform.position.x, Turns[CurrentTurn].transform.position.y + 1), Quaternion.identity);
                    currentArrow = NewArrow;
                }
            }
            else
            {
                GameObject NewArrow = Instantiate(arrow, new Vector2(Turns[CurrentTurn].transform.position.x, Turns[CurrentTurn].transform.position.y + 1), Quaternion.identity);
                currentArrow = NewArrow;
            }

            TurnIcons();
            Turns[CurrentTurn].IconTurn();
        }
    }

    //CallBack for switching ability icons and cooldowns
    public void AbilityImage(List<Sprite> AbilityImage, List<int> CD, List<string> AbilityDesc)
    {
        for (int i = 0; i < 4; i++)
        {
            AbilityIcon[i].sprite = null;
            AbilityButtons[i].interactable = true;
            AbilityButtons[i].gameObject.SetActive(true);
        }
        CurrCD = CD;
        //cooldowns.
        AbDescription = AbilityDesc;


        for (int i = 1; i < AbilityImage.Count; i++)
        {
            if (CurrCD[i] != 0)
            {
                AbilityButtons[i].interactable = false;
                AbilityButtons[i].GetComponent<TextMesh>().text = CurrCD[i].ToString();
            }
            else
            { // consistency in code style!
                AbilityButtons[i].GetComponent<TextMesh>().text = "";
            }
        }

        /*if (HeroOrEnemy == EnemyOrNo.Hero)
        {
            check2 = 3;
        } else
        {
            check2 = 0;
        }

        for (int i = 0; i < AbilityImage.Count; i++)
        {
            check = 0;
            for (int a = 0; a < 3; a++)
            {
                if (canattack[(i * 3) + a] == true)
                {
                    check3++;
                }
                if (canattack[(i * 3) + a] == true && StatCurrently[a+check2].GetComponent<Stats>().curHealth <= 0)
                {
                    check++;
                    Debug.Log(check);
                }
            }
            Debug.Log(check);
            if (check >= check3)
            {
                AbilityButtons[i].interactable = false;
            }
        }  */

        //assigning ability icons
        for (int i = 0; i < 4; i++)
        {
            if (AbilityImage[i] != null)
            {
                AbilityIcon[i].sprite = AbilityImage[i];
            } else
            {
                AbilityButtons[i].gameObject.SetActive(false);
            }
        }
    }

    //Start
    void Start()
    {
        for (int i = 0; i <= 3; i++) {
            AbilityIcon.Add(AbilityButtons[i].GetComponent<Image>());
        }
        Texter.SetActive(false);
        YouLost.SetActive(false);
        DisableButton();

        FirstTurn();

    }

    //Disabling Target buttons
    private void DisableButton()
    {
        for (int i = 0; i < 6; i++)
        {
            Targets[i].SetActive(false);
        }
    }

    //Disabling Ability buttons
    private void DisableAbilityButton()
    {
        for (int i = 0; i < 4; i++)
        {
            AbilityButtons[i].interactable = false;
        }
    }

    //Enabling  Ability buttons
    private void EnableAbilityButton()
    {
        for (int i = 0; i < 4; i++)
        {
            AbilityButtons[i].interactable = true;
        }
    }

    private void Awake()
    {
        Instance = this;

        PassData.Instance.GiveUnits();

        PassData.Instance.GiveEnemies();
    }

    //When you use healing ability- comes here
    public void HealingCallBack(List<bool> CanAttack, float Healing, float HMod, float SpBuff, float HpBuff, float MRBuff, float PRBuff, bool AoE, float Length, EnemyOrNo HeroOrEnemy)
    {
        check = 0;
        CurHealing = Healing;
        CurHMod = HMod;
        CurSpBuff = SpBuff;
        CurHpBuff = HpBuff;
        CurMRBuff = MRBuff;
        CurPRBuff = PRBuff;
        CurLength = Length;

        EnemyOrNo = HeroOrEnemy;

        if (AoE == true)
        {
            Turns[CurrentTurn].AnimateAbility();
            if (HeroOrEnemy == EnemyOrNo.Hero)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (CanAttack[i] && Stat[i].GetComponent<Stats>().curHealth > 0)
                    {
                        WhoIsAttacked = i;
                        Stat[i].Healed(CurHealing, CurHMod, CurSpBuff, CurHpBuff, CurMRBuff, CurPRBuff, Length);
                    } else { check++; }
                }
                if (check >= 3)
                {
                    SwitchingTurns();
                }
            }
            else
            {
                for (int i = 0; i < EnemiesNumber; i++)
                {
                    if (CanAttack[i] && Stat[i + 3].GetComponent<Stats>().curHealth > 0)
                    {
                        WhoIsAttacked = i+3;
                        Stat[i + 3].Healed(CurHealing, CurHMod, CurSpBuff, CurHpBuff, CurMRBuff, CurPRBuff, Length);
                    } else { check++; }
                }
                if (check >= 3)
                {
                    SwitchingTurns();
                }
            }
            SwitchingTurns();
        }
        else
        {
            //check- beneath who you should draw this circle- enemies or heroes.
            if (HeroOrEnemy == EnemyOrNo.Hero)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (CanAttack[i] && Stat[i + 3].GetComponent<Stats>().curHealth > 0)
                    {

                        Targets[i].GetComponent<SpriteRenderer>().sprite = CircleBlue;
                        Targets[i].GetComponent<Presser>().IsDamaging = false;
                        Targets[i].GetComponent<Presser>().Ressurect = false;
                        Targets[i].SetActive(true);

                    }
                    else { check++; }
                }
            } else
            {
                for (int i = 0; i < EnemiesNumber; i++)
                {
                    if (CanAttack[i] && Stat[i+3].GetComponent<Stats>().curHealth > 0)
                    {

                        Targets[i + 3].GetComponent<SpriteRenderer>().sprite = CircleBlue;
                        Targets[i + 3].GetComponent<Presser>().IsDamaging = false;
                        Targets[i].GetComponent<Presser>().Ressurect = false;
                        Targets[i + 3].SetActive(true);
                    } else { check++; }
                }
            }
            if (check >= 3)
            {
                SwitchingTurns();
            }
        }
    }

    //When you use damaging ability- comes here
    public void CallBack(List<bool> CanAttack, float Damage, float DamageOverTime, float Mod, float StunL, float SlowL, float SlowS, TypeOfDamage DamageType, bool AoE, EnemyOrNo HeroOrEnemy)
    {
        check = 0;
        CurDamage = Damage;
        CurMod = Mod;
        CurSlowL = SlowL;
        CurSlowS = SlowS;
        CurStunL = StunL;
        TypeOfDamage = DamageType;

        EnemyOrNo = HeroOrEnemy;

        CurDoT = DamageOverTime;
        if (AoE == true)
        {
            Turns[CurrentTurn].AnimateAbility();
            if (HeroOrEnemy == EnemyOrNo.Hero)
            {
                for (int i = 0; i < EnemiesNumber; i++)
                {
                    if (CanAttack[i] && Stat[i + 3].GetComponent<Stats>().curHealth > 0)
                    {
                        if (TypeOfDamage == TypeOfDamage.Phisical)
                        {
                            WhoIsAttacked = i+3;
                            Stat[i + 3].DamagedPhis(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);

                        }
                        else
                        {
                            WhoIsAttacked = i+3;
                            Stat[i + 3].DamagedMagical(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                        }
                    }
                    else { check++; }
                }
            } else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (CanAttack[i] && Stat[i].GetComponent<Stats>().curHealth > 0)
                    {
                        if (TypeOfDamage == TypeOfDamage.Phisical)
                        {
                            WhoIsAttacked = i;
                            Stat[i].DamagedPhis(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                        }
                        else
                        {
                            WhoIsAttacked = i;
                            Stat[i].DamagedMagical(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                        }
                    }
                    else { check++; }
                }
            }
            SwitchingTurns();
        }
        else
        {
            if (HeroOrEnemy == EnemyOrNo.Hero)
            {
                for (int i = 0; i < EnemiesNumber; i++)
                {
                    if (CanAttack[i] && Stat[i + 3].GetComponent<Stats>().curHealth > 0)
                    {
                        Targets[i + 3].GetComponent<SpriteRenderer>().sprite = CircleRed;
                        Targets[i + 3].GetComponent<Presser>().IsDamaging = true;
                        Targets[i].GetComponent<Presser>().Ressurect = false;
                        Targets[i + 3].SetActive(true);
                    }
                    else { check++; }
                }
            } else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (CanAttack[i] && Stat[i].GetComponent<Stats>().curHealth > 0)
                    {
                        Targets[i].GetComponent<SpriteRenderer>().sprite = CircleRed;
                        Targets[i].GetComponent<Presser>().IsDamaging = true;
                        Targets[i].GetComponent<Presser>().Ressurect = false;
                        Targets[i].SetActive(true);
                    } else { check++; }
                }
            }
            if (check >= 3)
            {
                SwitchingTurns();
            }
        }
    }

    //callback on targeting target with damage. 
    public void ButtonTargetDamage(int target)
    {
        WhoIsAttacked = target-1;
        if (EnemyOrNo == EnemyOrNo.Hero)
        {
            if (TypeOfDamage == TypeOfDamage.Phisical)
            {
                Turns[CurrentTurn].AnimateAbility();
                Stat[target - 1].DamagedPhis(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                DisableButton();
            }
            else
            {
                Turns[CurrentTurn].AnimateAbility();
                Stat[target - 1].DamagedMagical(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                DisableButton();
            }
        } else
        {
            if (TypeOfDamage == TypeOfDamage.Phisical)
            {
                Turns[CurrentTurn].AnimateAbility();
                Stat[target - 1].DamagedPhis(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                DisableButton();
            }
            else
            {
                Turns[CurrentTurn].AnimateAbility();
                Stat[target - 1].DamagedMagical(CurDamage, CurDoT, CurMod, CurStunL, CurSlowL, CurSlowS);
                DisableButton();
            }
        }
        SwitchingTurns();
    } 

    //callback on targeting target with healing.
    public void ButtonTargetHeal(int target)
    {
        WhoIsAttacked = target-1;
        Turns[CurrentTurn].AnimateAbility();
        Stat[target - 1].Healed(CurHealing, CurHMod, CurSpBuff, CurHpBuff, CurMRBuff, CurPRBuff, CurLength);
        DisableButton();
        SwitchingTurns();
    }

    public void AbilityButtonElem(int target)
    {
        DisableAbilityButton();
        Turns[CurrentTurn].UseAbility(target);
    }

    public void HealthBarChange(bool Damaged, float Amount)
    {
        if (Damaged == true)
        {
            HealthBars[WhoIsAttacked].fillAmount -= Amount;
        } else
        {
            HealthBars[WhoIsAttacked].fillAmount += Amount;
        }
    }

    private void TurnIcons()
    {
        for (int i = 0; i < 6; i++)
        {
            OrderOfHeads[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < MaxCharPerRound; i++)
        {
            if(Turns != null)
            {
                OrderOfHeads[i].GetComponent<Image>().sprite = Turns[i].GetComponent<Stats>().Face;
            } else
            {
                NewTurn();
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (OrderOfHeads[i].GetComponent<Image>().sprite == null)
            {
                OrderOfHeads[i].enabled = false;
            }
            else
            {
                OrderOfHeads[i].enabled = true;
            }
        }
    }

    public void Die(int who, int turnami)
    {
        StatCurrently.RemoveAt(who);
        Turns.RemoveAt(turnami);
        MaxCharPerRound = StatCurrently.Count;
        
        TurnIcons();
    }

    public void Rez(int who)
    {
        StatCurrently.Insert(who, Stat[who]);
    }

    public void TooltipOn(int AbilityNumber)
    {
        Texter.SetActive(true);
        Texting.GetComponent<Text>().text = AbDescription[AbilityNumber]; 
    }

    public void TooltipOff()
    {
        Texter.SetActive(false);
        Texting.GetComponent<Text>().text = "";

    }

    public void TakeUnits(List<GameObject> TakeUnitt)
    {
        Units.Clear();
        for (int i = 0; i < 3; i++)
        {
            Units.Add(TakeUnitt[i]);
            GameObject Hero = Instantiate(Units[i], TargetPoint[i].transform.position, Quaternion.identity);
            Heroes.Add(Hero);
        }
        for (int i = 0; i < 3; i++)
        {
            Stat.Add(Heroes[i].GetComponent<Stats>());
            Heroes[i].GetComponent<Stats>().WhoAmI = i;
            StatCurrently.Add(Stat[i]);
        }
    }

    public void BossLevel(GameObject Boss, Sprite bossBackGround)
    {
        bcg.GetComponent<Image>().sprite = bossBackGround;
        for (int i = 0; i < 3; i++)
        {
            Units.Add(Boss);
        }
        GameObject Bos = Instantiate(Units[3], TargetPoint[4].transform.position, Quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            Heroes.Add(Bos);
        }
        MaxCharPerRound = 4;
        MaxChar = 4;
        EnemiesNumber = 1;

        Turns.Clear();
        InitiatResult.Clear();

        Stat.Add(Heroes[3].GetComponent<Stats>());
        Heroes[3].GetComponent<Stats>().WhoAmI = 3;
        StatCurrently.Add(Stat[3]);

        Healthb45[0].SetActive(false);
        Healthb45[1].SetActive(false);
    }

    public void Level(LevelData dat)
    {
        int a = Random.Range(0, dat.Background.Count);
        bcg.GetComponent<Image>().sprite = dat.Background[a];
        if (a == 0)
        {
            bcg.GetComponent<Animator>().enabled = true;
        }
        else
        {
            bcg.GetComponent<Animator>().enabled = false;
        }

        int HowMany = 0;
        if (dat.EnemySupport.Count != 0)
        {
            HowMany = 3;
        }
        else
        {
            HowMany = 2;
        }
        Units.Add(dat.EnemyMelee[Random.Range(0, dat.EnemyMelee.Count)]);

        WhoSecond = Random.Range(1, HowMany + 1);
        int WhoThird = Random.Range(1, HowMany);
        switch (WhoSecond)
        {
            case 1:
                Units.Add(dat.EnemyMelee[Random.Range(0, dat.EnemyMelee.Count)]);
                break;
            case 2:
                Units.Add(dat.EnemyRanged[Random.Range(0, dat.EnemyRanged.Count)]);
                break;
            case 3:
                Units.Add(dat.EnemySupport[Random.Range(0, dat.EnemySupport.Count)]);
                break;
            default:
                break;
        }
        switch (WhoThird)
        {
            case 1:
                Units.Add(dat.EnemyRanged[Random.Range(0, dat.EnemyRanged.Count)]);
                break;
            case 2:
                Units.Add(dat.EnemySupport[Random.Range(0, dat.EnemySupport.Count)]);
                break;
            default:
                break;
        }
        for (int i = 3; i < 6; i++)
        {
            GameObject Hero = Instantiate(Units[i], TargetPoint[i].transform.position, Quaternion.identity);
            Heroes.Add(Hero);
        }

        MaxCharPerRound = Heroes.Count;
        MaxChar = Heroes.Count;

        Turns.Clear();
        InitiatResult.Clear();

        for (int i = 3; i < MaxCharPerRound; i++)
        {
            Stat.Add(Heroes[i].GetComponent<Stats>());
            Heroes[i].GetComponent<Stats>().WhoAmI = i;
            StatCurrently.Add(Stat[i]);
        }
        EnemiesNumber = 3;
    }

    public void PlayClip(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    public void Reviving(List<bool> CanAttack)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CanAttack[i] && Stat[i].GetComponent<Stats>().curHealth <= 0)
            {
                Targets[i].GetComponent<SpriteRenderer>().sprite = CircleBlue;
                Targets[i].GetComponent<Presser>().IsDamaging = false;
                Targets[i].GetComponent<Presser>().Ressurect = true;
                Targets[i].SetActive(true);
            }
        }
    }

    public void ReviveTarget(int target)
    {
        Turns[CurrentTurn].AnimateAbility();
        Stat[target - 1].Ressurected();
        DisableButton();
        SwitchingTurns();
    }

}
