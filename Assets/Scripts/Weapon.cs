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
    [Header("Shooting")]
    public LayerMask Shootable;
    public float Range = 10f;
    public float HitForce = 10f;
    
    public Transform BarrelEnd;
    public Bullet BulletObj;
    
    [Header("Effects")]
    public ParticleSystem MuzzleFlash;

    public void PullTrigger(TriggerPullInformation pullInformation)
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        var hit = false;
        if (pullInformation.durationHeld == 0)
        {
            MuzzleFlash.time = 0;
            MuzzleFlash.Play(true);
            // var newBullet = Instantiate(BulletObj, BarrelEnd.position, BarrelEnd.rotation);
            
            if (Physics.Raycast(ray, out var hitInfo, Range, Shootable))
            {
                hit = true;
                if (hitInfo.rigidbody)
                {
                    hitInfo.rigidbody.AddForceAtPosition(ray.direction * HitForce, hitInfo.point, ForceMode.Impulse);
                }

                if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    var ghost = hitInfo.collider.gameObject.GetComponentInParent<GhostBehaviour>();
                    if (ghost)
                    {
                        ghost.TakeDamage(2);
                    }
                }
                
                Debug.Log($"Shot {hitInfo.transform.gameObject.name}", hitInfo.transform);
            }
        }
        
        Debug.DrawRay(ray.origin, ray.direction*Range, hit ? Color.green : Color.red);

    }
}
