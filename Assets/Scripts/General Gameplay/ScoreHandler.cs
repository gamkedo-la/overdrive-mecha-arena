using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private Health health;

    private float score = 0;
    private float kills = 0;
    private float deaths = 0;

    private float killstreakBonus = 1.5f;
    private float deathPenalty;

    private void Start()
    {
        health = GetComponent<Health>();
        deathPenalty = health.getBaseHP;
    }
}
