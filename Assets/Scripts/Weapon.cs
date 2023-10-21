using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TriggerPullInformation
{
    public float pulledAt;
    public float durationHeld;
    public bool triggered;
}

public class Weapon : MonoBehaviour
{
    public Transform BarrelEnd;
    public Bullet BulletObj;

    public void PullTrigger(TriggerPullInformation pullInformation)
    {
        if (pullInformation.durationHeld == 0)
        {
            var newBullet = Instantiate(BulletObj, BarrelEnd.position, BarrelEnd.rotation);
            newBullet.Fire();
        }
        
    }
}
