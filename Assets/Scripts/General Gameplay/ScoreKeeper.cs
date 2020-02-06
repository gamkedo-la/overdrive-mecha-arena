using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    [TextArea(1,10)]
    public string SystemSummary = "Score system details: Points will awarded or removed throughout the battle based on the mecha's score, death count, and kill count.\n" +
                                    "Score points gained will be equal to the HP lost by the defender.\n" +
                                    "Deaths will remove that mecha's HP x (their death count x 0.66) from their score.\n" + 
                                    "Any death incurred after gaining the kill bonus will reset the bonus to 0%. All points gained during the killstreak will not be affected.\n" +
                                    "The first kill will add a bonus of 50% to any score gained after the kill. Each subsequent kill will increase the bonus by 25% until the maximum of 300% is reached.\n" +
                                    "At the end of the battle, the victor will be the mecha that holds the highest score.";

    [SerializeField] private TextMeshProUGUI playerRanking;
    [SerializeField] private List<GameObject> rankIndicators;

    private List<ScoreHandler> participants = new List<ScoreHandler>();
    public List<ScoreHandler> _participants { get { return participants; } }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
