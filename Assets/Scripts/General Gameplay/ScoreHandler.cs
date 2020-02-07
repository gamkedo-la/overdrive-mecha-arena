using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private ScoreKeeper scoreKeeper;
    private Health health;

    private Canvas HUD;
    private UpdatePlayerScore playerRank;

    private float score = 0;
    public float _score { get { return score; } }

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

    private GameObject topFourRankIndicator;

    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();

        if (gameObject.CompareTag("Player"))
        {
            HUD = FindObjectOfType<Canvas>();

            playerRank = HUD.GetComponentInChildren<UpdatePlayerScore>();
        }
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

    private void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerRank.UpdatePlayerScoreStuff(score, currentKillstreak, totalDeaths, scoreKeeper._rankings.FindIndex(x => x.CompareTag("Player")) + 1);
        }
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

            GameObject oldRank = topFourRankIndicator;
            Destroy(oldRank);

            topFourRankIndicator = null;
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

    public void SetTopFourIndicator(GameObject rankIndicator)
    {
        if (!gameObject.CompareTag("Player"))
        {
            if (topFourRankIndicator != null)
            {
                GameObject oldRank = topFourRankIndicator;
                Destroy(oldRank);

                topFourRankIndicator = null;
            }

            topFourRankIndicator = Instantiate(rankIndicator, transform.position + (Vector3.up * 35f), Quaternion.identity);
        }
    }
}
