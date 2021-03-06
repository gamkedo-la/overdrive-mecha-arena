﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialParticipantsSpawn : MonoBehaviour
{
    //public Text AiStateDebugText;
    [SerializeField] private int numberOfAIParticipants = 7;
    [SerializeField] private List<Transform> spawnPointsList;
    [SerializeField] private List<GameObject> enemyCharacters;
    [SerializeField] private List<GameObject> playerClasses;
    [SerializeField] private List<GameObject> spawnedEnemies;

    private GameObject playerMech;

    private SpawnParticipantIfAble respawnGOs;

    private bool derandomizeSpawn = false;
    private int spawnNextDerandom = 0;
    private int spawnID = 1;

    void Awake()
    {
        switch (PlayerPrefs.GetString("Player Mech"))
        {
            case "Speed":
                playerMech = playerClasses[0];
                break;
            case "Damage":
                playerMech = playerClasses[1];
                break;
            case "All-purpose":
                playerMech = playerClasses[2];
                break;
            default:
                playerMech = playerClasses[0];
                break;
        }

        spawnedEnemies = new List<GameObject>();
        if (derandomizeSpawn)
        {
            Debug.Log("Derandomizing spawn points for testing purposes");
        }
        respawnGOs = GetComponent<SpawnParticipantIfAble>();

        respawnGOs._spawnPoints = spawnPointsList;
        SpawnPlayer();

        for (int i = 0; i < numberOfAIParticipants; i++)
        {
            Transform spawnPoint = SelectSpawnPoint();

            SpawnMechAt(spawnPoint);

            // Remove the spawn point:
            spawnPointsList.Remove(spawnPoint);
        }

    }

    private void Update()
    {
        //string aiText = "";
        //for (int i = 0; i < spawnedEnemies.Count; i++)
        //{
        //    aiText += spawnedEnemies[i].name + " " + spawnedEnemies[i].GetComponent<AICharacter>()._currentState.reasonForLastStateChange + "\n";
        //}
        //AiStateDebugText.text = aiText;
    }

    private void SpawnPlayer()
    {
        Transform spawnPoint = SelectSpawnPoint();
        GameObject playerMecha = Instantiate(playerMech, spawnPoint);

        playerMecha.name = "Player";
    }

    private void SpawnMechAt(Transform spawnPoint)
    {
        int index = UnityEngine.Random.Range(0, enemyCharacters.Count);
        GameObject mechToSpawn = enemyCharacters[index];

        GameObject newMech = Instantiate(mechToSpawn, spawnPoint);
        newMech.name += spawnID;
        spawnID++;

        spawnedEnemies.Add(newMech);
    }

    //TODO: use one spawn point per mech to avoid mechs spawning on top of each other
    private Transform SelectSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, spawnPointsList.Count);
        if (derandomizeSpawn)
        {
            index = spawnNextDerandom % spawnPointsList.Count;
            spawnNextDerandom++;
        }
        return spawnPointsList[index].transform;
    }
}
