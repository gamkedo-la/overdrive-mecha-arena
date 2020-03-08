using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Mecha mech;
    [SerializeField] private GameObject shieldGO;
    [SerializeField] private float shieldToHealthConversionLimitMultiplier = 1.5f;
    private int startingHP = 200;


    private PlayerCamNoise playerCamNoise;
    private SetVcamFollowAndLookAt setVcamScript;
    private ScoreHandler scoreHandler;
    public ScoreHandler _scoreHandler { get { return scoreHandler; } }

    private Transform myAttacker;

    private float respawnTimer = 0.0f;

    private float currentHP;

    private string[] priorityValues = new string[] { "high", "medium", "low" };
    private string targetPriority;

    private bool isInvulnerable = false;

    private SpawnParticipantIfAble respawnManager;
    public Mecha _mech { get { return mech; } }
    public bool _isInvulnerable { set { isInvulnerable = value; } }
    public Transform _myAttacker { set { myAttacker = value; } get { return myAttacker; } }

    private DoubleStatsSpecial doubleStats;
    AudioOcclusion AOScript;
    public float getPriorityScore
    {
        get
        {
            if (targetPriority == "high")
            {
                return 0.0f;
            }
            else if (targetPriority == "medium")
            {
                return 100.0f;
            }
            else
            {
                return 200.0f;
            }
        }
    }
    public int getBaseHP { get { return startingHP; } }
    public float getCurrentHealthAsPercentage { get { return currentHP / startingHP * 100; } }
    public float getCurrentHP { get { return currentHP; } }
    public float healthRegen { set { currentHP += value; } }
    public int _shieldLevels { get { return (int)(shields / normalShieldLevels * 100); } }

    private float timer;
    [SerializeField] private float shieldRechargeDelay = 7.5f;
    private float shieldRechargeRate = 1f;
    private float normalShieldLevels;
    private float shields = 100;

    private float shieldRegenTimeLimit = 2.0f;
    private float shieldTimer = 1.5f;
    private bool shieldRechargingLocked = false;
    private float rechargeSlower = 0.5f;
    private int shieldsChargesLeft = 2;

    private bool isUsingShield = false;
    public bool _isUsingShield { set { isUsingShield = value; } }// Used by AI to turn shield on or off

    private void Start()
    {
        AOScript = gameObject.GetComponent<AudioOcclusion>();
        if (!gameObject.CompareTag("Non-playables"))
        {
            startingHP = mech.health;
            currentHP = startingHP;
            normalShieldLevels = mech.defense;
            shields = mech.defense;
            shieldsChargesLeft = mech.shieldCharges;

            scoreHandler = GetComponent<ScoreHandler>();

            isUsingShield = false;
            ShouldUseShield(false);
        }

        respawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnParticipantIfAble>();

        if (gameObject.CompareTag("Player"))
        {
            setVcamScript = GetComponent<SetVcamFollowAndLookAt>();
            playerCamNoise = setVcamScript._vcam.GetComponent<PlayerCamNoise>();
        }

        if (GetComponent<DoubleStatsSpecial>() != null)
        {
            doubleStats = GetComponent<DoubleStatsSpecial>();
        }
    }

    private void Update()
    {
        if (!gameObject.CompareTag("Non-playables"))
        {
            if (isUsingShield)
            {
                shieldTimer -= Time.deltaTime;
                if (shieldTimer <= 0)
                {
                    ShouldUseShield(false);
                }
            }
            else
            {
                shieldTimer += Time.deltaTime * rechargeSlower;
                if (shieldTimer >= shieldRegenTimeLimit)
                {
                    shieldTimer = shieldRegenTimeLimit;
                    shieldRechargingLocked = false;
                }
            }

            HandlePlayerShield();

            if (shields <= normalShieldLevels)
            {
                RechargeShieldIfAble();
            }

            HandleDrunkenBehavior();
        }
    }

    private void HandlePlayerShield()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (shields > 0)
            {
                ShouldUseShield(Input.GetButton("Shield"));
            }
            else
            {
                ShouldUseShield(false);
            }
        }
    }

    private void ShouldUseShield(bool isOn)
    {
        if ((shieldRechargingLocked || shieldsChargesLeft <= 0) && isOn)
        {
            return;
        }
        _isUsingShield = isOn;
        shieldGO.SetActive(isOn);
        if (isOn)
        {
            shieldsChargesLeft--;
            shieldRechargingLocked = true;
        }
    }

    private void HandleDrunkenBehavior()
    {
        if (gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player has " + currentHP + " out of " + startingHP);
            if (currentHP > startingHP)
            {
                playerCamNoise.SetCamNoise(true);
            }
            else
            {
                playerCamNoise.SetCamNoise(false);
            }
        }
        else if (gameObject.CompareTag("Enemy") && !gameObject.CompareTag("Non-playables"))
        {
            if (currentHP > startingHP)
            {
                // Enabled drunken behavior for AI mechs
            }
            else
            {
                // Disable drunken behavior for AI mechs
            }
        }
    }

    private void RechargeShieldIfAble()
    {
        if (!isUsingShield)
        {
            timer += Time.deltaTime;

            if (timer >= shieldRechargeDelay)
            {
                shields += shieldRechargeRate * Time.deltaTime;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    public bool IsDying()
    {
        return currentHP <= 0;
    }

    private void OnEnable()
    {
        if (!gameObject.CompareTag("Non-playables"))
        {
            CancelInvoke("SpawnCooldown");
            respawnTimer = 0;
            currentHP = startingHP;
            shields = normalShieldLevels;
            shieldsChargesLeft = mech.shieldCharges;
        }
    }

    public void TakeDamage(int damageAmount, Transform attacker, bool attackerInOverdrive = false)
    {
        if (!isInvulnerable)
        {
            int finalDamageAmount;
            if (attackerInOverdrive)
            {
                finalDamageAmount = damageAmount * 2;
            }
            else
            {
                finalDamageAmount = damageAmount;
            }

            ScoreHandler attackerScore = null;

            //update myAttacker to reflect this agent's current attacker; this will then be available to the Retreat State so this agent can run away from the attacker
            if (attacker.CompareTag("Player") || attacker.CompareTag("Enemy"))
            {
                myAttacker = attacker;
                attackerScore = attacker.GetComponent<ScoreHandler>();
            }

            if (isUsingShield && shields > 0)
            {
                shields -= finalDamageAmount;
                if (shields <= 0)
                {
                    //DetonateElectricalCharge();
                }
            }
            else
            {
                currentHP -= finalDamageAmount;

                if (attackerScore != null)
                {
                    attackerScore.AddToScore(finalDamageAmount);
                }

                //Debug.Log(gameObject.name + " took " + damageAmount + " damage, now has hp: " + currentHP);
                if (gameObject.CompareTag("Enemy"))
                {
                    ShouldUseShield(true);
                }

                SetMyValueAsATarget();

                if (currentHP <= 0)
                {
                    if (attackerScore != null)
                    {
                        attackerScore.IncreaseKillstreak();
                    }

                    if (!gameObject.CompareTag("Non-playables"))
                    {
                        Die();
                    }
                    else
                    {
                        AOScript.audio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        AOScript.audio.release();
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            //Debug.Log(gameObject.name + " is INVULNERABLE!!!");
        }
    }

    public bool isInOverdrive()
    {
        return currentHP > startingHP;
    }

    public void StealShieldAndConvertToHP(int damage, Transform attacker, Health callerHealth)
    {
        if (!isInvulnerable && shields > 0)
        {
            shields -= damage;
            //Debug.Log(gameObject.name + " took " + damage + " shield damage, now has: " + shields);

            //update myAttacker to reflect this agent's current attacker; this will then be available to the Retreat State so this agent can run away from the attacker
            if (attacker.CompareTag("Player") || attacker.CompareTag("Enemy"))
            {
                myAttacker = attacker;
            }

            if (shields <= 0)
            {
                //DetonateElectricalCharge();
            }

            if (callerHealth.getCurrentHP <= callerHealth.getBaseHP * shieldToHealthConversionLimitMultiplier)
            {
                callerHealth.healthRegen = damage;
                //Debug.Log(callerHealth.gameObject.name + " converted " + damage + " enemy shields points for health");
            }
        }
        else
        {
            //Debug.Log(gameObject.name + " is INVULNERABLE OR HAS NO SHIELDS!!!");
        }
    }

    private void SetMyValueAsATarget()
    {
        if (currentHP >= startingHP / 3)
        {
            targetPriority = priorityValues[2];
        }
        else if (currentHP >= startingHP / 2 && currentHP < startingHP / 3)
        {
            targetPriority = priorityValues[1];
        }
        else
        {
            // This GO has low HP and so it becomes an easier target to destroy
            targetPriority = priorityValues[0];
        }
    }

    private void Die()
    {
        scoreHandler.SubtractFromScore();
        scoreHandler._shouldSubtractScore = false;

        InvokeRepeating("SpawnCooldown", 0.0f, 1.0f);
        gameObject.SetActive(false);
    }

    public void ResetCharacter(Transform spawnPoint)
    {
        scoreHandler._shouldSubtractScore = true;

        gameObject.transform.position = spawnPoint.position;
        gameObject.SetActive(true);
    }

    private void SpawnCooldown()
    {
        respawnTimer++;

        if (respawnManager != null && respawnTimer >= respawnManager._respawnRate)
        {
            respawnManager.Respawn(gameObject);
        }
    }
}
