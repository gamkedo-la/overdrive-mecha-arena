using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mecha", menuName = "Mecha/Knight")]
public class KnightMecha : ScriptableObject
{
    public new string name;
    public Sprite facePortrait;

    public int health;
    public int defense;

    public int lightMeleeDMG;
    public int heavyMeleeDMG;
    public float meleeRange;

    public int specialAbilityPowerLevel;

    public int shotDamage;
    public float fireRate;
    public float shotRange;

    public float shotAccuracy;
    public float randomShotHitChance;
}
