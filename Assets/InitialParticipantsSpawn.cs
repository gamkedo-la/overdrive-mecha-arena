using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialParticipantsSpawn : MonoBehaviour
{
    [SerializeField] private int numberOfParticipants = 8;
    [SerializeField] private List<GameObject> spawnPointsList;
    [SerializeField] private List<GameObject> enemyCharacters;
    [SerializeField] private GameObject playerMech;

    void Start()
    {
        for (int i = 0; i < numberOfParticipants; i++)
        {
            Transform spawnPoint = SelectSpawnPoint();
            SpawnMechAt(spawnPoint);
        }

        //SpawnPlayer();
    }

    void Update()
    {
        //Debug.Log(spawnPointsList.Count);
    }

    private void SpawnMechAt(Transform spawnPoint)
    {
        int index = UnityEngine.Random.Range(0, enemyCharacters.Count);
        GameObject mechToSpawn = enemyCharacters[index];

        Instantiate(mechToSpawn, spawnPoint);
    }

    private Transform SelectSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, spawnPointsList.Count);
        return spawnPointsList[index].transform;
    }
}
