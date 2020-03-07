using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerEmitter : MonoBehaviour
{
    [SerializeField] private GameObject kamikazePrefab;
    private BoxCollider boxTrigger;
    private float minHeight = 15.0f;
    private float spawnTimer = 0f;

    private void Start()
    {
        boxTrigger = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if(spawnTimer >= 5f)
            {
                Vector3 pos = boxTrigger.center + new Vector3(Random.Range(-boxTrigger.size.x / 2, boxTrigger.size.x / 2),
                                                              Random.Range(minHeight, boxTrigger.size.y / 2),
                                                              Random.Range(-boxTrigger.size.z / 2, boxTrigger.size.z / 2));

                Instantiate(kamikazePrefab, pos, Quaternion.identity);
                spawnTimer = 0f;
            }
        }
    }
}
