using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleScoreBoard : MonoBehaviour
{
    void Start()
    {
        GameObject scoreKeeperGO = GameObject.Find("ScoreSystem");
        if(scoreKeeperGO != null)
        {
            Debug.Log("Score Keeper found");
        }
        else
        {
            Debug.Log("Score Keeper not found");
            return;
        }

        ScoreKeeper skScript = scoreKeeperGO.GetComponent<ScoreKeeper>();
        string[] rankNames = { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th"};


        for (int i = 0; i < skScript._mechScoreData.Count; i++)
        {
            Transform mech = transform.Find("" + i);

            TextMeshProUGUI nameText = mech.Find("Name Text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreText = mech.Find("Score Text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI killsText = mech.Find("Kills Text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI deathsText = mech.Find("Deaths Text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI rankText = mech.Find("Rank Text").GetComponent<TextMeshProUGUI>();

            string[] thisMechData = skScript.ReturnMechData(i);
            nameText.text = thisMechData[0];
            scoreText.text = thisMechData[1];
            killsText.text = thisMechData[2];
            deathsText.text = thisMechData[3];
            rankText.text = rankNames[i];
        }
    }
}
