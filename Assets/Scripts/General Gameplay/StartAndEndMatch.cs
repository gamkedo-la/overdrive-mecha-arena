using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class StartAndEndMatch : MonoBehaviour
{
    [SerializeField] private GameObject ScoreManager;
    [SerializeField] private GameObject AudioManager;

    [SerializeField] private TextMeshProUGUI tmProText;
    [SerializeField] private float timeLimit;

    private float timer;
    private bool allowCountdown = true;
    private bool countdownOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0.0f && allowCountdown)
        {
            timer -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

            tmProText.text = timeSpan.ToString("mm':'ss");
        }
        else if(timer <= 0.0f && !countdownOnce)
        {
            allowCountdown = false;
            countdownOnce = true;

            timer = 0.0f;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
            tmProText.text = timeSpan.ToString("mm':'ss");

            GameOver();
        }
    }

    public void ResetTimeLimit()
    {
        timer = timeLimit;
        allowCountdown = true;
        countdownOnce = false;
    }

    private void GameOver()
    {
        // 0 is the Main Menu scene and should never be changed
        Cursor.lockState = CursorLockMode.None;
        DontDestroyOnLoad(ScoreManager);
        DontDestroyOnLoad(AudioManager);
        SceneManager.LoadScene("Final Results", LoadSceneMode.Single);//Change to results scene once it's implemented
    }
}
