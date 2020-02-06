using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private ScoreKeeper scoreKeeper;
    private Health health;

    private float score = 0;

    private float totalKills = 0;
    public float _totalKills { get { return totalKills; } set { totalKills = value; } }

    private float totalDeaths = 0;
    public float _totalDeaths { get { return totalDeaths; } set { totalDeaths = value; } }

    private int currentKillstreak = 0;

    private float maxKillstreakBonus = 3.0f;
    private float initialKillstreakBonus = 1.5f;
    private float additiveKillstreakBonus = 0.25f;

    private float currentKillstreakBonus = 0.0f;

    private float deathPenalty;
    private float deathPenaltyMultiplier = 0.66f;

    private bool shouldSubtractScore = true;
    public bool _shouldSubtractScore { set { shouldSubtractScore = value; } }

    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Start()
    {
        if (!scoreKeeper._participants.Contains(this))
        {
            scoreKeeper._participants.Add(this);
        }

        health = GetComponent<Health>();
        deathPenalty = health.getBaseHP;
    }

    public void AddToScore(float damage)
    {
        float scoreToAdd = 0.0f;
        if (currentKillstreak >= 1)
        {
            scoreToAdd = damage * currentKillstreakBonus;
        }
        else
        {
            scoreToAdd = damage;
        }

        score += scoreToAdd;
    }

    public void SubtractFromScore()
    {
        if (shouldSubtractScore)
        {
            totalDeaths++;
            score -= deathPenalty * totalDeaths * deathPenaltyMultiplier;
            currentKillstreak = 0;
            currentKillstreakBonus = 0.0f;
        }
    }

    public void IncreaseKillstreak()
    {
        currentKillstreak++;
        if (currentKillstreak == 1)
        {
            currentKillstreakBonus = initialKillstreakBonus;
        }
        else if (currentKillstreak > 1 && currentKillstreakBonus < maxKillstreakBonus)
        {
            currentKillstreakBonus += additiveKillstreakBonus;
        }
    }
}
