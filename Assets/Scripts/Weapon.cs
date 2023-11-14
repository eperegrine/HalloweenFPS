using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TriggerPullInformation
{
    public float pulledAt;
    public float durationHeld;
    public bool triggered;
}

public enum WeaponState
{
    Normal,
    Reloading
}

public class Weapon : MonoBehaviour
{
    [Header("Shooting")]
    public LayerMask Shootable;
    public float Range = 100f;
    public float HitForce = 10f;
    public float FireRate = .2f;
    
    public Transform BarrelEnd;
    public Bullet BulletObj;

    [Header("Ammo")] 
    public int MaxAmmo = 240;
    public int ClipSize = 10;
    public float ReloadDuration = 1;

    private int _currentAmmo = 30;
    private int _currentClip;
    
    
    [Header("Effects")]
    public ParticleSystem MuzzleFlash;

    public AudioClip FireSound;
    public AudioSource FireAudioSource;

    private Animator _animator;
    private static readonly int AnimatorFireTrigger = Animator.StringToHash("Fire");
    private static readonly int AnimatorReloadTrigger = Animator.StringToHash("Reload");
    private WeaponState _currentState = WeaponState.Normal;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (FireAudioSource == null) FireAudioSource = GetComponent<AudioSource>();
    }

    private float LastFiredAt = 0f;
    
    public void PullTrigger(TriggerPullInformation pullInformation)
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        var hit = false;

        Debug.Log($"[FIRE] Ammo: {_currentAmmo}, Clip {_currentClip}");

        
        if (_currentState == WeaponState.Normal && _currentClip <= 0 && _currentAmmo >= 1)
        {
            Reload();
        }
        
        if (_currentState == WeaponState.Normal && _currentClip > 0 && pullInformation.durationHeld == 0 && Time.time-LastFiredAt >= FireRate)
        {
            //Handle Logic and Fire Events
            MuzzleFlash.time = 0;
            MuzzleFlash.Play(true);
            _animator.SetTrigger(AnimatorFireTrigger);
            LastFiredAt = Time.time;
            FireAudioSource.PlayOneShot(FireSound);
            _currentClip--;
            
            //Simulate Bullet via raycast
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

    public void Reload()
    {
        _currentState = WeaponState.Reloading;
        _animator.SetTrigger(AnimatorReloadTrigger);
        Invoke(nameof(Reload_Actual), ReloadDuration);
    }
    
    /// <summary>
    /// This sets the clip values for a reload, use Reload to trigger player animation and state change
    /// </summary>
    public void Reload_Actual()
    {
        _currentState = WeaponState.Normal;
        var clipDeficit = ClipSize - _currentClip;
        if (clipDeficit <= _currentAmmo)
        {
            _currentAmmo -= clipDeficit;
            _currentClip += clipDeficit;
        }
        else
        {
            _currentClip += _currentAmmo;
            _currentAmmo = 0;
        }
        Debug.Log($"Ammo: {_currentAmmo}, Clip {_currentClip}");
    }
}
