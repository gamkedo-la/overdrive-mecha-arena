using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private ScoreKeeper scoreKeeper;
    private Health health;

    private float score = 0;
    private float totalKills = 0;
    private float totalDeaths = 0;

    private bool isFirstKillInStreak = false;
    public bool _isFirtstKillInStreak { set { isFirstKillInStreak = value; } }

    private float initialKillstreakBonus = 1.5f;
    private float currentKillstreakBonus = 0.0f;

    private float deathPenalty;
    private float deathPenaltyMultiplier = 0.66f;

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
        
    }
}
