using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticipantIfAble : MonoBehaviour
{
    private List<Transform> spawnPoints;

    private List<GameObject> aiCharacters = new List<GameObject>();
    private GameObject playerCharacter;

    [Tooltip("Modify the rate (in seconds) at which mechas will respawn")]
    [SerializeField] private float respawnRate = 5.0f;
    private float respawnTimer = 0.0f;

    public GameObject _playerRespawnGO { set { playerCharacter = value; } }
    public List<Transform> _spawnPoints { set { spawnPoints = value; } }

    private void Update()
    {
        
    }

    public void AddAiToRespawnSystem(GameObject character)
    {
        aiCharacters.Add(character);
    }
}
