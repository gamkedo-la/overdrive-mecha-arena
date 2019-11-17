using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mecha", menuName = "Mecha")]
public class Mecha : ScriptableObject
{
    [Header("UI Display Properties")]
    [Tooltip("These are the possible names that will displayed for mechs of this type")]
    public List<string> names;
    public Sprite facePortrait;

    [Header("Health Properties")]
    public int health;
    public int defense;

    [Header("Special Ability Properties")]
    public float specialCooldown;
    public float specialUseTimeLimit;
    [Tooltip("How much damage will this ability cause, if any at all?")]
    public int strengthOfSpecialAbility;

    [Header("Shooting Properties")]
    [Range(1.0f, 5.0f)] public int damage;
    [Range(150.0f, 350.0f)] public float range;
    [Range(0.1f, 1.0f)] public float fireRate;

    [Header("Player and AI Movement Properties")]
    public float fowardMoveSpeed;
    public float backwardMoveSpeed;

    [Tooltip("Base speed will be multiplied by the dash speed value.")]
    [Range(1.0f, 2.5f)] public float dashSpeed;
    public float dashTimeLimit;

    [Header("Player only Movement Properties")]
    public float turnSpeed;
    public float strafingRightMoveSpeed;
    public float strafingLeftMoveSpeed;
}
