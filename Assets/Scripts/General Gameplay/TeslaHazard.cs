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
        ps = GetComponentInParent<ParticleSystem>();
        Physics.IgnoreLayerCollision(12, 13);

        StartCoroutine(waitBeforeSurging());
    }

    void Update()
    {
        if(canDischargePower)
        {
            canDischargePower = false;
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.radius = surgeRadius;
        }
        else
        {
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.radius = normalRadius;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(canDischargePower)
        {
            Health mechaHealth = other.gameObject.GetComponent<Health>();

            if (mechaHealth != null)
            {
                Debug.Log("Mecha hit by dangerous object!");
                mechaHealth.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Collider that made contact with me does not have a Health script!");
            }
        }
        else
        {
            // Could do something cool like maybe give a damage boost to the mechs that stay withing danger range
        }
    }

    IEnumerator waitBeforeSurging()
    {
        yield return new WaitForSeconds(timeBetweenElectricalSurges);
        canDischargePower = true;
    }
}
