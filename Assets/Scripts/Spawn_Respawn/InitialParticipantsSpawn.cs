using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialParticipantsSpawn : MonoBehaviour
{
    [SerializeField] private int numberOfAIParticipants = 7;
    [SerializeField] private List<Transform> spawnPointsList;
    [SerializeField] private List<GameObject> enemyCharacters;
    [SerializeField] private GameObject playerMech;

    private SpawnParticipantIfAble respawnGOs;

    public List<Transform> _spawnPointsList { get { return spawnPointsList; } }

    void Start()
    {
        respawnGOs = GetComponent<SpawnParticipantIfAble>();

        respawnGOs._spawnPoints = spawnPointsList;

        for (int i = 0; i < numberOfAIParticipants; i++)
        {
            Transform spawnPoint = SelectSpawnPoint();
            SpawnMechAt(spawnPoint);
        }

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Transform spawnPoint = SelectSpawnPoint();
        Instantiate(playerMech, spawnPoint);

        respawnGOs._playerRespawnGO = playerMech;
    }

    private void SpawnMechAt(Transform spawnPoint)
    {
        int index = UnityEngine.Random.Range(0, enemyCharacters.Count);
        GameObject mechToSpawn = enemyCharacters[index];

        Instantiate(mechToSpawn, spawnPoint);

        respawnGOs.AddAiToRespawnSystem(mechToSpawn);
    }

    //TODO: use one spawn point per mech to avoid mechs spawning on top of each other
    private Transform SelectSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, spawnPointsList.Count);
        return spawnPointsList[index].transform;
    }
}
