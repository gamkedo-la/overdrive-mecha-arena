using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaHazard : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float timeBetweenElectricalSurges = 5.0f;
    [SerializeField] private float surgeRadius = 40.0f;
    [SerializeField] private float normalRadius = 10.0f;
    ParticleSystem ps;

    private bool canDischargePower = false;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Physics.IgnoreLayerCollision(12, 13);

        StartCoroutine(waitBeforeSurging());
    }

    void Update()
    {
        //if (canDischargePower)
        //{
        //    canDischargePower = false;
        //    ParticleSystem.ShapeModule shape = ps.shape;
        //    shape.radius = surgeRadius;
        //}
        //else
        //{
        //    ParticleSystem.ShapeModule shape = ps.shape;
        //    shape.radius = normalRadius;
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (canDischargePower && !other.isTrigger)
        {
            Health mechaHealth = other.gameObject.GetComponent<Health>();

            if (mechaHealth != null)
            {
                //Debug.Log("Mecha hit by dangerous object!");
                mechaHealth.TakeDamage(damage, gameObject.transform);
                canDischargePower = false;
            }
            else
            {
                Debug.Log("Collider that made contact with me does not have a Health script!");
            }
        }
        else
        {
            // Could do something cool like maybe give a damage boost to the mechs that risk staying within danger range
        }
    }

    IEnumerator waitBeforeSurging()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenElectricalSurges);
            canDischargePower = true;
            //Debug.Log("Buzz");
            ps.Emit(20);
            yield return new WaitForSeconds(0.1f);
            canDischargePower = false;
        }
    }
}
