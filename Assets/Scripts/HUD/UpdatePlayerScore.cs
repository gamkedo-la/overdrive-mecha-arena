using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdatePlayerScore : MonoBehaviour
{
    private ScoreHandler playerScore;

    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalDeathsText;
    [SerializeField] private TextMeshProUGUI killstreakText;

    public void UpdatePlayerScoreStuff(float score, float killstreak, float deaths, int rank)
    {
        string correctedRankString = rank.ToString();
        switch(rank)
        {
            case 1:
                correctedRankString += "st";
                break;
            case 2:
                correctedRankString += "nd";
                break;
            case 3:
                correctedRankString += "rd";
                break;
            default:
                correctedRankString += "th";
                break;
        }

        rankText.SetText(correctedRankString);
        scoreText.SetText(score.ToString());
        totalDeathsText.SetText("Deaths: " + deaths.ToString());
        killstreakText.SetText("Killstreak: " + killstreak.ToString());
    }
}
