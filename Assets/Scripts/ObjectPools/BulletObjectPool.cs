using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{

    public GameObject GetBulletFromPool()
    {
        // Loop through all the bullets:
        foreach (Transform childBullet in transform)
        {
            if(childBullet.name.Contains("Bullet"))
            {
                bool bulletReady = true;

                // Loop through all the particles, if playing means not ready:
                foreach (ParticleSystem childParticleSystem in childBullet
                    .GetComponentsInChildren<ParticleSystem>())
                {
                    if (childParticleSystem.isPlaying)
                    {
                        bulletReady = false;
                    }
                }

                // Return a ready one:
                if(bulletReady == true)
                {
                    Debug.Log("Bullet returned from pool: " + childBullet.name);
                    return childBullet.gameObject;
                }
            }
        }

        return null;
    }
}
