using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticipantIfAble : MonoBehaviour
{
    [TextArea]
    public string Notes = "This script passes a spawn point to each mecha's Respawn method that is responsible for when they come back";

    private List<Transform> spawnPoints;

    [Tooltip("Modify the rate (in seconds) at which mechas will respawn")]
    [SerializeField] private float respawnRate = 5.0f;

    public List<Transform> _spawnPoints { set { spawnPoints = value; } }
    public float _respawnRate { get { return respawnRate; } }


    //TODO: use one spawn point per mech to avoid mechs spawning on top of each other
    private Transform SelectSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, spawnPoints.Count);
        return spawnPoints[index].transform;
    }

    public void Respawn(GameObject character)
    {
        Health spawnee = character.GetComponent<Health>();
        Transform transform = SelectSpawnPoint();

        spawnee.ResetCharacter(transform);
    }
}
