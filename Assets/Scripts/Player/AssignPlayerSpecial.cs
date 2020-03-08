using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPlayerSpecial : MonoBehaviour
{
    [SerializeField] private Mecha mech;
    //TODO: Create list of special ability VFX/GOs that will gotten by the player's current ability script and used by it
    [SerializeField] private List<GameObject> specialsVFX;
    [SerializeField] private GameObject missilePrefab;

    [SerializeField] private List<Transform> missileSpawnPoints;

    public GameObject _missilePrefab { get { return missilePrefab; } }
    public List<Transform> _missileSpawnPoints { get { return missileSpawnPoints; } }

    private void Awake()
    {
        switch (mech.mechType)
        {
            case "Speed":
                gameObject.AddComponent<InvulnerabilitySpecial>();
                break;
            case "Damage":
                gameObject.AddComponent<MissileBarrageSpecial>();
                break;
            case "All-purpose":
                gameObject.AddComponent<DoubleStatsSpecial>();
                break;
        }
    }
}
