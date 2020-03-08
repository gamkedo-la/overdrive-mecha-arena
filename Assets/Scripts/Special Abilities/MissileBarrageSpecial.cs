using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBarrageSpecial : SpecialAbility
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private List<Transform> missileSpawnPoints; //these are part of the mech prefab GO

    private int barrageDamage = 20;

    protected override void Start()
    {
        base.Start();

        barrageDamage = _mech.strengthOfSpecialAbility;

        if(gameObject.GetComponent<PlayerMovement>())
        {
            AssignPlayerSpecial assignPlayerSpecial = GetComponent<AssignPlayerSpecial>();

            missilePrefab = assignPlayerSpecial._missilePrefab;

            missileSpawnPoints = assignPlayerSpecial._missileSpawnPoints;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (CanUseSpecial())
        {
            if (Input.GetButtonDown("Fire3") && gameObject.tag == "Player")
            {
                UseMissileBarrageSpecial();
            }
        }
    }

    public void UseMissileBarrageSpecial()
    {
        specialCooldownTimer = 0.0f;
        isSpecialInUse = true;

        Debug.Log("Launching Missiles!");

        StartCoroutine(spawnMissilesOverTime());
    }

    IEnumerator spawnMissilesOverTime()
    {
        while(isSpecialInUse)
        {
            Instantiate(missilePrefab, missileSpawnPoints[0]);

            yield return new WaitForSeconds(0.75f);
        }
    }

    public void UseSpecial()
    {
        Debug.Log("AI using UseMissileBarrageSpecial");
        UseMissileBarrageSpecial();
    }
}
