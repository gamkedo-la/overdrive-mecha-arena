using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPlayerSpecial : MonoBehaviour
{
    [SerializeField] private Mecha mech;

    private void Awake()
    {
        switch(mech.mechType)
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
