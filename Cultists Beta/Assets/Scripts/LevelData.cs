using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level X")]
public class LevelData : ScriptableObject
{
    public List<GameObject> EnemyMelee;
    public List<GameObject> EnemyRanged;
    public List<GameObject> EnemySupport;
    public List<Sprite> Background;
    public List<AnimatorControllerParameter> AnimatorControll;
    public Sprite bossBackground;
    public GameObject Boss;
    public int NumberOfLevels;
}
